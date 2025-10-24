using System.ComponentModel.DataAnnotations;
using AbacatePay.Attributes;
using Newtonsoft.Json;

namespace AbacatePay.Models.Coupon;

/// <summary>
/// Request to create a coupon
/// </summary>
public class CouponRequest
{
    /// <summary>
    /// Coupon data
    /// </summary>
    [Required]
    [JsonProperty("data")]
    public Data Data { get; set; } = new();
}

/// <summary>
/// Coupon data
/// </summary>
public class Data
{
    /// <summary>
    /// Unique coupon identifier
    /// </summary>
    [Required]
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Coupon description or notes
    /// </summary>
    [JsonProperty("notes")]
    public string? Notes { get; set; }

    /// <summary>
    /// Maximum number of times the coupon can be redeemed
    /// </summary>
    [JsonProperty("maxRedeems")]
    [Range(-1, 100)]
    public int MaxRedeems { get; set; }

    /// <summary>
    /// Type of discount
    /// </summary>
    [Required]
    [DiscountKindValidation]
    [JsonProperty("discountKind")]
    public string DiscountKind { get; set; } = string.Empty;

    /// <summary>
    /// Discount value to be applied
    /// </summary>
    [Required]
    [JsonProperty("discount")]
    public int Discount { get; set; }

    /// <summary>
    /// Additional coupon metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

