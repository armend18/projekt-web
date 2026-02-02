namespace Movies.Application.Models;

public class Genres
{
    public int Id { get; set; }
    
    public required string GenreName { get; init; } 
    public required string Description { get; init; }
    
    public List<MovieGenres> Movie_Genres { get; set; } = new();
}