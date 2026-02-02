using System.Security.AccessControl;

namespace Movies.Application.Models;

public class MovieDirector
{
    public int Id { get; set; }
    
    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }    
    public int DirectorId { get; set; }
    public Director Director { get; set; }
    
}