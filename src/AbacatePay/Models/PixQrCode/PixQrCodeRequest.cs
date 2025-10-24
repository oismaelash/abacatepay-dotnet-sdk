using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.PixQrCode;

/// <summary>
/// Request to create a PIX QRCode
/// </summary>
public class PixQrCodeRequest
{
    /// <summary>
    /// Payment amount in cents
    /// </summary>
    [Required]
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Payment expiration time in seconds
    /// </summary>
    [JsonProperty("expiresIn")]
    public int? ExpiresIn { get; set; }

    /// <summary>
    /// Payment description (maximum 140 characters)
    /// </summary>
    [MaxLength(140)]
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Customer data for immediate creation (optional)
    /// </summary>
    [JsonProperty("customer")]
    public PixQrCodeCustomer? Customer { get; set; }

    /// <summary>
    /// Additional metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}

/// <summary>
/// PIX QRCode customer data
/// </summary>
public class PixQrCodeCustomer
{
    /// <summary>
    /// Customer full name
    /// </summary>
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Customer cellphone
    /// </summary>
    [Required]
    [JsonProperty("cellphone")]
    public string Cellphone { get; set; } = string.Empty;

    /// <summary>
    /// Customer email address
    /// </summary>
    [Required]
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Customer tax ID (CPF or CNPJ)
    /// </summary>
    [Required]
    [JsonProperty("taxId")]
    public string TaxId { get; set; } = string.Empty;
}
