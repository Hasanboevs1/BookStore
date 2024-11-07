using Book.API.DTOs.Books;

namespace Book.API.Interfaces;

public interface IBookService
{
    Task<Models.Books.Book> GetAsync(long id);
    Task<IEnumerable<Models.Books.Book>> GetAllAsync();
    Task<Models.Books.Book> AddAsync(BookDTO book);
    Task<Models.Books.Book> UpdateAsync(long id, BookDTO book);
    Task<bool> DeleteAsync(long id);
}
