using Movies.Application.Data;
using Movies.Application.Models;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    
    public static void ClearAllData(MoviesContext context)
    {
        // Delete Join Tables First 
        context.MovieGenres.RemoveRange(context.MovieGenres);
        context.MovieCasts.RemoveRange(context.MovieCasts);     
        context.MovieDirectors.RemoveRange(context.MovieDirectors); 
        context.Comments.RemoveRange(context.Comments);           

        // Delete The Main Movies Table
        context.Movies.RemoveRange(context.Movies);

        // Delete The "Definition" Tables
        context.Genres.RemoveRange(context.Genres);       
        context.Casts.RemoveRange(context.Casts);         
        context.Directors.RemoveRange(context.Directors); 

        context.SaveChanges();
    }

    
    public static async Task SeedAsync(MoviesContext context)
    {
        // If data exists, do nothing
        if (context.Movies.Any()) return;

      
        
        var genresMap = new Dictionary<string, Genres>();
        var genreNames = new[] { 
            "Action", "Sci-Fi", "Thriller", "Drama", "Adventure", "Crime", "Comedy" 
        };
        foreach (var name in genreNames)
        {
            genresMap[name] = new Genres { GenreName = name, Description = "Standard Genre" };
        }

  
        var directorsMap = new Dictionary<string, Director>();
        var directorNames = new[] { 
            "Christopher Nolan", "Frank Darabont", "Bong Joon Ho", "Ridley Scott" 
        };
        foreach (var name in directorNames)
        {
            
            directorsMap[name] = new Director { FullName = name,  };
        }

       
        var castMap = new Dictionary<string, Cast>();
        var actorNames = new[] {
            "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page",
            "Tim Robbins", "Morgan Freeman", "Bob Gunton",
            "Matthew McConaughey", "Anne Hathaway", "Jessica Chastain",
            "Christian Bale", "Heath Ledger", "Aaron Eckhart",
            "Song Kang-ho", "Lee Sun-kyun", "Cho Yeo-jeong",
            "Russell Crowe", "Joaquin Phoenix", "Connie Nielsen"
        };
        foreach (var name in actorNames)
        {
            castMap[name] = new Cast { FullName = name};
        }

    
        context.Genres.AddRange(genresMap.Values);
        context.Directors.AddRange(directorsMap.Values);
        context.Casts.AddRange(castMap.Values); 

     

        var movies = new List<Movie>();

        // 1. Inception
        movies.Add(CreateMovie(
            id: new Guid("11111111-1111-1111-1111-111111111111"),
            title: "Inception",
            year: 2010, age: 13, runtime: 148, rating: 8.8f,
            country: "USA",
            cover: "https://filmartgallery.com/cdn/shop/files/Inception-Vintage-Movie-Poster-Original.jpg?v=1738912645",
            video: "https://www.youtube.com/embed/YoHD9XEInc0",
            desc: "A skilled thief is offered a chance to have his past crimes forgiven by implanting another person's idea into a target's subconscious.",
            directorName: "Christopher Nolan",
            castNames: new[] { "Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page" },
            genreNames: new[] { "Action", "Sci-Fi", "Thriller" },
            genresMap, directorsMap, castMap
        ));

        // 2. Shawshank Redemption
        movies.Add(CreateMovie(
            id: new Guid("22222222-2222-2222-2222-222222222222"),
            title: "The Shawshank Redemption",
            year: 1994, age: 16, runtime: 142, rating: 9.3f,
            country: "USA",
            cover: "https://image.tmdb.org/t/p/original/5OWFF1DhvYVQiX1yUrBE9CQqO5t.jpg",
            video: "https://www.youtube.com/embed/PLl99DlL6b4",
            desc: "Two imprisoned men bond over years, finding solace and eventual redemption through acts of common decency.",
            directorName: "Frank Darabont",
            castNames: new[] { "Tim Robbins", "Morgan Freeman", "Bob Gunton" },
            genreNames: new[] { "Drama" },
            genresMap, directorsMap, castMap
        ));

        // 3. Interstellar
        movies.Add(CreateMovie(
            id: new Guid("33333333-3333-3333-3333-333333333333"),
            title: "Interstellar",
            year: 2014, age: 12, runtime: 169, rating: 8.6f,
            country: "USA",
            cover: "https://m.media-amazon.com/images/I/61ASebTsLpL._AC_UF1000,1000_QL80_.jpg",
            video: "https://www.youtube.com/embed/zSWdZVtXT7E",
            desc: "A group of explorers travels through a wormhole in space in an attempt to ensure humanity's survival.",
            directorName: "Christopher Nolan",
            castNames: new[] { "Matthew McConaughey", "Anne Hathaway", "Jessica Chastain" },
            genreNames: new[] { "Adventure", "Drama", "Sci-Fi" },
            genresMap, directorsMap, castMap
        ));

        // 4. The Dark Knight
        movies.Add(CreateMovie(
            id: new Guid("44444444-4444-4444-4444-444444444444"),
            title: "The Dark Knight",
            year: 2008, age: 13, runtime: 152, rating: 9.0f,
            country: "USA",
            cover: "https://storage.googleapis.com/pod_public/750/257216.jpg",
            video: "https://www.youtube.com/embed/EXeTwQWrcwY",
            desc: "Batman faces the Joker, a criminal mastermind who plunges Gotham City into chaos.",
            directorName: "Christopher Nolan",
            castNames: new[] { "Christian Bale", "Heath Ledger", "Aaron Eckhart" },
            genreNames: new[] { "Action", "Crime", "Drama" },
            genresMap, directorsMap, castMap
        ));

        // 5. Parasite
        movies.Add(CreateMovie(
            id: new Guid("55555555-5555-5555-5555-555555555555"),
            title: "Parasite",
            year: 2019, age: 16, runtime: 132, rating: 8.6f,
            country: "South Korea",
            cover: "https://image.tmdb.org/t/p/original/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg",
            video: "https://www.youtube.com/embed/5xH0HfJHsaY",
            desc: "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.",
            directorName: "Bong Joon Ho",
            castNames: new[] { "Song Kang-ho", "Lee Sun-kyun", "Cho Yeo-jeong" },
            genreNames: new[] { "Comedy", "Drama", "Thriller" },
            genresMap, directorsMap, castMap
        ));

        // 6. Gladiator
        movies.Add(CreateMovie(
            id: new Guid("66666666-6666-6666-6666-666666666666"),
            title: "Gladiator",
            year: 2000, age: 16, runtime: 155, rating: 8.5f,
            country: "USA",
            cover: "https://i.ebayimg.com/images/g/OcEAAOSwHgdmFp-N/s-l1200.jpg",
            video: "https://www.youtube.com/embed/P5ieIbInFpg",
            desc: "A betrayed Roman general seeks revenge against the corrupt emperor who murdered his family and sent him into slavery.",
            directorName: "Ridley Scott",
            castNames: new[] { "Russell Crowe", "Joaquin Phoenix", "Connie Nielsen" },
            genreNames: new[] { "Action", "Adventure", "Drama" },
            genresMap, directorsMap, castMap
        ));

        context.Movies.AddRange(movies);
        await context.SaveChangesAsync();
    }

    // --- HELPER METHOD ---
    private static Movie CreateMovie(
        Guid id, string title, int year, int age, int runtime, float rating, 
        string country, string cover, string video, string desc,
        string directorName, string[] castNames, string[] genreNames,
        Dictionary<string, Genres> gMap, 
        Dictionary<string, Director> dMap, 
        Dictionary<string, Cast> cMap) 
    {
        return new Movie
        {
            
            Id = id,
            Title = title,
            // Generate the slug from the title (e.g., "Inception" -> "inception")
            Slug = title.ToLower().Replace(" ", "-"), 
            Description = desc,
            YearOfRelease = year,
            RunTime = runtime,
            Age = age,
            Country = country,
            Rating = rating,
            Cover = cover,
            VideoLink = video,
            DateCreated = DateOnly.FromDateTime(DateTime.Now),


            Movie_Director = new List<MovieDirector>
            {
                new MovieDirector
                {
                    Director = dMap[directorName]
                }
            },


            Movie_Genres = genreNames.Select(g => new MovieGenres
                {
                    Genre = gMap[g]
                })
                .ToList(),


            Movie_Casts = castNames.Select(c => new MovieCast
                {
                    Cast = cMap[c],
                })
                .ToList(),
           
        };
    }
}