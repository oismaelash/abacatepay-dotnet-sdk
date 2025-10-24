using AbacatePay.Models.Common;

namespace AbacatePay.Services;

/// <summary>
/// HTTP service interface for API communication
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Send a GET request
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="endpoint">API endpoint</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>API response</returns>
    Task<ApiResponse<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);

    /// <summary>
    /// Send a POST request
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="endpoint">API endpoint</param>
    /// <param name="data">Request data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>API response</returns>
    Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null, CancellationToken cancellationToken = default);
}
