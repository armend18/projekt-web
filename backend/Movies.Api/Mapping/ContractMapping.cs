using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

public static class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            // Auto-generate slug
            Slug = $"{request.Title.ToLower().Replace(" ", "-")}-{request.YearOfRelease}",
            Title = request.Title,
            Description = request.Description,
            YearOfRelease = request.YearOfRelease,
            RunTime = request.RunTime,
            Age = request.Age,
            Country = request.Country,
            Rating = request.Rating,
            Cover = request.Cover,
            VideoLink = request.VideoLink,
            DateCreated = DateOnly.FromDateTime(DateTime.Now),

            // 🔥 IMPORTANT: Initialize EMPTY lists here.
            // We will fill these in the Controller logic to prevent duplicates.
            Movie_Genres = new List<MovieGenres>(),
            Movie_Casts = new List<MovieCast>(),
            Movie_Director = new List<MovieDirector>()
        };
    }

    public static Movie MapToMovie(this UpdateMovieRequest request)
    {
        return new Movie
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            YearOfRelease = request.YearOfRelease,
            RunTime = request.RunTime,
            Age = request.Age,
            Country = request.Country,
            Rating = request.Rating,
            Cover = request.Cover,
            VideoLink = request.VideoLink,
            Slug = null
        };
    }

    public static MovieResponse MapToResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Slug = movie.Slug,
            Title = movie.Title,
            Description = movie.Description,
            YearOfRelease = movie.YearOfRelease,
            RunTime = movie.RunTime,
            Age = movie.Age,
            Country = movie.Country,
            Rating = movie.Rating,
            Cover = movie.Cover,
            VideoLink = movie.VideoLink,
            
            // Null coalescing (??) checks to prevent crashes if lists are empty
            Genres = movie.Movie_Genres?
                .Select(mg => mg.Genre.GenreName) 
                .ToList() ?? new List<string>(),

            Cast = movie.Movie_Casts?
                .Select(mc => mc.Cast.FullName) 
                .ToList() ?? new List<string>(),

            Directors = movie.Movie_Director?
                .Select(md => md.Director.FullName) 
                .ToList() ?? new List<string>()
        };
    }

    public static MovieCardResponse MapToCardResponse(this Movie movie)
    {
        return new MovieCardResponse
        {
            Id = movie.Id,
            Slug = movie.Slug,
            Title = movie.Title,
            Rating = movie.Rating,
            Cover = movie.Cover,
            YearOfRelease = movie.YearOfRelease,
            
            Genres = movie.Movie_Genres?
                .Select(mg => mg.Genre.GenreName)
                .Take(3)
                .ToList() ?? new List<string>()
        };
    }

    public static MovieCardsResponse MapToResponse(this IEnumerable<Movie> movies)
    {
        return new MovieCardsResponse
        {
            Items = movies.Select(MapToCardResponse)
        };
    }

    public static CommentResponse MapToResponseComment(this Comment c)
    {
        return new CommentResponse
        {
            Id = c.Id,
            Text = c.IsDeleted ? "[This comment has been deleted]" : c.Text,
            Username = c.IsDeleted ? "[Deleted]" :(c.User?.UserName ?? "Unknown User"), 
            UserId = c.UserId,
            CreatedAt = c.CreatedAt,
            ParentId = c.ParentCommentId,
            IsDeleted = c.IsDeleted,
            Replies = c.Replies.Select(MapToResponseComment).ToList()
        };
    }
}