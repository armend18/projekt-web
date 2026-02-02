namespace Movies.Application.Models;

public class Cast
{
    public int CastId { get; set; }
    public String FullName { get; set; }
    public String DateOfBirth { get; }
    public String Country {get;}
    public List<MovieCast>  Movie_Casts{get;}
    
}