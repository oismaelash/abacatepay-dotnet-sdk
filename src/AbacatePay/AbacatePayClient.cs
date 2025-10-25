using AbacatePay.Models;
using AbacatePay.Models.Common;
using AbacatePay.Models.Customer;
using AbacatePay.Models.Coupon;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;
using AbacatePay.Models.Store;
using AbacatePay.Models.Withdraw;
using AbacatePay.Services;
using AbacatePay.Validators;

namespace AbacatePay;

/// <summary>
/// Main AbacatePay client for interacting with the API
/// </summary>
public class AbacatePayClient : IDisposable
{
    private readonly IHttpService _httpService;
    private readonly HttpClient _httpClient;
    private bool _disposed;

    // Service properties for accessing domain-specific operations
    public ICustomerService Customers { get; }
    public IBillingService Billings { get; }
    public IPixQrCodeService PixQrCodes { get; }
    public ICouponService Coupons { get; }
    public IStoreService Store { get; }
    public IWithdrawService Withdraws { get; }

    /// <summary>
    /// Initialize AbacatePay client with configuration
    /// </summary>
    /// <param name="config">Configuration object</param>
    public AbacatePayClient(AbacatePayConfig config)
    {
        ValidateConfig(config);
        
        _httpClient = new HttpClient();
        _httpService = new HttpService(_httpClient, config);
        
        // Initialize domain services
        Customers = new CustomerService(_httpService);
        Billings = new BillingService(_httpService);
        PixQrCodes = new PixQrCodeService(_httpService);
        Coupons = new CouponService(_httpService);
        Store = new StoreService(_httpService);
        Withdraws = new WithdrawService(_httpService);
    }

    /// <summary>
    /// Initialize AbacatePay client with API credentials
    /// </summary>
    /// <param name="apiKey">Your Bearer token</param>
    /// <param name="sandbox">Whether to use sandbox mode</param>
    public AbacatePayClient(string apiKey, bool sandbox = false)
    {
        var config = new AbacatePayConfig
        {
            ApiKey = apiKey,
            Sandbox = sandbox,
            BaseUrl = "https://api.abacatepay.com"
        };

        ValidateConfig(config);
        
        _httpClient = new HttpClient();
        _httpService = new HttpService(_httpClient, config);
        
        // Initialize domain services
        Customers = new CustomerService(_httpService);
        Billings = new BillingService(_httpService);
        PixQrCodes = new PixQrCodeService(_httpService);
        Coupons = new CouponService(_httpService);
        Store = new StoreService(_httpService);
        Withdraws = new WithdrawService(_httpService);
    }

    private static void ValidateConfig(AbacatePayConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.ApiKey))
            throw new ArgumentException("API Key is required", nameof(config.ApiKey));
        
        if (string.IsNullOrWhiteSpace(config.BaseUrl))
            throw new ArgumentException("Base URL is required", nameof(config.BaseUrl));
        
        if (!Uri.TryCreate(config.BaseUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid Base URL format", nameof(config.BaseUrl));
    }




    #region Customer Methods (Convenience Methods)

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="request">Customer request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer response</returns>
    public async Task<CustomerResponseData> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default)
    {
        return await Customers.CreateCustomerAsync(request, cancellationToken);
    }

    /// <summary>
    /// List all customers
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers</returns>
    public async Task<List<CustomerResponse>> ListCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await Customers.ListCustomersAsync(cancellationToken);
    }

    #endregion

    #region Coupon Methods (Convenience Methods)

    /// <summary>
    /// Create a new coupon
    /// </summary>
    /// <param name="request">Coupon request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Coupon response</returns>
    public async Task<CouponData> CreateCouponAsync(CouponRequest request, CancellationToken cancellationToken = default)
    {
        return await Coupons.CreateCouponAsync(request, cancellationToken);
    }

    /// <summary>
    /// List all coupons
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of coupons</returns>
    public async Task<List<CouponData>> ListCouponsAsync(CancellationToken cancellationToken = default)
    {
        return await Coupons.ListCouponsAsync(cancellationToken);
    }

    #endregion

    #region Billing Methods (Convenience Methods)

    /// <summary>
    /// Create a new billing
    /// </summary>
    /// <param name="request">Billing request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    public async Task<BillingData> CreateBillingAsync(BillingRequest request, CancellationToken cancellationToken = default)
    {
        return await Billings.CreateBillingAsync(request, cancellationToken);
    }

    /// <summary>
    /// Get billing details by ID
    /// </summary>
    /// <param name="billingId">Billing ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    public async Task<BillingData> GetBillingAsync(string billingId, CancellationToken cancellationToken = default)
    {
        return await Billings.GetBillingAsync(billingId, cancellationToken);
    }

    /// <summary>
    /// List all billings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billings</returns>
    public async Task<List<BillingData>> ListBillingsAsync(CancellationToken cancellationToken = default)
    {
        return await Billings.ListBillingsAsync(cancellationToken);
    }

    #endregion

    #region PIX QRCode Methods (Convenience Methods)

    /// <summary>
    /// Create a new PIX QRCode
    /// </summary>
    /// <param name="request">PIX QRCode request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    public async Task<PixQrCodeData> CreatePixQrCodeAsync(PixQrCodeRequest request, CancellationToken cancellationToken = default)
    {
        return await PixQrCodes.CreatePixQrCodeAsync(request, cancellationToken);
    }

    /// <summary>
    /// Check PIX QRCode status
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode status response</returns>
    public async Task<PixQrCodeStatusData> CheckPixQrCodeStatusAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
    {
        return await PixQrCodes.CheckPixQrCodeStatusAsync(pixQrCodeId, cancellationToken);
    }

    /// <summary>
    /// Simulate PIX QRCode payment (Dev Mode only)
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    public async Task<PixQrCodeData> SimulatePixQrCodePaymentAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
    {
        return await PixQrCodes.SimulatePixQrCodePaymentAsync(pixQrCodeId, cancellationToken);
    }

    #endregion

    #region Store Methods (Convenience Methods)

    /// <summary>
    /// Get store details
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store response</returns>
    public async Task<StoreData> GetStoreAsync(CancellationToken cancellationToken = default)
    {
        return await Store.GetStoreAsync(cancellationToken);
    }

    #endregion

    #region Withdraw Methods (Convenience Methods)

    /// <summary>
    /// Create a new withdraw
    /// </summary>
    /// <param name="request">Withdraw request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> CreateWithdrawAsync(WithdrawRequest request, CancellationToken cancellationToken = default)
    {
        return await Withdraws.CreateWithdrawAsync(request, cancellationToken);
    }

    /// <summary>
    /// Get withdraw details by ID
    /// </summary>
    /// <param name="withdrawId">Withdraw ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> GetWithdrawAsync(string withdrawId, CancellationToken cancellationToken = default)
    {
        return await Withdraws.GetWithdrawAsync(withdrawId, cancellationToken);
    }

    /// <summary>
    /// List all withdraws
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> ListWithdrawsAsync(CancellationToken cancellationToken = default)
    {
        return await Withdraws.ListWithdrawsAsync(cancellationToken);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _httpClient?.Dispose();
            _disposed = true;
        }
    }

    #endregion
}
