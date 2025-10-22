using AbacatePay;
using AbacatePay.Models.Webhook;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AbacatePay.Examples.WebhookHandler;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly ILogger<WebhookController> _logger;
    private readonly string _webhookSecret;

    public WebhookController(ILogger<WebhookController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _webhookSecret = configuration["AbacatePay:WebhookSecret"] ?? "";
    }

    [HttpPost("abacatepay")]
    public async Task<IActionResult> HandleWebhook()
    {
        try
        {
            // Read the raw request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var payload = await reader.ReadToEndAsync();

            // Get the signature from headers
            var signature = Request.Headers["X-AbacatePay-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(signature))
            {
                _logger.LogWarning("Webhook signature missing");
                return BadRequest("Signature missing");
            }

            // Verify the webhook signature
            if (!AbacatePayClient.VerifyWebhookSignature(payload, signature, _webhookSecret))
            {
                _logger.LogWarning("Invalid webhook signature");
                return Unauthorized("Invalid signature");
            }

            // Parse the webhook event
            var webhookEvent = JsonConvert.DeserializeObject<WebhookEvent>(payload);
            if (webhookEvent == null)
            {
                _logger.LogError("Failed to deserialize webhook event");
                return BadRequest("Invalid webhook data");
            }

            _logger.LogInformation("Received webhook event: {EventType} - {EventId}", 
                webhookEvent.Type, webhookEvent.Id);

            // Handle different event types
            await HandleWebhookEvent(webhookEvent);

            return Ok(new { message = "Webhook processed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook");
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task HandleWebhookEvent(WebhookEvent webhookEvent)
    {
        switch (webhookEvent.Type)
        {
            case WebhookEventType.PAYMENT_COMPLETED:
                await HandlePaymentCompleted(webhookEvent);
                break;

            case WebhookEventType.PAYMENT_FAILED:
                await HandlePaymentFailed(webhookEvent);
                break;

            case WebhookEventType.PAYMENT_CANCELLED:
                await HandlePaymentCancelled(webhookEvent);
                break;

            case WebhookEventType.PAYMENT_EXPIRED:
                await HandlePaymentExpired(webhookEvent);
                break;

            case WebhookEventType.REFUND_COMPLETED:
                await HandleRefundCompleted(webhookEvent);
                break;

            case WebhookEventType.REFUND_FAILED:
                await HandleRefundFailed(webhookEvent);
                break;

            default:
                _logger.LogWarning("Unhandled webhook event type: {EventType}", webhookEvent.Type);
                break;
        }
    }

    private async Task HandlePaymentCompleted(WebhookEvent webhookEvent)
    {
        var paymentData = JsonConvert.DeserializeObject<PaymentWebhookData>(webhookEvent.Data?.ToString() ?? "");
        if (paymentData?.Payment != null)
        {
            _logger.LogInformation("Payment completed: {PaymentId} - Amount: R$ {Amount}", 
                paymentData.Payment.Id, paymentData.Payment.Amount / 100.0);

            // Update your database, send confirmation email, etc.
            await UpdateOrderStatus(paymentData.Payment.Id, "paid");
            await SendPaymentConfirmation(paymentData.Payment);
        }
    }

    private async Task HandlePaymentFailed(WebhookEvent webhookEvent)
    {
        var paymentData = JsonConvert.DeserializeObject<PaymentWebhookData>(webhookEvent.Data?.ToString() ?? "");
        if (paymentData?.Payment != null)
        {
            _logger.LogInformation("Payment failed: {PaymentId}", paymentData.Payment.Id);

            // Update your database, notify customer, etc.
            await UpdateOrderStatus(paymentData.Payment.Id, "failed");
            await NotifyPaymentFailure(paymentData.Payment);
        }
    }

    private async Task HandlePaymentCancelled(WebhookEvent webhookEvent)
    {
        var paymentData = JsonConvert.DeserializeObject<PaymentWebhookData>(webhookEvent.Data?.ToString() ?? "");
        if (paymentData?.Payment != null)
        {
            _logger.LogInformation("Payment cancelled: {PaymentId}", paymentData.Payment.Id);

            // Update your database, restore inventory, etc.
            await UpdateOrderStatus(paymentData.Payment.Id, "cancelled");
            await RestoreInventory(paymentData.Payment);
        }
    }

    private async Task HandlePaymentExpired(WebhookEvent webhookEvent)
    {
        var paymentData = JsonConvert.DeserializeObject<PaymentWebhookData>(webhookEvent.Data?.ToString() ?? "");
        if (paymentData?.Payment != null)
        {
            _logger.LogInformation("Payment expired: {PaymentId}", paymentData.Payment.Id);

            // Update your database, clean up resources, etc.
            await UpdateOrderStatus(paymentData.Payment.Id, "expired");
            await CleanupExpiredPayment(paymentData.Payment);
        }
    }

    private async Task HandleRefundCompleted(WebhookEvent webhookEvent)
    {
        var refundData = JsonConvert.DeserializeObject<RefundWebhookData>(webhookEvent.Data?.ToString() ?? "");
        if (refundData?.Refund != null)
        {
            _logger.LogInformation("Refund completed: {RefundId} - Amount: R$ {Amount}", 
                refundData.Refund.Id, refundData.Refund.Amount / 100.0);

            // Update your database, send refund confirmation, etc.
            await UpdateRefundStatus(refundData.Refund.Id, "completed");
            await SendRefundConfirmation(refundData.Refund);
        }
    }

    private async Task HandleRefundFailed(WebhookEvent webhookEvent)
    {
        var refundData = JsonConvert.DeserializeObject<RefundWebhookData>(webhookEvent.Data?.ToString() ?? "");
        if (refundData?.Refund != null)
        {
            _logger.LogInformation("Refund failed: {RefundId}", refundData.Refund.Id);

            // Update your database, notify customer, etc.
            await UpdateRefundStatus(refundData.Refund.Id, "failed");
            await NotifyRefundFailure(refundData.Refund);
        }
    }

    // Business logic methods (implement according to your needs)
    private async Task UpdateOrderStatus(string paymentId, string status)
    {
        _logger.LogInformation("Updating order status for payment {PaymentId} to {Status}", paymentId, status);
        // Implement your database update logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task SendPaymentConfirmation(AbacatePay.Models.Payment.PaymentResponse payment)
    {
        _logger.LogInformation("Sending payment confirmation for payment {PaymentId}", payment.Id);
        // Implement your email/notification logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task NotifyPaymentFailure(AbacatePay.Models.Payment.PaymentResponse payment)
    {
        _logger.LogInformation("Notifying payment failure for payment {PaymentId}", payment.Id);
        // Implement your notification logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task RestoreInventory(AbacatePay.Models.Payment.PaymentResponse payment)
    {
        _logger.LogInformation("Restoring inventory for payment {PaymentId}", payment.Id);
        // Implement your inventory restoration logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task CleanupExpiredPayment(AbacatePay.Models.Payment.PaymentResponse payment)
    {
        _logger.LogInformation("Cleaning up expired payment {PaymentId}", payment.Id);
        // Implement your cleanup logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task UpdateRefundStatus(string refundId, string status)
    {
        _logger.LogInformation("Updating refund status for refund {RefundId} to {Status}", refundId, status);
        // Implement your database update logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task SendRefundConfirmation(AbacatePay.Models.Refund.RefundResponse refund)
    {
        _logger.LogInformation("Sending refund confirmation for refund {RefundId}", refund.Id);
        // Implement your email/notification logic here
        await Task.Delay(100); // Simulate async operation
    }

    private async Task NotifyRefundFailure(AbacatePay.Models.Refund.RefundResponse refund)
    {
        _logger.LogInformation("Notifying refund failure for refund {RefundId}", refund.Id);
        // Implement your notification logic here
        await Task.Delay(100); // Simulate async operation
    }
}
