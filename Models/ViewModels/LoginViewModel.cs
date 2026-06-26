using System.ComponentModel.DataAnnotations;

namespace SportHub.Models.ViewModels;

// LoginViewModel for user authentication
public class LoginViewModel
{
    [Required]
    public string? Name { get; init; }

    [Required]
    public string? Password { get; init; }

    public string ReturnUrl { get; init; } = "/";
}