using Book.API.DTOs.Categories;
using Book.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers;

[Route("api/categories")]
[ApiController]
[Authorize(Roles = "USER")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) => Ok(await _service.GetAsync(id));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id) => Ok(await _service.DeleteAsync(id));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CategoryDTO dto) => Ok(await _service.AddAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto) => Ok(await _service.UpdateAsync(id, dto));
}
