using AbacatePay.Models.Store;

namespace AbacatePay.Services;

/// <summary>
/// Service implementation for store operations
/// </summary>
public class StoreService : BaseService, IStoreService
{
    public StoreService(IHttpService httpService) : base(httpService)
    {
    }

    /// <summary>
    /// Get store details
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store response</returns>
    public async Task<StoreData> GetStoreAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteRequestAsync(
            () => HttpService.GetAsync<StoreData>("/v1/store/get", cancellationToken),
            "Store retrieval"
        );
    }
}
