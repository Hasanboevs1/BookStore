using Book.API.Contexts;
using Book.API.DTOs.Books;
using Book.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Services;

public class BookService : IBookService
{
    private readonly Context context;
    public BookService(Context context)
    {
        this.context = context;
    }

    public async Task<Models.Books.Book> AddAsync(BookDTO book)
    {
        var model = new Models.Books.Book()
        {
            Title = book.Title,
            Description = book.Description,
            Author = book.Author,
            Price = book.Price,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        model.Url = await ImageUrlGenerator(book.Image);
        context.Add(model);
        await context.SaveChangesAsync();

        return model;

    }

    private async Task<string> ImageUrlGenerator(IFormFile image)
    {
        var imagePath = Path.Combine("wwwroot/Images", image.FileName);
        using (var stream = new FileStream(imagePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return $"/Images/{image.FileName}";
    }


   public async Task<bool> DeleteAsync(long id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
        {
            return false;
        }

        // Remove the image file if it exists
        var imagePath = Path.Combine("wwwroot", book.Url.TrimStart('/'));
        if (File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Models.Books.Book>> GetAllAsync()
    {
        return  await context.Books.ToListAsync();
    }

    public async Task<Models.Books.Book> GetAsync(long id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
        {
            throw new Exception("book_not_found");
        }
        return book;
    }

    public async Task<Models.Books.Book> UpdateAsync(long id, BookDTO book)
    {
        var model = await context.Books.FindAsync(id);
        if (model == null)
        {
            throw new Exception("not_found");
        }

        // If a new image is provided, delete the old image
        if (book.Image != null)
        {
            var oldImagePath = Path.Combine("wwwroot", model.Url.TrimStart('/'));
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }

            // Save the new image and update the URL
            model.Url = await ImageUrlGenerator(book.Image);
        }

        // Update the book properties
        model.Title = book.Title;
        model.Description = book.Description;
        model.Author = book.Author;
        model.Price = book.Price;
        model.UpdatedDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return model;
    }
}
