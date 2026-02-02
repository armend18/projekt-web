namespace Movies.Application.Models;

public class MovieGenres
{
    public int Id { get; set; }
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }
    public int GenreId { get; set; }
    public Genres Genre { get; set; }
    
}