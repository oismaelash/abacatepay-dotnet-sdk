using AbacatePay.Models;
using AbacatePay.Models.Common;

namespace AbacatePay.Services;

/// <summary>
/// Base service class that provides common functionality for all domain services
/// </summary>
public abstract class BaseService
{
    protected readonly IHttpService HttpService;

    protected BaseService(IHttpService httpService)
    {
        HttpService = httpService;
    }

    /// <summary>
    /// Executes a request and handles common error scenarios
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="requestFunc">Function that performs the HTTP request</param>
    /// <param name="operationName">Name of the operation for error messages</param>
    /// <returns>Response data</returns>
    protected async Task<T> ExecuteRequestAsync<T>(Func<Task<ApiResponse<T>>> requestFunc, string operationName)
    {
        var response = await requestFunc();
        
        if (response.Error != null)
        {
            throw new AbacatePayException($"{operationName} failed\n" + response.Error.ToString());
        }

        return response.Data ?? throw new AbacatePayException($"{operationName} failed - no data returned");
    }

    /// <summary>
    /// Executes a request that returns a list and handles common error scenarios
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="requestFunc">Function that performs the HTTP request</param>
    /// <param name="operationName">Name of the operation for error messages</param>
    /// <returns>List of response data</returns>
    protected async Task<List<T>> ExecuteListRequestAsync<T>(Func<Task<ApiResponse<List<T>>>> requestFunc, string operationName)
    {
        var response = await requestFunc();
        
        if (response.Error != null)
        {
            throw new AbacatePayException($"{operationName} failed\n" + response.Error.ToString());
        }

        return response.Data ?? new List<T>();
    }
}
