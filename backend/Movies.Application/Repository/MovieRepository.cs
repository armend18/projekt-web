//
// using Movies.Application.Models;
//
// namespace Movies.Application.Repository;
//
// public class MovieRepository : IMovieRepository
// {
//     
//
//
//
// public async Task<bool> CreateAsync(Movie movie)
// {
//     
//     return  false;
// }
//
// public async Task<Movie?> GetByIdAsync(Guid id)
// {
//     Movie movie=new Movie
//     {
//         Id = default,
//         Title = null,
//         Description = null,
//         YearOfRelease = 0,
//         RunTime = 0,
//         Age = 0,
//         Country = null,
//         Director = null,
//         CastList = new string[]
//         {
//         },
//         Genres = new string[]
//         {
//         },
//         Rating = 0,
//         Cover = null,
//         VideoLink = null
//     };
//
//     return movie ;
// }
//
//     public async Task<Movie?> GetBySlugAsync(string slug)
//     {
//         Movie movie=new Movie
//         {
//             Id = default,
//             Title = null,
//             Description = null,
//             YearOfRelease = 0,
//             RunTime = 0,
//             Age = 0,
//             Country = null,
//             Director = null,
//             CastList = new string[]
//             {
//             },
//             Genres = new string[]
//             {
//             },
//             Rating = 0,
//             Cover = null,
//             VideoLink = null
//         };
//        
//         return movie;
//     }
//
//     public async Task<IEnumerable<Movie>> GetAllAsync()
//     {
//
//        var movies = new List<Movie>();
//         return movies;
//     }
//
//
//     public async Task<bool> UpdateAsync(Movie movie)
//     {
//         return false;
//     }
//
//
// public async Task<bool> DeleteByIdAsync(Guid id)
// {
//  
//
//     return false;
// }
//
//
//
// public async Task<bool> ExistsByIdAsync(Guid id)
//     {
//        
//         return false;
//     }
// }