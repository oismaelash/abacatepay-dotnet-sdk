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

public class DiscountKindValidationAttribute : ValidationAttribute
{
    private static readonly string[] ValidTypes = { "PERCENTAGE", "FIXED" };

    public override bool IsValid(object? value)
    {
        if (value is not string stringValue)
            return false;

        return ValidTypes.Contains(stringValue.ToString().ToUpper());
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field must be one of: {string.Join(", ", ValidTypes)}";
    }
}

public class MaxRedeemsValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not int intValue)
            return false;

        // 0 is not allowed, must be -1 (unlimited), or any value from 1 to 100 inclusively
        return intValue == -1 || (intValue >= 1 && intValue <= 100);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The {name} field must be between -1 (unlimited) and from 1 to 100 inclusively";
    }
}

public class FrequencyValidationAttribute : ValidationAttribute
{
    private static readonly string[] ValidTypes = { "ONE_TIME", "MULTIPLE_PAYMENTS" };

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