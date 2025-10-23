using Newtonsoft.Json;

namespace AbacatePay.Models.Payment;

/// <summary>
/// Payment response from API
/// </summary>
public class PaymentResponse
{
    /// <summary>
    /// Payment ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Payment status
    /// </summary>
    [JsonProperty("status")]
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Payment amount in cents
    /// </summary>
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Payment currency
    /// </summary>
    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Payment description
    /// </summary>
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Payment method used
    /// </summary>
    [JsonProperty("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Payment creation timestamp
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Payment expiration timestamp
    /// </summary>
    [JsonProperty("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Payment completion timestamp
    /// </summary>
    [JsonProperty("paid_at")]
    public DateTime? PaidAt { get; set; }

    /// <summary>
    /// Payment metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Payment method specific data
    /// </summary>
    [JsonProperty("payment_data")]
    public PaymentData? PaymentData { get; set; }

    /// <summary>
    /// Customer information
    /// </summary>
    [JsonProperty("customer")]
    public CustomerInfo? Customer { get; set; }
}

/// <summary>
/// Payment status enumeration
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment is pending
    /// </summary>
    PENDING,

    /// <summary>
    /// Payment is processing
    /// </summary>
    PROCESSING,

    /// <summary>
    /// Payment is completed
    /// </summary>
    COMPLETED,

    /// <summary>
    /// Payment failed
    /// </summary>
    FAILED,

    /// <summary>
    /// Payment was cancelled
    /// </summary>
    CANCELLED,

    /// <summary>
    /// Payment expired
    /// </summary>
    EXPIRED,

    /// <summary>
    /// Payment was refunded
    /// </summary>
    REFUNDED
}

/// <summary>
/// Payment method specific data
/// </summary>
public class PaymentData
{
    /// <summary>
    /// PIX specific data
    /// </summary>
    [JsonProperty("pix")]
    public PixData? Pix { get; set; }


}

/// <summary>
/// PIX payment data
/// </summary>
public class PixData
{
    /// <summary>
    /// PIX QR code
    /// </summary>
    [JsonProperty("qr_code")]
    public string? QrCode { get; set; }

    /// <summary>
    /// PIX QR code as image (base64)
    /// </summary>
    [JsonProperty("qr_code_image")]
    public string? QrCodeImage { get; set; }

    /// <summary>
    /// PIX copy and paste code
    /// </summary>
    [JsonProperty("copy_paste_code")]
    public string? CopyPasteCode { get; set; }

    /// <summary>
    /// PIX key used
    /// </summary>
    [JsonProperty("pix_key")]
    public string? PixKey { get; set; }

    /// <summary>
    /// PIX key type
    /// </summary>
    [JsonProperty("pix_key_type")]
    public PixKeyType? PixKeyType { get; set; }
}


