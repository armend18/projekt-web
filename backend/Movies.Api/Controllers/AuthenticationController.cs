using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.Api;
using Movies.Application.AuthModels;
using Movies.Application.Data;
using Movies.Application.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly MoviesContext _dbContext;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthenticationController(UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        MoviesContext dbContext,
        TokenValidationParameters tokenValidationParameters)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _dbContext = dbContext;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpPost(ApiEndpoints.Users.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterVM payload)
    {
        if (!ModelState.IsValid) return BadRequest("Please provide all required fields");

        var userExist = await _userManager.FindByEmailAsync(payload.Email);
        if (userExist != null) return BadRequest($"User {payload.Email} already exists");

        User user = new User()
        {
            Email = payload.Email,
            UserName = payload.Username,
            SecurityStamp = Guid.NewGuid().ToString(),
            Name = payload.Name,
            LastName = payload.LastName
        };

        var result = await _userManager.CreateAsync(user, payload.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { errors });
        }

        await _userManager.AddToRoleAsync(user, "User");
        return Created(nameof(Register), $"User {payload.Email} created");
    }

    [HttpPost(ApiEndpoints.Users.Login)]
    public async Task<IActionResult> Login([FromBody] LoginVM payload)
    {
        if (!ModelState.IsValid) return BadRequest("Please provide all required fields");

        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, payload.Password))
        {
            var tokenValue = await GenerateJwtTokenAsync(user, null);
            return Ok(tokenValue);
        }
        return Unauthorized();
    }

    [HttpPost(ApiEndpoints.Users.RefreshToken)]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVm payload)
    {
        try
        {
            var result = await VerifyAndGenerateTokenAsync(payload);
            if (result == null) return BadRequest("Invalid tokens");
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVm payload)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            // Validate signature while ignoring expiration
            var tokenVerification = _tokenValidationParameters.Clone();
            tokenVerification.ValidateLifetime = false;

            var tokenInVerification = jwtTokenHandler.ValidateToken(payload.Token, tokenVerification, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (result == false) return null;
            }

            var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = UnixTimeStampToDateTimeInUTC(utcExpiryDate);

            var dbRefreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);

            if (dbRefreshToken == null) throw new Exception("Refresh Token does not exist");
            if (dbRefreshToken.IsRevoked) throw new Exception("Refresh Token has been revoked");
            if (dbRefreshToken.DateExpire < DateTime.UtcNow) throw new Exception("Refresh Token expired");

            var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (dbRefreshToken.JwtId != jti) throw new Exception("Token does not match");

            var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);
            return await GenerateJwtTokenAsync(dbUserData, dbRefreshToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Something went wrong: " + ex.Message);
        }
    }

    private async Task<AuthResultVM> GenerateJwtTokenAsync(User user, RefreshToken existingRefreshToken)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(10),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        if (existingRefreshToken != null)
        {
            existingRefreshToken.IsRevoked = true;
            _dbContext.RefreshTokens.Update(existingRefreshToken);
        }

        var newRefreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = user.Id,
            DateAdded = DateTime.UtcNow,
            DateExpire = DateTime.UtcNow.AddMonths(6),
            Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
        };

        await _dbContext.RefreshTokens.AddAsync(newRefreshToken);
        await _dbContext.SaveChangesAsync();

        return new AuthResultVM()
        {
            Token = jwtToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = token.ValidTo
        };
    }

    private DateTime UnixTimeStampToDateTimeInUTC(long unixTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
        return dateTimeVal;
    }
}