namespace Movies.Application.Models;

public class MovieCast
{
    public int Id { get; set; }
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }
    public int CastId { get; set; }
    public Cast Cast { get; set; }
    
}