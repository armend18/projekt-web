using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movies.Application; // Ensure this namespace contains Initializer
using Movies.Application.Data;
using Movies.Application.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add EF Core DbContext
builder.Services.AddDbContext<MoviesContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Movies.Api")
    ));

// Configure CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => 
            origin.StartsWith("http://localhost") || 
            origin.StartsWith("https://127.0.0.1") || 
            origin.StartsWith("http://127.0.0.1"))
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Token validation parameters
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(configuration["JWT:Secret"])
    ),
    ValidateIssuer = true,
    ValidIssuer = configuration["JWT:ValidIssuer"],
    ValidateAudience = true,
    ValidAudience = configuration["JWT:ValidAudience"],
    ValidateLifetime = true,
};
builder.Services.AddSingleton(tokenValidationParameters);

// Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<MoviesContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
    

    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
     
        throw new Exception("Database is not fully migrated. Please run 'dotnet ef database update'");
    }

    }



// Middleware pipeline

Initializer.SeedRolesAsync(app); 

// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<MoviesContext>();
//     
//     // Optional: Wipe the DB every restart during development
//     DbSeeder.ClearAllData(context); 
//     
//     // Populate the data
//     await DbSeeder.SeedAsync(context);
// }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS BEFORE auth and controllers
app.UseCors("AllowFrontend");

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// HTTPS redirection only in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
else
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        Console.WriteLine("WARNING: HTTPS Redirection Disabled for Local Testing.");
    });
}

// Map controllers
app.MapControllers();

// Run the app
app.Run();