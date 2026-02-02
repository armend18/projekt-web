namespace Movies.Contracts.Responses;

public class MovieCardResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public required float Rating { get; init; }
    public required string Cover { get; init; }
    public required int YearOfRelease { get; init; }
    
    public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
}


public class MovieCardsResponse
{
    public required IEnumerable<MovieCardResponse> Items { get; init; } = Enumerable.Empty<MovieCardResponse>();
}