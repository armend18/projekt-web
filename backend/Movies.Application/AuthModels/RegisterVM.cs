using System.ComponentModel.DataAnnotations;

namespace Movies.Application.AuthModels;

public class RegisterVM
{
    [Required(ErrorMessage = "Username is required")]
    public String  Username { get; set; }
   [Required(ErrorMessage = "Password is required")]
    public String Password { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public String Email { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public String Name { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    public String LastName { get; set; }

}