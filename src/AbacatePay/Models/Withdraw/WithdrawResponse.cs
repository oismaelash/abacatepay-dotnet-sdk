using AbacatePay.Models.Common;
using Newtonsoft.Json;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// Withdraw response from API
/// </summary>
public class WithdrawData
{
    /// <summary>
    /// Withdraw ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is in development mode
    /// </summary>
    [JsonProperty("devMode")]
    public bool DevMode { get; set; }

    /// <summary>
    /// Receipt URL for the withdraw
    /// </summary>
    [JsonProperty("receiptUrl")]
    public string ReceiptUrl { get; set; } = string.Empty;

    /// <summary>
    /// Transaction kind (WITHDRAW)
    /// </summary>
    [JsonProperty("kind")]
    public string Kind { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw amount in cents
    /// </summary>
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Platform fee in cents
    /// </summary>
    [JsonProperty("platformFee")]
    public int PlatformFee { get; set; }

    /// <summary>
    /// External ID for the withdraw
    /// </summary>
    [JsonProperty("externalId")]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw creation timestamp
    /// </summary>
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Withdraw last update timestamp
    /// </summary>
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}

public class WithdrawResponse : ApiResponse<WithdrawData>
{
}