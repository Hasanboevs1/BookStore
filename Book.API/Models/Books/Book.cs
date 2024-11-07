using Book.API.Models.Categories;

namespace Book.API.Models.Books;

public class Book
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public decimal Price { get; set; }
    public string Url { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public Category Category { get; set; }
}
