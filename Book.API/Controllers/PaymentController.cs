using Book.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers;

[Route("api/payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest paymentRequest)
    {
        if (paymentRequest == null || paymentRequest.Amount <= 0)
        {
            return BadRequest("Invalid payment request.");
        }

        var payment = await _paymentService.CreatePaymentAsync(
            paymentRequest.UserId,
            paymentRequest.Amount,
            paymentRequest.Currency,
            paymentRequest.PaymentMethodId
        );

        return Ok(new { clientSecret = payment.TransactionId });
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompletePaymentAsync(string paymentIntentId)
    {
        // Call the service to update the payment status
        var payment = await _paymentService.CompletePaymentAsync(paymentIntentId);

        if (payment == null)
        {
            return NotFound("Payment not found.");
        }

        return Ok(payment);
    }
}

public class PaymentRequest
{
    public int UserId { get; set; }
    public decimal Amount { get; set; }  // Amount in dollars
    public string Currency { get; set; } // Currency (e.g., "usd")
    public int PaymentMethodId { get; set; }
}
