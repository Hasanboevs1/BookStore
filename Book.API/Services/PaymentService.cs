using Book.API.Contexts;
using Book.API.Interfaces;
using Book.API.Models.Payments;
using Book.API.Models.Statuses;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Book.API.Services;

public class PaymentService : IPaymentService
{
    private readonly Context _context;
    private readonly string _stripeSecretKey;

    public PaymentService(Context context, IConfiguration configuration)
    {
        _context = context;
        _stripeSecretKey = configuration.GetValue<string>("Stripe:SecretKey")!;
    }

    public async Task<Payment> CreatePaymentAsync(int userId, decimal amount, string currency, int paymentMethodId)
    {
        StripeConfiguration.ApiKey = _stripeSecretKey;

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new Exception("User not found.");
        }

        var paymentMethodExists = await _context.PaymentMethods.AnyAsync(pm => pm.Id == paymentMethodId);
        if (!paymentMethodExists)
        {
            throw new Exception("Payment method not found.");
        }

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Stripe expects amount in the smallest currency unit (e.g., cents)
            Currency = currency,
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        var payment = new Payment
        {
            UserId = userId,
            Amount = amount,
            Currency = currency,
            CreatedAt = DateTime.UtcNow,
            Status = PaymentStatus.Pending,
            TransactionId = paymentIntent.Id,
            PaymentMethodId = paymentMethodId
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<Payment?> CompletePaymentAsync(string paymentIntentId)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.TransactionId == paymentIntentId);

        if (payment != null)
        {
            // Assuming you've validated the payment status with Stripe API here
            payment.Status = PaymentStatus.Completed;
            await _context.SaveChangesAsync();
        }

        return payment;
    }

}
