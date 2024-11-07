using Book.API.Contexts;
using Book.API.DTOs.Categories;
using Book.API.Exceptions;
using Book.API.Interfaces;
using Book.API.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Services;

public class CategoryService : ICategoryService
{
    private readonly Context context;
    public CategoryService(Context _context) => context = _context;
    public async Task<Category> GetAsync(int id)
    {
        var model = await context.Categories.FindAsync(id);
        if (model == null)
            throw new CustomException(404, "category_not_found");
        return model;
    }
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await context.Categories.ToListAsync();
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var model = await context.Categories.FindAsync(id);
        if (model == null)
            throw new CustomException(404, "category_not_found");
        context.Remove(model);
        return true;
    }
    public async Task<Category> AddAsync(CategoryDTO dto)
    {
        var model = new Category
        {
            Name = dto.Name
        };  

        await context.AddAsync(model);
        await context.SaveChangesAsync();

        return model;

    }
    public async Task<Category> UpdateAsync(int id, CategoryDTO dto)
    {
        var model = await context.Categories.FindAsync(id);
        if (model == null)
            throw new CustomException(404, "category_not_found");
        model.Name = dto.Name;
        await context.SaveChangesAsync();
        return model;
    }
}
