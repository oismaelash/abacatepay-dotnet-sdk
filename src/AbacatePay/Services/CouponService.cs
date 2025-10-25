using AbacatePay.Models.Coupon;
using AbacatePay.Validators;

namespace AbacatePay.Services;

/// <summary>
/// Service implementation for coupon operations
/// </summary>
public class CouponService : BaseService, ICouponService
{
    public CouponService(IHttpService httpService) : base(httpService)
    {
    }

    /// <summary>
    /// Create a new coupon
    /// </summary>
    /// <param name="request">Coupon request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Coupon response</returns>
    public async Task<CouponData> CreateCouponAsync(CouponRequest request, CancellationToken cancellationToken = default)
    {
        RequestValidator.ValidateCouponRequest(request);
        
        return await ExecuteRequestAsync(
            () => HttpService.PostAsync<CouponData>("/v1/coupon/create", request, cancellationToken),
            "Coupon creation"
        );
    }

    /// <summary>
    /// List all coupons
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of coupons</returns>
    public async Task<List<CouponData>> ListCouponsAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteListRequestAsync(
            () => HttpService.GetAsync<List<CouponData>>("/v1/coupon/list", cancellationToken),
            "Coupon list"
        );
    }
}
