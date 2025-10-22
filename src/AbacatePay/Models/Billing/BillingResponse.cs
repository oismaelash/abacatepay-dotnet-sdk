using Newtonsoft.Json;

namespace AbacatePay.Models.Billing;

/// <summary>
/// Billing response from API
/// </summary>
public class BillingResponse
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
    public List<BillingProductResponse> Products { get; set; } = new();

    /// <summary>
    /// Billing frequency
    /// </summary>
    [JsonProperty("frequency")]
    public string Frequency { get; set; } = string.Empty;

    /// <summary>
    /// Next billing information
    /// </summary>
    [JsonProperty("nextBilling")]
    public object? NextBilling { get; set; }

    /// <summary>
    /// Customer information
    /// </summary>
    [JsonProperty("customer")]
    public BillingCustomerResponse? Customer { get; set; }

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
}

/// <summary>
/// Billing product response
/// </summary>
public class BillingProductResponse
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
}

/// <summary>
/// Billing customer response
/// </summary>
public class BillingCustomerResponse
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
}
