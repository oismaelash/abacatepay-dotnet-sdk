using System.ComponentModel.DataAnnotations;
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
    public CouponData Data { get; set; } = new();
}

/// <summary>
/// Coupon data
/// </summary>
public class CouponData
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
    [Required]
    [JsonProperty("maxRedeems")]
    public int MaxRedeems { get; set; }

    /// <summary>
    /// Type of discount
    /// </summary>
    [Required]
    [JsonProperty("discountKind")]
    public DiscountKind DiscountKind { get; set; }

    /// <summary>
    /// Discount value to be applied
    /// </summary>
    [Required]
    [JsonProperty("discount")]
    public decimal Discount { get; set; }

    /// <summary>
    /// Additional coupon metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Discount kind enumeration
/// </summary>
public enum DiscountKind
{
    /// <summary>
    /// Percentage discount
    /// </summary>
    PERCENTAGE,

    /// <summary>
    /// Fixed amount discount
    /// </summary>
    FIXED
}
