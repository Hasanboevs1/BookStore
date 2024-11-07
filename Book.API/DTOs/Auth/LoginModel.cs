﻿using System.ComponentModel.DataAnnotations;

namespace Book.API.DTOs.Auth;

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
