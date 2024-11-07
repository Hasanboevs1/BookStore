using Book.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "USER")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("id")]
    public async Task<IActionResult> GetAsync(long id) => Ok(await _service.GetAsync(id));

    [HttpDelete("id")]
    public async Task<IActionResult> Remove(long id) => Ok(await _service.DeleteAsync(id));
}
