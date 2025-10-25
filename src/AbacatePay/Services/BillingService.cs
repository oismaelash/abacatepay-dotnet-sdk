using AbacatePay.Models.Billing;
using AbacatePay.Validators;

namespace AbacatePay.Services;

/// <summary>
/// Service implementation for billing operations
/// </summary>
public class BillingService : BaseService, IBillingService
{
    public BillingService(IHttpService httpService) : base(httpService)
    {
    }

    /// <summary>
    /// Create a new billing
    /// </summary>
    /// <param name="request">Billing request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    public async Task<BillingData> CreateBillingAsync(BillingRequest request, CancellationToken cancellationToken = default)
    {
        RequestValidator.ValidateBillingRequest(request);
        
        return await ExecuteRequestAsync(
            () => HttpService.PostAsync<BillingData>("/v1/billing/create", request, cancellationToken),
            "Billing creation"
        );
    }

    /// <summary>
    /// Get billing details by ID
    /// </summary>
    /// <param name="billingId">Billing ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    public async Task<BillingData> GetBillingAsync(string billingId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(billingId))
            throw new ArgumentException("Billing ID is required", nameof(billingId));

        return await ExecuteRequestAsync(
            () => HttpService.GetAsync<BillingData>($"/v1/billing/get?id={billingId}", cancellationToken),
            "Billing retrieval"
        );
    }

    /// <summary>
    /// List all billings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billings</returns>
    public async Task<List<BillingData>> ListBillingsAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteListRequestAsync(
            () => HttpService.GetAsync<List<BillingData>>("/v1/billing/list", cancellationToken),
            "Billing list"
        );
    }
}
