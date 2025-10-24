using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// Custom validation attribute for PIX key type
/// </summary>
public class PixKeyTypeValidationAttribute : ValidationAttribute
{
    private static readonly string[] ValidTypes = { "CPF", "CNPJ", "EMAIL", "PHONE", "RANDOM", "BR_CODE" };

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
/// PIX key types
/// </summary>
public enum PixKeyType
{
    /// <summary>
    /// CPF (Brazilian individual tax ID)
    /// </summary>
    CPF,
    
    /// <summary>
    /// CNPJ (Brazilian company tax ID)
    /// </summary>
    CNPJ,
    
    /// <summary>
    /// Email address
    /// </summary>
    EMAIL,
    
    /// <summary>
    /// Phone number
    /// </summary>
    PHONE,
    
    /// <summary>
    /// Random key
    /// </summary>
    RANDOM,
    
    /// <summary>
    /// BR Code (Brazilian bank code)
    /// </summary>
    BR_CODE
}

/// <summary>
/// PIX information for withdraw
/// </summary>
public class Pix
{
    /// <summary>
    /// PIX key type
    /// </summary>
    [Required]
    [PixKeyTypeValidation]
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// PIX key value
    /// </summary>
    [Required]
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;
}
