using AbacatePay.Models.Withdraw;

namespace AbacatePay.Services;

/// <summary>
/// Service interface for withdraw operations
/// </summary>
public interface IWithdrawService
{
    /// <summary>
    /// Create a new withdraw
    /// </summary>
    /// <param name="request">Withdraw request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    Task<WithdrawData> CreateWithdrawAsync(WithdrawRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get withdraw details by ID
    /// </summary>
    /// <param name="withdrawId">Withdraw ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    Task<WithdrawData> GetWithdrawAsync(string withdrawId, CancellationToken cancellationToken = default);

    /// <summary>
    /// List all withdraws
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Withdraw response</returns>
    Task<WithdrawData> ListWithdrawsAsync(CancellationToken cancellationToken = default);
}
