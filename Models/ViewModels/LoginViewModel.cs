using System.ComponentModel.DataAnnotations;

namespace SportHub.Models.ViewModels;

// LoginViewModel for user authentication
public class LoginViewModel
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Password { get; set; }

    public string ReturnUrl { get; set; } = "/";
}