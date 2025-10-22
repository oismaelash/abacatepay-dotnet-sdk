using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.Customer;

/// <summary>
/// Request to create a customer
/// </summary>
public class CustomerRequest
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
