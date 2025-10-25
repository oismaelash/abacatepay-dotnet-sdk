using AbacatePay.Models.Coupon;

namespace AbacatePay.Services;

/// <summary>
/// Service interface for coupon operations
/// </summary>
public interface ICouponService
{
    /// <summary>
    /// Create a new coupon
    /// </summary>
    /// <param name="request">Coupon request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Coupon response</returns>
    Task<CouponData> CreateCouponAsync(CouponRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// List all coupons
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of coupons</returns>
    Task<List<CouponData>> ListCouponsAsync(CancellationToken cancellationToken = default);
}
