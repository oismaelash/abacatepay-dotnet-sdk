using AbacatePay.Models.Billing;

namespace AbacatePay.Services;

/// <summary>
/// Service interface for billing operations
/// </summary>
public interface IBillingService
{
    /// <summary>
    /// Create a new billing
    /// </summary>
    /// <param name="request">Billing request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    Task<BillingData> CreateBillingAsync(BillingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get billing details by ID
    /// </summary>
    /// <param name="billingId">Billing ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing response</returns>
    Task<BillingData> GetBillingAsync(string billingId, CancellationToken cancellationToken = default);

    /// <summary>
    /// List all billings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of billings</returns>
    Task<List<BillingData>> ListBillingsAsync(CancellationToken cancellationToken = default);
}
