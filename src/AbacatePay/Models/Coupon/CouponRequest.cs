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
    /// Unique coupon identifier
    /// </summary>
    [Required]
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Coupon description or notes
    /// </summary>
    [Required]
    [JsonProperty("notes")]
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of times the coupon can be redeemed
    /// </summary>
    [JsonProperty("maxRedeems")]
    [MaxRedeemsValidation]
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
    /// Additional coupon metadata (string or object values)
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}

