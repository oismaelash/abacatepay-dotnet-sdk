using AbacatePay.Models;
using AbacatePay.Models.Common;
using AbacatePay.Models.Customer;
using AbacatePay.Models.Coupon;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;
using AbacatePay.Models.Store;
using AbacatePay.Models.Withdraw;
using AbacatePay.Services;
using System.ComponentModel.DataAnnotations;

namespace AbacatePay;

/// <summary>
/// Main AbacatePay client for interacting with the API
/// </summary>
public class AbacatePayClient : IDisposable
{
    private readonly IHttpService _httpService;
    private readonly HttpClient _httpClient;
    private bool _disposed;

    /// <summary>
    /// Initialize AbacatePay client with configuration
    /// </summary>
    /// <param name="config">Configuration object</param>
    public AbacatePayClient(AbacatePayConfig config)
    {
        ValidateConfig(config);
        
        _httpClient = new HttpClient();
        _httpService = new HttpService(_httpClient, config);
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




    #region Customer Methods

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="request">Customer request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer response</returns>
    public async Task<CustomerResponse> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default)
    {
        ValidateCustomerRequest(request);
        
        var response = await _httpService.PostAsync<CustomerResponse>("/v1/customer/create", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Customer creation failed - no data returned");
    }

    /// <summary>
    /// List all customers
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers</returns>
    public async Task<List<CustomerResponse>> ListCustomersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<List<CustomerResponse>>("/v1/customer/list", cancellationToken);
        return response.Data ?? new List<CustomerResponse>();
    }

    #endregion

    #region Coupon Methods

    /// <summary>
    /// Create a new coupon
    /// </summary>
    /// <param name="request">Coupon request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Coupon response</returns>
    public async Task<CouponResponse> CreateCouponAsync(CouponRequest request, CancellationToken cancellationToken = default)
    {
        ValidateCouponRequest(request);
        
        var response = await _httpService.PostAsync<CouponResponse>("/v1/coupon/create", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Coupon creation failed - no data returned");
    }

    /// <summary>
    /// List all coupons
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of coupons</returns>
    public async Task<List<CouponResponse>> ListCouponsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<List<CouponResponse>>("/v1/coupon/list", cancellationToken);
        return response.Data ?? new List<CouponResponse>();
    }

    #endregion

    #region Billing Methods

    /// <summary>
    /// Create a new billing
    /// </summary>
    /// <param name="request">Billing request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    public async Task<BillingResponse> CreateBillingAsync(BillingRequest request, CancellationToken cancellationToken = default)
    {
        ValidateBillingRequest(request);
        
        var response = await _httpService.PostAsync<BillingResponse>("/v1/billing/create", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Billing creation failed - no data returned");
    }

    /// <summary>
    /// Get billing details by ID
    /// </summary>
    /// <param name="billingId">Billing ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    public async Task<BillingResponse> GetBillingAsync(string billingId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(billingId))
            throw new ArgumentException("Billing ID is required", nameof(billingId));

        var response = await _httpService.GetAsync<BillingResponse>($"/v1/billing/get?id={billingId}", cancellationToken);
        return response.Data ?? throw new AbacatePayException("Billing not found");
    }

