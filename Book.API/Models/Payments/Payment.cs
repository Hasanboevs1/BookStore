using Book.API.Models.Statuses;
using Book.API.Models.Users;

namespace Book.API.Models.Payments;

public class Payment
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; }
    public string TransactionId { get; set; }
}
