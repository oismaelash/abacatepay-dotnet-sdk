using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using AbacatePay.Attributes;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// Request to create a withdraw
/// </summary>
public class WithdrawRequest
{
    /// <summary>
    /// Withdraw description
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// External ID for the withdraw
    /// </summary>
    [Required]
    [JsonProperty("externalId")]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw method (PIX)
    /// </summary>
    [Required]
    [JsonProperty("method")]
    [MethodValidation]
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Withdraw amount in cents
    /// </summary>
    [Required]
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// PIX information
    /// </summary>
    [Required]
    [JsonProperty("pix")]
    public Pix Pix { get; set; } = new Pix();
}
