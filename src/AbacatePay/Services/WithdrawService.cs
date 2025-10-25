using AbacatePay.Models.Withdraw;

namespace AbacatePay.Services;

/// <summary>
/// Service implementation for withdraw operations
/// </summary>
public class WithdrawService : BaseService, IWithdrawService
{
    public WithdrawService(IHttpService httpService) : base(httpService)
    {
    }

    /// <summary>
    /// Create a new withdraw
    /// </summary>
    /// <param name="request">Withdraw request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> CreateWithdrawAsync(WithdrawRequest request, CancellationToken cancellationToken = default)
    {
        return await ExecuteRequestAsync(
            () => HttpService.PostAsync<WithdrawData>("/v1/withdraw/create", request, cancellationToken),
            "Withdraw creation"
        );
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

        return await ExecuteRequestAsync(
            () => HttpService.GetAsync<WithdrawData>($"/v1/withdraw/get?externalId={withdrawId}", cancellationToken),
            "Withdraw retrieval"
        );
    }

    /// <summary>
    /// List all withdraws
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    public async Task<WithdrawData> ListWithdrawsAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteRequestAsync(
            () => HttpService.GetAsync<WithdrawData>("/v1/withdraw/list", cancellationToken),
            "Withdraw list"
        );
    }
}
