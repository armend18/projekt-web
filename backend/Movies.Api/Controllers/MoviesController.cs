using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Mapping;
using Movies.Application.Data;
using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();

        // Genres logic
        foreach (var genreName in request.Genres)
        {
            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == genreName);
            if (existingGenre != null)
            {
                movie.Movie_Genres.Add(new MovieGenres { MovieId = movie.Id, GenreId = existingGenre.Id, Genre = existingGenre });
            }
            else
            {
                movie.Movie_Genres.Add(new MovieGenres { MovieId = movie.Id, Genre = new Genres { GenreName = genreName, Description = "API" } });
            }
        }

        // Cast logic
        foreach (var actorName in request.CastList)
        {
            var existingActor = await _context.Casts.FirstOrDefaultAsync(c => c.FullName == actorName);
            if (existingActor != null)
            {
                movie.Movie_Casts.Add(new MovieCast { MovieId = movie.Id, CastId = existingActor.CastId, Cast = existingActor });
            }
            else
            {
                movie.Movie_Casts.Add(new MovieCast { MovieId = movie.Id, Cast = new Cast { FullName = actorName } });
            }
        }

        // Director logic
        if (!string.IsNullOrWhiteSpace(request.Director))
        {
            var existingDirector = await _context.Directors.FirstOrDefaultAsync(d => d.FullName == request.Director);
            if (existingDirector != null)
            {
                movie.Movie_Director.Add(new MovieDirector { MovieId = movie.Id, DirectorId = existingDirector.Id, Director = existingDirector });
            }
            else
            {
                movie.Movie_Director.Add(new MovieDirector { MovieId = movie.Id, Director = new Director { FullName = request.Director } });
            }
        }

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var query = GetMoviesWithDetails();

        var movie = Guid.TryParse(idOrSlug, out var id) 
            ? await query.FirstOrDefaultAsync(m => m.Id == id) 
            : await query.FirstOrDefaultAsync(m => m.Slug == idOrSlug);

        if (movie is null) return NotFound();
        
        return Ok(movie.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await GetMoviesWithDetails().ToListAsync();
        return Ok(movies.Select(m => m.MapToResponse()));
    }

    [HttpGet(ApiEndpoints.Movies.GetCards)]
    public async Task<IActionResult> GetCards()
    {
        // Load only necessary data for performance
        var movies = await _context.Movies
            .AsNoTracking()
            .Include(m => m.Movie_Genres).ThenInclude(mg => mg.Genre)
            .OrderByDescending(m => m.DateCreated)
            .ToListAsync();

        return Ok(movies.MapToResponse());
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie is null) return NotFound();

        _context.Entry(movie).CurrentValues.SetValues(request);
        await _context.SaveChangesAsync();

        return Ok(movie.MapToResponse());
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie is null) return NotFound();
        
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet(ApiEndpoints.Movies.GetByGenre)]
    public async Task<IActionResult> GetByGenre([FromRoute] string genre)
    {
        var filteredMovies = await GetMoviesWithDetails()
            .Where(m => m.Movie_Genres.Any(mg => mg.Genre.GenreName == genre))
            .ToListAsync();
            
        return Ok(filteredMovies.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetTopMovies)]
    public async Task<IActionResult> GetTopMovies([FromQuery] int limit = 5)
    {
        var topMovies = await GetMoviesWithDetails()
            .OrderByDescending(m => m.Rating)
            .Take(limit)
            .ToListAsync();
            
        return Ok(topMovies.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetLatestMovies)]
    public async Task<IActionResult> GetLatestMovies([FromQuery] int limit = 5)
    {
        var latestMovies = await GetMoviesWithDetails()
            .OrderByDescending(m => m.DateCreated)
            .Take(limit)
            .ToListAsync();

        return Ok(latestMovies.MapToResponse());
    }

    private IQueryable<Movie> GetMoviesWithDetails()
    {
        return _context.Movies
            .AsSplitQuery()
            .Include(m => m.Movie_Genres).ThenInclude(mg => mg.Genre)
            .Include(m => m.Movie_Casts).ThenInclude(mc => mc.Cast)
            .Include(m => m.Movie_Director).ThenInclude(md => md.Director)
            .Include(m => m.Comments).ThenInclude(c => c.User);
    }
}