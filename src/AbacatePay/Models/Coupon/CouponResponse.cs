using AbacatePay.Enums;
using AbacatePay.Models.Common;
using Newtonsoft.Json;

namespace AbacatePay.Models.Coupon;

/// <summary>
/// Coupon data from API
/// </summary>
public class CouponData
{
    /// <summary>
    /// Coupon ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Coupon description or notes
    /// </summary>
    [JsonProperty("notes")]
    public string? Notes { get; set; }

    /// <summary>
    /// Maximum number of times the coupon can be redeemed
    /// </summary>
    [JsonProperty("maxRedeems")]
    public int MaxRedeems { get; set; }

    /// <summary>
    /// Number of times the coupon has been redeemed
    /// </summary>
    [JsonProperty("redeemsCount")]
    public int RedeemsCount { get; set; }

    /// <summary>
    /// Type of discount
    /// </summary>
    [JsonProperty("discountKind")]
    public DiscountKind DiscountKind { get; set; }

    /// <summary>
    /// Discount value
    /// </summary>
    [JsonProperty("discount")]
    public decimal Discount { get; set; }

    /// <summary>
    /// Whether this coupon was created in dev mode
    /// </summary>
    [JsonProperty("devMode")]
    public bool DevMode { get; set; }

    /// <summary>
    /// Coupon status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Coupon creation timestamp
    /// </summary>
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Coupon last update timestamp
    /// </summary>
    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Additional coupon metadata
    /// </summary>
    [JsonProperty("metadata")]
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Coupon response from API
/// </summary>
public class CouponResponse : ApiResponse<CouponData>
{
}