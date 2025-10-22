using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// Request to create a withdraw
/// </summary>
public class WithdrawRequest
{
    /// <summary>
    /// Withdraw amount in cents
    /// </summary>
    [Required]
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// PIX key of the recipient
    /// </summary>
    [Required]
    [JsonProperty("pixKey")]
    public string PixKey { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw notes or description
    /// </summary>
    [JsonProperty("notes")]
    public string? Notes { get; set; }
}
