namespace Movies.Contracts.Responses;

public class MovieResponse
{

        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required int YearOfRelease { get; init; }
        public required int RunTime { get; init; }
        public required int Age { get; init; }
        public required string Country { get; init; }
        
        public required IEnumerable<string> Directors { get; init; } = Enumerable.Empty<string>();
        public required IEnumerable<string> Cast { get; init; } = Enumerable.Empty<string>();
        public required IEnumerable<string> Genres { get; init; } = Enumerable.Empty<string>();
    
        public required float Rating { get; set; }
        public required string Cover { get; set; }
        public required string VideoLink { get; init; }
        public required string Slug { get; init; }
    

}
