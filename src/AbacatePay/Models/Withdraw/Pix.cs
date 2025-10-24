using Newtonsoft.Json;

namespace AbacatePay.Models.Withdraw;

/// <summary>
/// PIX information for withdraw
/// </summary>
public class Pix
{
    /// <summary>
    /// PIX key type (CPF, CNPJ, EMAIL, PHONE, RANDOM)
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// PIX key value
    /// </summary>
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;
}
