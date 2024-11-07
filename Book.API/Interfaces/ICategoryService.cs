using Book.API.DTOs.Categories;
using Book.API.Models.Categories;

namespace Book.API.Interfaces;

public interface ICategoryService
{
    Task<Category> GetAsync(int id);
    Task<IEnumerable<Category>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
    Task<Category> AddAsync(CategoryDTO dto);
    Task<Category> UpdateAsync(int id, CategoryDTO dto);
}
