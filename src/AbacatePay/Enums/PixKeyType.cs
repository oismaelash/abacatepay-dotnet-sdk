namespace AbacatePay.Enums;

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
