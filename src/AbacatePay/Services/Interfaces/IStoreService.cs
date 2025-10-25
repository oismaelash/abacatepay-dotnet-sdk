using AbacatePay.Models.Store;

namespace AbacatePay.Services;

/// <summary>
/// Service interface for store operations
/// </summary>
public interface IStoreService
{
    /// <summary>
    /// Get store details
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store response</returns>
    Task<StoreData> GetStoreAsync(CancellationToken cancellationToken = default);
}
