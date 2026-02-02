public class CommentResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Username { get; set; }
    public string UserId { get; set; } 
    public DateTime CreatedAt { get; set; }
    public Guid? ParentId { get; set; }
    public bool IsDeleted { get; set; }
    public List<CommentResponse> Replies { get; set; } = new();
}