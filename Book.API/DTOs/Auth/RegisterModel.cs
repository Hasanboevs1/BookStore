using System.ComponentModel.DataAnnotations;

namespace Book.API.DTOs.Auth;

public class RegisterModel
{

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
