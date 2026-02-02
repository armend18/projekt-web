namespace Movies.Contracts.Responses;

public class MoviesResponse
{
    public required IEnumerable<MovieCardResponse> Items { get; init; } = Enumerable.Empty<MovieCardResponse>();
}
