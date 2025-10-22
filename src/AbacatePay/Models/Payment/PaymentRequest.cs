using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.Payment;

/// <summary>
/// Request to create a payment
/// </summary>
public class PaymentRequest
{
    /// <summary>
    /// Payment amount in cents (e.g., 1000 = R$ 10.00)
    /// </summary>
    [Required]
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Payment currency (default: BRL)
    /// </summary>
    [JsonProperty("currency")]
    public string Currency { get; set; } = "BRL";

    /// <summary>
    /// Payment description
    /// </summary>
    [Required]
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Payment method
    /// </summary>
    [Required]
    [JsonProperty("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Customer information
    /// </summary>
    [JsonProperty("customer")]
    public CustomerInfo? Customer { get; set; }

    /// <summary>
    /// Payment metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// Payment expiration time in seconds (default: 3600 = 1 hour)
    /// </summary>
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; } = 3600;

    /// <summary>
    /// Webhook URL for payment status updates
    /// </summary>
    [JsonProperty("webhook_url")]
    public string? WebhookUrl { get; set; }

    /// <summary>
    /// Redirect URL after payment completion
    /// </summary>
    [JsonProperty("redirect_url")]
    public string? RedirectUrl { get; set; }

    /// <summary>
    /// Payment method specific options
    /// </summary>
    [JsonProperty("payment_options")]
    public PaymentOptions? PaymentOptions { get; set; }
}

/// <summary>
/// Available payment methods
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// PIX instant payment
    /// </summary>
    PIX,

    /// <summary>
    /// Boleto bancário
    /// </summary>
    BOLETO,

}

/// <summary>
/// Customer information
/// </summary>
public class CustomerInfo
{
    /// <summary>
    /// Customer name
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Customer email
    /// </summary>
    [JsonProperty("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Customer phone
    /// </summary>
    [JsonProperty("phone")]
    public string? Phone { get; set; }

    /// <summary>
    /// Customer document (CPF/CNPJ)
    /// </summary>
    [JsonProperty("document")]
    public string? Document { get; set; }

    /// <summary>
    /// Customer address
    /// </summary>
    [JsonProperty("address")]
    public AddressInfo? Address { get; set; }
}

/// <summary>
/// Address information
/// </summary>
public class AddressInfo
{
    /// <summary>
    /// Street address
    /// </summary>
    [JsonProperty("street")]
    public string? Street { get; set; }

    /// <summary>
    /// Address number
    /// </summary>
    [JsonProperty("number")]
    public string? Number { get; set; }

    /// <summary>
    /// Address complement
    /// </summary>
    [JsonProperty("complement")]
    public string? Complement { get; set; }

    /// <summary>
    /// Neighborhood
    /// </summary>
    [JsonProperty("neighborhood")]
    public string? Neighborhood { get; set; }

    /// <summary>
    /// City
    /// </summary>
    [JsonProperty("city")]
    public string? City { get; set; }

    /// <summary>
    /// State
    /// </summary>
    [JsonProperty("state")]
    public string? State { get; set; }

    /// <summary>
    /// ZIP code
    /// </summary>
    [JsonProperty("zip_code")]
    public string? ZipCode { get; set; }
}

/// <summary>
/// Payment method specific options
/// </summary>
public class PaymentOptions
{
    /// <summary>
    /// PIX specific options
    /// </summary>
    [JsonProperty("pix")]
    public PixOptions? Pix { get; set; }

    /// <summary>
    /// Boleto specific options
    /// </summary>
    [JsonProperty("boleto")]
    public BoletoOptions? Boleto { get; set; }

}

/// <summary>
/// PIX payment options
/// </summary>
public class PixOptions
{
    /// <summary>
    /// PIX key (CPF, email, phone, or random key)
    /// </summary>
    [JsonProperty("pix_key")]
    public string? PixKey { get; set; }

    /// <summary>
    /// PIX key type
    /// </summary>
    [JsonProperty("pix_key_type")]
    public PixKeyType? PixKeyType { get; set; }
}

/// <summary>
/// PIX key types
/// </summary>
public enum PixKeyType
{
    /// <summary>
    /// CPF key
    /// </summary>
    CPF,

    /// <summary>
    /// Email key
    /// </summary>
    EMAIL,

    /// <summary>
    /// Phone key
    /// </summary>
    PHONE,

    /// <summary>
    /// Random key
    /// </summary>
    RANDOM
}

/// <summary>
/// Boleto payment options
/// </summary>
public class BoletoOptions
{
    /// <summary>
    /// Boleto expiration date
    /// </summary>
    [JsonProperty("expires_at")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Boleto instructions
    /// </summary>
    [JsonProperty("instructions")]
    public string? Instructions { get; set; }
}

