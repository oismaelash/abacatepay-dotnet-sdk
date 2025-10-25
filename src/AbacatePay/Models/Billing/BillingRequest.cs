using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using AbacatePay.Enums;
using AbacatePay.Attributes;

namespace AbacatePay.Models.Billing;

/// <summary>
/// Request to create a billing
/// </summary>
public class BillingRequest
{
    /// <summary>
    /// Billing frequency
    /// </summary>
    [Required]
    [JsonProperty("frequency")]
    [FrequencyValidation]
    public string Frequency { get; set; } = string.Empty;

    /// <summary>
    /// Accepted payment methods
    /// </summary>
    [Required]
    [JsonProperty("methods")]
    [EnumDataType(typeof(BillingPaymentMethod))]
    public List<string> Methods { get; set; } = new();

    /// <summary>
    /// Products included in the billing
    /// </summary>
    [Required]
    [JsonProperty("products")]
    public List<BillingProduct> Products { get; set; } = new();

    /// <summary>
    /// Return URL for redirecting back
    /// </summary>
    [Required]
    [JsonProperty("returnUrl")]
    public string ReturnUrl { get; set; } = string.Empty;

    /// <summary>
    /// Completion URL after payment
    /// </summary>
    [Required]
    [JsonProperty("completionUrl")]
    public string CompletionUrl { get; set; } = string.Empty;

    /// <summary>
    /// Customer ID (optional if customer is provided)
    /// </summary>
    [JsonProperty("customerId")]
    public string? CustomerId { get; set; }

    /// <summary>
    /// Customer data for immediate creation (optional if customerId is provided)
    /// </summary>
    [JsonProperty("customer")]
    public BillingCustomerData? Customer { get; set; }

    /// <summary>
    /// Whether to allow coupons for this billing
    /// </summary>
    [JsonProperty("allowoCoupons")]
    public bool AllowCoupons { get; set; }

    /// <summary>
    /// Coupons to be used for this billing
    /// </summary>
    [JsonProperty("coupons")]
    public List<string>? Coupons { get; set; }
    

    /// <summary>
    /// External ID for the billing
    /// </summary>
    [JsonProperty("externalId")]
    public string? ExternalId { get; set; }
}


/// <summary>
/// Billing product
/// </summary>
public class BillingProduct
{
    /// <summary>
    /// Unique product identifier in your system
    /// </summary>
    [Required]
    [JsonProperty("externalId")]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Product name
    /// </summary>
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product description
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Product quantity (minimum 1)
    /// </summary>
    [Required]
    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price in cents (minimum 100)
    /// </summary>
    [Required]
    [JsonProperty("price")]
    public int Price { get; set; }
}

/// <summary>
/// Billing customer data
/// </summary>
public class BillingCustomerData
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
