using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Mapping;
using Movies.Application.Data;
using Movies.Application.Models;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class CommentsController : ControllerBase
{
    private readonly MoviesContext _context;
    private readonly UserManager<User> _userManager;

    public CommentsController(MoviesContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("api/comments")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Text = request.Text,
            MovieId = request.MovieId,
            ParentCommentId = request.ParentCommentId,
            UserId = userId, 
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return Ok(new { comment.Id });
    }

    [HttpDelete("api/comments/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var comment = await _context.Comments
            .Include(c => c.Replies) 
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null) return NotFound();

        var isAdmin = User.IsInRole("Admin");

        if (comment.UserId != userId && !isAdmin)
        {
            return Forbid();
        }

        bool performedHardDelete = false;
        Guid? parentIdToCheck = comment.ParentCommentId;

        if (comment.Replies.Any())
        {
            // Soft delete: hide text but preserve child relationship
            comment.IsDeleted = true;
        }
        else
        {
            // Hard delete: remove leaf node
            _context.Comments.Remove(comment);
            performedHardDelete = true;
        }

        await _context.SaveChangesAsync();

        if (performedHardDelete && parentIdToCheck != null)
        {
            await CleanUpOrphanedParents(parentIdToCheck.Value);
        }

        return Ok(new { message = "Comment processed successfully" });
    }

    private async Task CleanUpOrphanedParents(Guid parentId)
    {
        while (true)
        {
            var parent = await _context.Comments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == parentId);

            if (parent == null || !parent.IsDeleted || parent.Replies.Any()) 
                break;

            var grandParentId = parent.ParentCommentId;
            
            _context.Comments.Remove(parent);
            await _context.SaveChangesAsync();

            if (grandParentId == null) break;
            parentId = grandParentId.Value;
        }
    }

    [HttpGet("api/movies/{movieId}/comments")]
    public async Task<IActionResult> GetCommentsByMovie([FromRoute] Guid movieId)
    {
        var comments = await _context.Comments
            .Where(c => c.MovieId == movieId)
            .Include(c => c.User) 
            .Include(c => c.Replies)
            .ThenInclude(r => r.User) 
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        var topLevelComments = comments.Where(c => c.ParentCommentId == null).ToList();
        var response = topLevelComments.Select(c => c.MapToResponseComment());

        return Ok(response);
    }
}