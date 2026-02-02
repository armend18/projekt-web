using Microsoft.AspNetCore.Identity;

namespace Movies.Application.Models;

public class User: IdentityUser
{
    public string Name { get; set; }
    public string LastName{ get; set; }
    
    public bool IsDeleted { get; set; }

}

