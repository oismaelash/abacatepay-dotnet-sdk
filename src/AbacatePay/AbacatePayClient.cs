using AbacatePay.Models;
using AbacatePay.Models.Common;
using AbacatePay.Models.Payment;
using AbacatePay.Models.Refund;
using AbacatePay.Models.Webhook;
using AbacatePay.Models.Customer;
using AbacatePay.Models.Coupon;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;
using AbacatePay.Models.Withdraw;
using AbacatePay.Models.Store;
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
            BaseUrl = sandbox ? "https://sandbox.abacatepay.com" : "https://api.abacatepay.com"
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

    #region Payment Methods

    /// <summary>
    /// Create a new payment
    /// </summary>
    /// <param name="request">Payment request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment response</returns>
    public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        ValidatePaymentRequest(request);
        
        var response = await _httpService.PostAsync<PaymentResponse>("/v1/payments", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Payment creation failed - no data returned");
    }

    /// <summary>
    /// Get payment details by ID
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment response</returns>
    public async Task<PaymentResponse> GetPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
            throw new ArgumentException("Payment ID is required", nameof(paymentId));

        var response = await _httpService.GetAsync<PaymentResponse>($"/v1/payments/{paymentId}", cancellationToken);
        return response.Data ?? throw new AbacatePayException("Payment not found");
    }

    /// <summary>
    /// Cancel a payment
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment response</returns>
    public async Task<PaymentResponse> CancelPaymentAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
            throw new ArgumentException("Payment ID is required", nameof(paymentId));

        var response = await _httpService.PostAsync<PaymentResponse>($"/v1/payments/{paymentId}/cancel", null, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Payment cancellation failed - no data returned");
    }

    /// <summary>
    /// List payments with optional filters
    /// </summary>
    /// <param name="limit">Number of payments to return (default: 20, max: 100)</param>
    /// <param name="offset">Number of payments to skip (default: 0)</param>
    /// <param name="status">Filter by payment status</param>
    /// <param name="createdAfter">Filter payments created after this date</param>
    /// <param name="createdBefore">Filter payments created before this date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of payments</returns>
    public async Task<List<PaymentResponse>> ListPaymentsAsync(
        int limit = 20, 
        int offset = 0, 
        PaymentStatus? status = null, 
        DateTime? createdAfter = null, 
        DateTime? createdBefore = null, 
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();
        
        if (limit > 0) queryParams.Add($"limit={limit}");
        if (offset > 0) queryParams.Add($"offset={offset}");
        if (status.HasValue) queryParams.Add($"status={status.Value.ToString().ToLower()}");
        if (createdAfter.HasValue) queryParams.Add($"created_after={createdAfter.Value:yyyy-MM-ddTHH:mm:ssZ}");
        if (createdBefore.HasValue) queryParams.Add($"created_before={createdBefore.Value:yyyy-MM-ddTHH:mm:ssZ}");

        var endpoint = "/v1/payments";
        if (queryParams.Count > 0)
            endpoint += "?" + string.Join("&", queryParams);

        var response = await _httpService.GetAsync<List<PaymentResponse>>(endpoint, cancellationToken);
        return response.Data ?? new List<PaymentResponse>();
    }

    #endregion

    #region Refund Methods

    /// <summary>
    /// Create a refund for a payment
    /// </summary>
    /// <param name="request">Refund request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Refund response</returns>
    public async Task<RefundResponse> CreateRefundAsync(RefundRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRefundRequest(request);
        
        var response = await _httpService.PostAsync<RefundResponse>("/v1/refunds", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Refund creation failed - no data returned");
    }

    /// <summary>
    /// Get refund details by ID
    /// </summary>
    /// <param name="refundId">Refund ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Refund response</returns>
    public async Task<RefundResponse> GetRefundAsync(string refundId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refundId))
            throw new ArgumentException("Refund ID is required", nameof(refundId));

        var response = await _httpService.GetAsync<RefundResponse>($"/v1/refunds/{refundId}", cancellationToken);
        return response.Data ?? throw new AbacatePayException("Refund not found");
    }

    /// <summary>
    /// List refunds with optional filters
    /// </summary>
    /// <param name="limit">Number of refunds to return (default: 20, max: 100)</param>
    /// <param name="offset">Number of refunds to skip (default: 0)</param>
    /// <param name="status">Filter by refund status</param>
    /// <param name="paymentId">Filter by payment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of refunds</returns>
    public async Task<List<RefundResponse>> ListRefundsAsync(
        int limit = 20, 
        int offset = 0, 
        RefundStatus? status = null, 
        string? paymentId = null, 
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();
        
        if (limit > 0) queryParams.Add($"limit={limit}");
        if (offset > 0) queryParams.Add($"offset={offset}");
        if (status.HasValue) queryParams.Add($"status={status.Value.ToString().ToLower()}");
        if (!string.IsNullOrWhiteSpace(paymentId)) queryParams.Add($"payment_id={paymentId}");

        var endpoint = "/v1/refunds";
        if (queryParams.Count > 0)
            endpoint += "?" + string.Join("&", queryParams);

        var response = await _httpService.GetAsync<List<RefundResponse>>(endpoint, cancellationToken);
        return response.Data ?? new List<RefundResponse>();
    }

    #endregion

    #region Webhook Methods

    /// <summary>
    /// Create a webhook configuration
    /// </summary>
    /// <param name="request">Webhook configuration request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Webhook configuration response</returns>
    public async Task<WebhookConfigResponse> CreateWebhookAsync(WebhookConfigRequest request, CancellationToken cancellationToken = default)
    {
        ValidateWebhookRequest(request);
        
        var response = await _httpService.PostAsync<WebhookConfigResponse>("/v1/webhooks", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Webhook creation failed - no data returned");
    }

    /// <summary>
    /// Get webhook configuration by ID
    /// </summary>
    /// <param name="webhookId">Webhook ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Webhook configuration response</returns>
    public async Task<WebhookConfigResponse> GetWebhookAsync(string webhookId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(webhookId))
            throw new ArgumentException("Webhook ID is required", nameof(webhookId));

        var response = await _httpService.GetAsync<WebhookConfigResponse>($"/v1/webhooks/{webhookId}", cancellationToken);
        return response.Data ?? throw new AbacatePayException("Webhook not found");
    }

    /// <summary>
    /// Update webhook configuration
    /// </summary>
    /// <param name="webhookId">Webhook ID</param>
    /// <param name="request">Webhook configuration request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Webhook configuration response</returns>
    public async Task<WebhookConfigResponse> UpdateWebhookAsync(string webhookId, WebhookConfigRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(webhookId))
            throw new ArgumentException("Webhook ID is required", nameof(webhookId));

        ValidateWebhookRequest(request);
        
        var response = await _httpService.PutAsync<WebhookConfigResponse>($"/v1/webhooks/{webhookId}", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Webhook update failed - no data returned");
    }

    /// <summary>
    /// Delete webhook configuration
    /// </summary>
    /// <param name="webhookId">Webhook ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteWebhookAsync(string webhookId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(webhookId))
            throw new ArgumentException("Webhook ID is required", nameof(webhookId));

        await _httpService.DeleteAsync<object>($"/v1/webhooks/{webhookId}", cancellationToken);
    }

    /// <summary>
    /// List webhook configurations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of webhook configurations</returns>
    public async Task<List<WebhookConfigResponse>> ListWebhooksAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<List<WebhookConfigResponse>>("/v1/webhooks", cancellationToken);
        return response.Data ?? new List<WebhookConfigResponse>();
    }

    /// <summary>
    /// Verify webhook signature
    /// </summary>
    /// <param name="payload">Webhook payload</param>
    /// <param name="signature">Webhook signature</param>
    /// <param name="secret">Webhook secret</param>
    /// <returns>True if signature is valid</returns>
    public static bool VerifyWebhookSignature(string payload, string signature, string secret)
    {
        if (string.IsNullOrWhiteSpace(payload) || string.IsNullOrWhiteSpace(signature) || string.IsNullOrWhiteSpace(secret))
            return false;

        try
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret));
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
            var computedSignature = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
            
            return computedSignature == signature.ToLower();
        }
        catch
        {
            return false;
        }
    }

    #endregion

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

    #region Withdraw Methods

    /// <summary>
    /// Create a new withdraw
    /// </summary>
    /// <param name="request">Withdraw request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawResponse> CreateWithdrawAsync(WithdrawRequest request, CancellationToken cancellationToken = default)
    {
        ValidateWithdrawRequest(request);
        
        var response = await _httpService.PostAsync<WithdrawResponse>("/v1/withdraw/create", request, cancellationToken);
        return response.Data ?? throw new AbacatePayException("Withdraw creation failed - no data returned");
    }

    /// <summary>
    /// Get withdraw details by ID
    /// </summary>
    /// <param name="withdrawId">Withdraw ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawResponse> GetWithdrawAsync(string withdrawId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(withdrawId))
            throw new ArgumentException("Withdraw ID is required", nameof(withdrawId));

        var response = await _httpService.GetAsync<WithdrawResponse>($"/v1/withdraw/get?id={withdrawId}", cancellationToken);
        return response.Data ?? throw new AbacatePayException("Withdraw not found");
    }

    /// <summary>
    /// List all withdraws
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of withdraws</returns>
    public async Task<List<WithdrawResponse>> ListWithdrawsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<List<WithdrawResponse>>("/v1/withdraw/list", cancellationToken);
        return response.Data ?? new List<WithdrawResponse>();
    }

    #endregion

    #region Store Methods

    /// <summary>
    /// Get store details
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store response</returns>
    public async Task<StoreResponse> GetStoreAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpService.GetAsync<StoreResponse>("/v1/store/get", cancellationToken);
        return response.Data ?? throw new AbacatePayException("Store not found");
    }

    #endregion

    #region Validation Methods

    private static void ValidatePaymentRequest(PaymentRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Payment request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (request.Amount <= 0)
            throw new ArgumentException("Payment amount must be greater than 0", nameof(request.Amount));
    }

    private static void ValidateRefundRequest(RefundRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Refund request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (request.Amount.HasValue && request.Amount <= 0)
            throw new ArgumentException("Refund amount must be greater than 0", nameof(request.Amount));
    }

    private static void ValidateWebhookRequest(WebhookConfigRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Webhook request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid webhook URL format", nameof(request.Url));
    }

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

        if (request.Amount < 100)
            throw new ArgumentException("Amount must be at least 100 cents", nameof(request.Amount));
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
