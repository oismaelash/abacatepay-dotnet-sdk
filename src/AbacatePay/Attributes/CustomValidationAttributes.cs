using System.ComponentModel.DataAnnotations;

namespace AbacatePay.Attributes;

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
