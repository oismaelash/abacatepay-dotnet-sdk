using Newtonsoft.Json;

namespace AbacatePay.Models.Withdraw;

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
    [JsonProperty("type")]
    public PixKeyType Type { get; set; }

    /// <summary>
    /// PIX key value
    /// </summary>
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;
}
