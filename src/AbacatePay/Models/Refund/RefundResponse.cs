using Newtonsoft.Json;

namespace AbacatePay.Models.Refund;

/// <summary>
/// Refund response from API
/// </summary>
public class RefundResponse
{
    /// <summary>
    /// Refund ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Payment ID that was refunded
    /// </summary>
    [JsonProperty("payment_id")]
    public string PaymentId { get; set; } = string.Empty;

    /// <summary>
    /// Refund status
    /// </summary>
    [JsonProperty("status")]
    public RefundStatus Status { get; set; }

    /// <summary>
    /// Refund amount in cents
    /// </summary>
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Refund currency
    /// </summary>
    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Refund reason
    /// </summary>
    [JsonProperty("reason")]
    public string? Reason { get; set; }

    /// <summary>
    /// Refund creation timestamp
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Refund completion timestamp
    /// </summary>
    [JsonProperty("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Refund metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Refund processing details
    /// </summary>
    [JsonProperty("processing_details")]
    public RefundProcessingDetails? ProcessingDetails { get; set; }
}

/// <summary>
/// Refund status enumeration
/// </summary>
public enum RefundStatus
{
    /// <summary>
    /// Refund is pending
    /// </summary>
    PENDING,

    /// <summary>
    /// Refund is processing
    /// </summary>
    PROCESSING,

    /// <summary>
    /// Refund is completed
    /// </summary>
    COMPLETED,

    /// <summary>
    /// Refund failed
    /// </summary>
    FAILED,

    /// <summary>
    /// Refund was cancelled
    /// </summary>
    CANCELLED
}

/// <summary>
/// Refund processing details
/// </summary>
public class RefundProcessingDetails
{
    /// <summary>
    /// Processing fee in cents
    /// </summary>
    [JsonProperty("processing_fee")]
    public int? ProcessingFee { get; set; }

    /// <summary>
    /// Net refund amount in cents
    /// </summary>
    [JsonProperty("net_amount")]
    public int? NetAmount { get; set; }

    /// <summary>
    /// Authorization code for the refund
    /// </summary>
    [JsonProperty("authorization_code")]
    public string? AuthorizationCode { get; set; }

    /// <summary>
    /// Transaction ID for the refund
    /// </summary>
    [JsonProperty("transaction_id")]
    public string? TransactionId { get; set; }

    /// <summary>
    /// Refund method used
    /// </summary>
    [JsonProperty("refund_method")]
    public string? RefundMethod { get; set; }

    /// <summary>
    /// Estimated processing time in hours
    /// </summary>
    [JsonProperty("estimated_processing_time")]
    public int? EstimatedProcessingTime { get; set; }
}
