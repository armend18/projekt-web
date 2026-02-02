using Movies.Application.AuthModels; // Import where your User class is defined

namespace Movies.Application.Models;

public class Comment
{
    public Guid Id { get; set; }
    public required string Text { get; set; }

    public required string UserId { get; set; } 
    public User User { get; set; } 
    public required Guid MovieId { get; set; }
    public Movie? Movie { get; set; }

    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
}