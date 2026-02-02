using Microsoft.AspNetCore.Mvc;

namespace Movies.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string GetCards = $"{Base}/cards";
        public const string GetByGenre = $"{Base}/genre/{{genre}}";
        public const string GetTopMovies = $"{Base}/topmovies";
        public const string GetLatestMovies = $"{Base}/latest";
        public const string GetLatestReviews = $"{Base}/topratedmovies";
        public const string GetLatestUsers= $"{Base}/latestusers";
        


    }
    public static class Users
    {
        private const string Base = $"{ApiBase}/authentication";
        public const string Authenticate = Base;
        public const string Register = Base + "/register";
        public const string Login = Base + "/login";
        public const string RefreshToken = Base + "/refreshtoken";
    }

    
    
    
    
}