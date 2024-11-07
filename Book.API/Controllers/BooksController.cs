using Book.API.DTOs.Books;
using Book.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers;

[Route("api/books")]
[ApiController]
[Authorize(Roles = "USER")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    public BooksController(IBookService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id) => Ok(await _service.GetAsync(id));

    [HttpPost]
    public async Task<IActionResult> Add([FromForm] BookDTO dto)
    {
        var book =  await _service.AddAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromForm] BookDTO dto) => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id) => Ok(await _service.DeleteAsync(id));
}
