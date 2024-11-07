using Book.API.Models.Users;

namespace Book.API.Models.Tokens;

public class RefreshToken
{
    public long Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}
