namespace Movies.Contracts.Requests;

public class CreateMovieRequest
{
 
    public required string Title { get; init; }
    
    public required string Description { get; set; }

    public required int YearOfRelease { get; set; }
    
    public required int RunTime { get; set; }
    
    public required int Age{get;set;}
    
    public required string Country { get; set; }
    
    public required string Director { get; set; }

    public required List<string> CastList { get; init; } = new();
    
    public required List<string> Genres { get; init; } = new();

    public required float Rating { get; set; } = 0;
    
    public required string Cover{get;set;}
    
    public required string VideoLink{get;init;}
}
