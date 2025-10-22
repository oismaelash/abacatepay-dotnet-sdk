using Newtonsoft.Json;
using AbacatePay.Models.Payment;
using AbacatePay.Models.Refund;

namespace AbacatePay.Models.Webhook;

/// <summary>
/// Webhook event from AbacatePay
/// </summary>
public class WebhookEvent
{
    /// <summary>
    /// Event ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Event type
    /// </summary>
    [JsonProperty("type")]
    public WebhookEventType Type { get; set; }

    /// <summary>
    /// Event data
    /// </summary>
    [JsonProperty("data")]
    public object? Data { get; set; }

    /// <summary>
    /// Event creation timestamp
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Webhook signature for verification
    /// </summary>
    [JsonProperty("signature")]
    public string? Signature { get; set; }
}

/// <summary>
/// Webhook event types
/// </summary>
public enum WebhookEventType
{
    /// <summary>
    /// Payment created
    /// </summary>
    PAYMENT_CREATED,

    /// <summary>
    /// Payment updated
    /// </summary>
    PAYMENT_UPDATED,

    /// <summary>
    /// Payment completed
    /// </summary>
    PAYMENT_COMPLETED,

    /// <summary>
    /// Payment failed
    /// </summary>
    PAYMENT_FAILED,

    /// <summary>
    /// Payment cancelled
    /// </summary>
    PAYMENT_CANCELLED,

    /// <summary>
    /// Payment expired
    /// </summary>
    PAYMENT_EXPIRED,

    /// <summary>
    /// Refund created
    /// </summary>
    REFUND_CREATED,

    /// <summary>
    /// Refund completed
    /// </summary>
    REFUND_COMPLETED,

    /// <summary>
    /// Refund failed
    /// </summary>
    REFUND_FAILED
}

/// <summary>
/// Payment webhook data
/// </summary>
public class PaymentWebhookData
{
    /// <summary>
    /// Payment information
    /// </summary>
    [JsonProperty("payment")]
    public PaymentResponse? Payment { get; set; }

    /// <summary>
    /// Previous payment state (for updates)
    /// </summary>
    [JsonProperty("previous_state")]
    public PaymentStatus? PreviousState { get; set; }
}

/// <summary>
/// Refund webhook data
/// </summary>
public class RefundWebhookData
{
    /// <summary>
    /// Refund information
    /// </summary>
    [JsonProperty("refund")]
    public RefundResponse? Refund { get; set; }

    /// <summary>
    /// Associated payment information
    /// </summary>
    [JsonProperty("payment")]
    public PaymentResponse? Payment { get; set; }
}
