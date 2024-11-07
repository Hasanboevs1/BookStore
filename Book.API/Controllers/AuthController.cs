using Book.API.Contexts;
using Book.API.DTOs.Auth;
using Book.API.Models.Roles;
using Book.API.Models.Users;
using Book.API.Services;
using Book.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Book.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtServices _service;
    private readonly Context context;
    public AuthController(JwtServices service, Context context)
    {
        _service = service;
        this.context = context;
    }

    [HttpPost("register")]
    [SwaggerOperation(Summary = "Registers a new user")]
    [SwaggerResponse(200, "User registered successfully")]
    [SwaggerResponse(400, "User already exists")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user != null)
            return BadRequest("User Already Exists");

        var newUser = new User
        {
            Id = context.Users.Count() + 1,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Email = model.Email,
            Password = HashingUtility.HashPassword(model.Password),
            Role = Role.USER,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        return Ok("Registered Successfully");  
    }

    [HttpPost("login")]
    [SwaggerOperation(Summary = "Logs in a user and returns a JWT token")]
    [SwaggerResponse(200, "Token generated successfully", typeof(string))]
    [SwaggerResponse(401, "Invalid credentials")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await context.Users
        .Include(u => u.RefreshTokens) 
        .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null || !HashingUtility.VerifyPassword(model.Password, user.Password))
            return Unauthorized("Invalid credentials");

        var accessToken = _service.GenerateToken(user.Id, user.Email, user.Role.ToString());
        var refreshToken = _service.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
    }

    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refreshes the access token using a refresh token")]
    [SwaggerResponse(200, "Token refreshed successfully", typeof(string))]
    [SwaggerResponse(400, "Invalid or expired refresh token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model)
    {
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == model.RefreshToken));

        if (user == null) return Unauthorized("Invalid refresh token");

        var refreshToken = user.RefreshTokens.Single(x => x.Token == model.RefreshToken);
        if (refreshToken.IsExpired || refreshToken.IsRevoked)
            return Unauthorized("Refresh token is expired or revoked");

        var newAccessToken = _service.GenerateToken(user.Id, user.Email, user.Role.ToString());
        var newRefreshToken = _service.GenerateRefreshToken();

        refreshToken.IsRevoked = true;
        refreshToken.Revoked = DateTime.UtcNow;
        user.RefreshTokens.Add(newRefreshToken);

        await context.SaveChangesAsync();

        return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token });
    }

}
