using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// Custom validation attribute for method type
/// </summary>
public class MethodValidationAttribute : ValidationAttribute
{
    private static readonly string[] ValidTypes = { "PIX" };

    public override bool IsValid(object? value)
    {
        if (value is not string stringValue)
            return false;

        return ValidTypes.Contains(stringValue.ToUpper());
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field must be one of: {string.Join(", ", ValidTypes)}";
    }
}

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
