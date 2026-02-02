using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Models;

namespace Movies.Application.Data;

public class MoviesContext : IdentityDbContext<User>
{
    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //many to many for directors and movies
        modelBuilder.Entity<MovieDirector>()
            .HasOne(a => a.Director)
            .WithMany(b => b.Movie_Director)
            .HasForeignKey(a => a.DirectorId);
        modelBuilder.Entity<MovieDirector>()
            .HasOne(m=>m.Movie)
            .WithMany(md=>md.Movie_Director)
            .HasForeignKey(a => a.MovieId);
        
        //many to many for genres and movies
        modelBuilder.Entity<MovieDirector>()
            .HasOne(a => a.Movie)
            .WithMany(b => b.Movie_Director)
            .HasForeignKey(a => a.MovieId);
        modelBuilder.Entity<MovieGenres>()
            .HasOne(b => b.Genre)
            .WithMany(b => b.Movie_Genres)
            .HasForeignKey(c => c.GenreId);
        
            //many to many for cast and movies
        modelBuilder.Entity<MovieCast>()
            .HasOne(b => b.Movie)
            .WithMany(b => b.Movie_Casts)
            .HasForeignKey(b => b.MovieId);
        modelBuilder.Entity<MovieCast>()
            .HasOne(b => b.Cast)
            .WithMany(b => b.Movie_Casts)
            .HasForeignKey(b => b.CastId);
        
            //delete all comments when movie deleted
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Movie)
            .WithMany(m => m.Comments)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
            //delete comment but not replyes
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);
            
        
        
        

    }
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<User> User=> Set<User>();
    
    public DbSet<Cast> Casts => Set<Cast>();
    
    public DbSet<Comment> Comments => Set<Comment>();
    
    public DbSet<Genres> Genres => Set<Genres>();

    public DbSet<Director> Directors => Set<Director>();
    public DbSet<MovieDirector> MovieDirectors => Set<MovieDirector>();
    
    public DbSet<MovieGenres> MovieGenres => Set<MovieGenres>();
    public DbSet<MovieCast> MovieCasts => Set<MovieCast>();
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
}