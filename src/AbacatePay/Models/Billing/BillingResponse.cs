using AbacatePay.Models.Common;
using Newtonsoft.Json;

namespace AbacatePay.Models.Billing;

/// <summary>
/// Billing response from API
/// </summary>
public class BillingData
{
    /// <summary>
    /// Billing ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Billing URL
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Total amount in cents
    /// </summary>
    [JsonProperty("amount")]
    public int Amount { get; set; }

    /// <summary>
    /// Billing status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Whether this billing was created in dev mode
    /// </summary>
    [JsonProperty("devMode")]
    public bool DevMode { get; set; }

    /// <summary>
    /// Accepted payment methods
    /// </summary>
    [JsonProperty("methods")]
    public List<string> Methods { get; set; } = new();

    /// <summary>
    /// Products included in the billing
    /// </summary>
    [JsonProperty("products")]
    public List<BillingProductData> Products { get; set; } = new();

    /// <summary>
    /// Billing frequency
    /// </summary>
    [JsonProperty("frequency")]
    public string Frequency { get; set; } = string.Empty;

    /// <summary>
    /// Customer information
    /// </summary>
    [JsonProperty("customer")]
    public BillingCustomer? Customer { get; set; }

    /// <summary>
    /// Billing creation timestamp
    /// </summary>
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Billing last update timestamp
    /// </summary>
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Billing metadata including fee and URLs
    /// </summary>
    [JsonProperty("metadata")]
    public BillingMetadata? Metadata { get; set; }

    /// <summary>
    /// Whether coupons are allowed for this billing
    /// </summary>
    [JsonProperty("allowCoupons")]
    public bool AllowCoupons { get; set; }

    /// <summary>
    /// Available coupons for this billing
    /// </summary>
    [JsonProperty("coupons")]
    public List<string> Coupons { get; set; } = new();

    /// <summary>
    /// Coupons that have been used for this billing
    /// </summary>
    [JsonProperty("couponsUsed")]
    public List<string> CouponsUsed { get; set; } = new();

    /// <summary>
    /// External ID for the billing
    /// </summary>
    [JsonProperty("externalId")]
    public string? ExternalId { get; set; }
}

/// <summary>
/// Billing response from API
/// </summary>
public class BillingResponse : ApiResponse<BillingData>
{
}

/// <summary>
/// Billing product response
/// </summary>
public class BillingProductData
{
    /// <summary>
    /// Product ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// External product ID
    /// </summary>
    [JsonProperty("externalId")]
    public string ExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Product quantity
    /// </summary>
    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    /// <summary>
    /// Product quantity
    /// </summary>
    [JsonProperty("price")]
    public int Price { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Billing product response from API
/// </summary>
public class BillingProductResponse : ApiResponse<BillingProductData>
{
}

/// <summary>
/// Billing customer data
/// </summary>
public class BillingCustomerResponseData
{
    /// <summary>
    /// Customer ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Customer metadata
    /// </summary>
    [JsonProperty("metadata")]
    public BillingCustomerMetadata? Metadata { get; set; }
}

/// <summary>
/// Billing customer response from API
/// </summary>
public class BillingCustomerResponse : ApiResponse<BillingCustomerResponseData>
{
}

/// <summary>
/// Billing customer metadata
/// </summary>
public class BillingCustomerMetadata
{
    /// <summary>
    /// Customer full name
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Customer cellphone
    /// </summary>
    [JsonProperty("cellphone")]
    public string? Cellphone { get; set; }

    /// <summary>
    /// Customer email address
    /// </summary>
    [JsonProperty("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Customer tax ID (CPF or CNPJ)
    /// </summary>
    [JsonProperty("taxId")]
    public string? TaxId { get; set; }

    /// <summary>
    /// Customer zip code
    /// </summary>
    [JsonProperty("zipCode")]
    public string? ZipCode { get; set; }
}

/// <summary>
/// Billing metadata
/// </summary>
public class BillingMetadata
{
    /// <summary>
    /// Platform fee in cents
    /// </summary>
    [JsonProperty("fee")]
    public int Fee { get; set; }

    /// <summary>
    /// Return URL after payment
    /// </summary>
    [JsonProperty("returnUrl")]
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Completion URL after payment
    /// </summary>
    [JsonProperty("completionUrl")]
    public string? CompletionUrl { get; set; }
}

/// <summary>
/// Billing customer
/// </summary>
public class BillingCustomer
{
    /// <summary>
    /// Customer ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Customer metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}
