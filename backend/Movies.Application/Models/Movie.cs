using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public class Movie
{
    public required Guid Id { get; init; }
    
    public required string Title { get; init; }


    public required string Slug { get; set; } 

    public required string Description { get; set; }

    public required int YearOfRelease { get; set; }
    
    public required int RunTime { get; set; }
    
    public required int Age { get; set; }
    
    public required string Country { get; set; }
    
    public List<MovieDirector> Movie_Director { get; set; } = new();
    
    public List<MovieGenres> Movie_Genres { get; set; } = new();
    
    public List<MovieCast> Movie_Casts { get; set; } = new();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public required float Rating { get; set; } = 0;
    
    public required string Cover { get; set; }
    
    public required string VideoLink { get; init; }
    
    public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}