    /// <summary>
    /// List all billings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billings</returns>
    public async Task<List<BillingResponse>> ListBillingsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<List<BillingResponse>>("/v1/billing/list", cancellationToken);
        return response.Data ?? new List<BillingResponse>();
    }

    #endregion

    #region PIX QRCode Methods

    /// <summary>
    /// Create a new PIX QRCode
    /// </summary>
    /// <param name="request">PIX QRCode request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    public async Task<PixQrCodeResponse> CreatePixQrCodeAsync(PixQrCodeRequest request, CancellationToken cancellationToken = default)
    {
        ValidatePixQrCodeRequest(request);
        
        var response = await _httpService.PostAsync<PixQrCodeResponse>("/v1/pixQrCode/create", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("PIX QRCode creation failed - no data returned");
    }

    /// <summary>
    /// Check PIX QRCode status
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode status response</returns>
    public async Task<PixQrCodeStatusResponse> CheckPixQrCodeStatusAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pixQrCodeId))
            throw new ArgumentException("PIX QRCode ID is required", nameof(pixQrCodeId));

        var response = await _httpService.GetAsync<PixQrCodeStatusResponse>($"/v1/pixQrCode/check?id={pixQrCodeId}", cancellationToken);
        return response.Data ?? throw new AbacatePayException("PIX QRCode not found");
    }

    /// <summary>
    /// Simulate PIX QRCode payment (Dev Mode only)
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="request">Simulate payment request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    public async Task<PixQrCodeResponse> SimulatePixQrCodePaymentAsync(string pixQrCodeId, PixQrCodeSimulateRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pixQrCodeId))
            throw new ArgumentException("PIX QRCode ID is required", nameof(pixQrCodeId));

        var response = await _httpService.PostAsync<PixQrCodeResponse>($"/v1/pixQrCode/simulate-payment?id={pixQrCodeId}", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("PIX QRCode payment simulation failed - no data returned");
    }

    #endregion


    #region Store Methods

    /// <summary>
    /// Get store details
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store response</returns>
    public async Task<StoreData> GetStoreAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<StoreData>("/v1/store/get", cancellationToken);
        
        if (response.Error != null)
        {
            throw new AbacatePayException("Store not found\n" + response.Error.ToString());
        }

        return response.Data ?? throw new AbacatePayException("Store not found");
    }

    #endregion

    #region Withdraw Methods

    /// <summary>
    /// Create a new withdraw
    /// </summary>
    /// <param name="request">Withdraw request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> CreateWithdrawAsync(WithdrawRequest request, CancellationToken cancellationToken = default)
    {
        ValidateWithdrawRequest(request);
        
        var response = await _httpService.PostAsync<WithdrawData>("/v1/withdraw/create", request, cancellationToken);
        
        if (response.Error != null)
        {
            throw new AbacatePayException("Withdraw creation failed\n" + response.Error.ToString());
        }

        return response.Data ?? throw new AbacatePayException("Withdraw creation failed - no data returned");
    }

    /// <summary>
    /// Get withdraw details by ID
    /// </summary>
    /// <param name="withdrawId">Withdraw ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> GetWithdrawAsync(string withdrawId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(withdrawId))
            throw new ArgumentException("Withdraw ID is required", nameof(withdrawId));

        var response = await _httpService.GetAsync<WithdrawData>($"/v1/withdraw/get?id={withdrawId}", cancellationToken);
        
        if (response.Error != null)
        {
            throw new AbacatePayException("Withdraw not found\n" + response.Error.ToString());
        }
        
        return response.Data ?? throw new AbacatePayException("Withdraw not found");
    }

    /// <summary>
    /// List all withdraws
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of withdraws</returns>
    public async Task<WithdrawData> ListWithdrawsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<WithdrawData>("/v1/withdraw/list", cancellationToken);
        
        if (response.Error != null)
        {
            throw new AbacatePayException("Withdraw list failed\n" + response.Error.ToString());
        }

        return response.Data ?? throw new AbacatePayException("Withdraw list failed - no data returned");
    }

    #endregion

    #region Validation Methods


    private static void ValidateCustomerRequest(CustomerRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Customer request validation failed: {string.Join(", ", errorMessages)}");
        }
    }

    private static void ValidateCouponRequest(CouponRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Coupon request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (request.Data.MaxRedeems < -1)
            throw new ArgumentException("MaxRedeems must be -1 (unlimited) or greater than 0", nameof(request.Data.MaxRedeems));
    }

    private static void ValidateBillingRequest(BillingRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Billing request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (!Uri.TryCreate(request.ReturnUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid return URL format", nameof(request.ReturnUrl));

        if (!Uri.TryCreate(request.CompletionUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid completion URL format", nameof(request.CompletionUrl));

        if (request.Products == null || !request.Products.Any())
            throw new ArgumentException("At least one product is required", nameof(request.Products));

        foreach (var product in request.Products)
        {
            if (product.Quantity < 1)
                throw new ArgumentException("Product quantity must be at least 1", nameof(product.Quantity));

            if (product.Price < 100)
                throw new ArgumentException("Product price must be at least 100 cents", nameof(product.Price));
        }

        if (string.IsNullOrWhiteSpace(request.CustomerId) && request.Customer == null)
            throw new ArgumentException("Either CustomerId or Customer data must be provided");
    }

    private static void ValidatePixQrCodeRequest(PixQrCodeRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"PIX QRCode request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (request.Amount < 100)
            throw new ArgumentException("Amount must be at least 100 cents", nameof(request.Amount));
    }

    private static void ValidateWithdrawRequest(WithdrawRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Withdraw request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (request.Amount >= 300)
            throw new ArgumentException("Amount must be at least 100 cents", nameof(request.Amount));

        if (request.Pix == null)
            throw new ArgumentException("PIX information is required", nameof(request.Pix));

        // PIX type validation is handled by the enum - no need for string validation

        if (string.IsNullOrWhiteSpace(request.Pix.Key))
            throw new ArgumentException("PIX key is required", nameof(request.Pix.Key));

        if (request.Method != "PIX")
            throw new ArgumentException("Method must be 'PIX'", nameof(request.Method));
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
