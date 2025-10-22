using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.Refund;

/// <summary>
/// Request to create a refund
/// </summary>
public class RefundRequest
{
    /// <summary>
    /// Payment ID to refund
    /// </summary>
    [Required]
    [JsonProperty("payment_id")]
    public string PaymentId { get; set; } = string.Empty;

    /// <summary>
    /// Refund amount in cents (if not provided, full amount will be refunded)
    /// </summary>
    [JsonProperty("amount")]
    public int? Amount { get; set; }

    /// <summary>
    /// Refund reason
    /// </summary>
    [JsonProperty("reason")]
    public string? Reason { get; set; }

    /// <summary>
    /// Refund metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Whether to notify the customer about the refund
    /// </summary>
    [JsonProperty("notify_customer")]
    public bool? NotifyCustomer { get; set; }
}
