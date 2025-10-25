using AbacatePay.Models.Common;
using Newtonsoft.Json;

namespace AbacatePay.Models.Customer;

/// <summary>
/// Customer response from API
/// </summary>
public class CustomerResponseData
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
    public CustomerMetadata? Metadata { get; set; }
}

/// <summary>
/// Customer response from API
/// </summary>
public class CustomerResponse : ApiResponse<CustomerResponseData>
{
}

/// <summary>
/// Customer metadata
/// </summary>
public class CustomerMetadata
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
