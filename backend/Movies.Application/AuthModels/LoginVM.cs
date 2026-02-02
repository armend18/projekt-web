using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace Movies.Application.AuthModels;

public class LoginVM
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    
}