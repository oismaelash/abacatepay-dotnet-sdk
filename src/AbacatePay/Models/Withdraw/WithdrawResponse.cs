using Newtonsoft.Json;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// Withdraw response from API
/// </summary>
public class WithdrawResponse
{
    /// <summary>
    /// Withdraw ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw amount in cents
    /// </summary>
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Withdraw status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// PIX key of the recipient
    /// </summary>
    [JsonProperty("pixKey")]
    public string PixKey { get; set; } = string.Empty;

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
