using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using AbacatePay.Attributes;
using AbacatePay.Enums;

namespace AbacatePay.Models.Withdraw;


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
