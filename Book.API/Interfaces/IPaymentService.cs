using Book.API.Models.Payments;

namespace Book.API.Interfaces;

public interface IPaymentService
{
    Task<Payment> CreatePaymentAsync(int userId, decimal amount, string currency, int paymentMethodId);
    Task<Payment?> CompletePaymentAsync(string paymentIntentId);
}
