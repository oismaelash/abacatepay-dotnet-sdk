using Newtonsoft.Json;

namespace AbacatePay.Models.PixQrCode;

/// <summary>
/// PIX QRCode response from API
/// </summary>
public class PixQrCodeResponse
{
    /// <summary>
    /// PIX QRCode ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount in cents
    /// </summary>
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Payment status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Whether this QRCode was created in dev mode
    /// </summary>
    [JsonProperty("devMode")]
    public bool DevMode { get; set; }

    /// <summary>
    /// PIX BR Code
    /// </summary>
    [JsonProperty("brCode")]
    public string? BrCode { get; set; }

    /// <summary>
    /// PIX BR Code as base64 image
    /// </summary>
    [JsonProperty("brCodeBase64")]
    public string? BrCodeBase64 { get; set; }

    /// <summary>
    /// Platform fee in cents
    /// </summary>
    [JsonProperty("platformFee")]
    public int? PlatformFee { get; set; }

    /// <summary>
    /// QRCode creation timestamp
    /// </summary>
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// QRCode last update timestamp
    /// </summary>
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// QRCode expiration timestamp
    /// </summary>
    [JsonProperty("expiresAt")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Additional metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// PIX QRCode status check response
/// </summary>
public class PixQrCodeStatusResponse
{
    /// <summary>
    /// Payment status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// QRCode expiration timestamp
    /// </summary>
    [JsonProperty("expiresAt")]
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// PIX QRCode simulate payment request
/// </summary>
public class PixQrCodeSimulateRequest
{
    /// <summary>
    /// Additional metadata for simulation
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}
