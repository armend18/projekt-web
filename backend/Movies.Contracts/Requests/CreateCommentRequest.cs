namespace Movies.Contracts.Requests;

public class CreateCommentRequest
{
    public required string Text { get; set; }
    public required Guid MovieId { get; set; }
    public Guid? ParentCommentId { get; set; }
}