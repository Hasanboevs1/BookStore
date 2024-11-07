using Book.API.Models.Payments;
using Book.API.Models.Roles;
using Book.API.Models.Tokens;

namespace Book.API.Models.Users;

public class User
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Role Role { get; set; } = Role.USER;
    public ICollection<Models.Books.Book> Books { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    
}
