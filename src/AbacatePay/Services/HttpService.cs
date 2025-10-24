using System.Net.Http.Headers;
using System.Text;
using AbacatePay.Models;
using AbacatePay.Models.Common;
using Newtonsoft.Json;

namespace AbacatePay.Services;

/// <summary>
/// HTTP service implementation for API communication
/// </summary>
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly AbacatePayConfig _config;

    public HttpService(HttpClient httpClient, AbacatePayConfig config)
    {
        _httpClient = httpClient;
        _config = config;
        ConfigureHttpClient();
    }

    private void ConfigureHttpClient()
    {
        _httpClient.BaseAddress = new Uri(_config.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);

        // Set authentication header (Bearer token)
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.ApiKey);

        // Set default headers
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "AbacatePay-DotNet-SDK/1.0.0");
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            return await ProcessResponse<T>(response);
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("Request was cancelled", cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new AbacatePayException("Request timeout");
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var content = data != null ? CreateJsonContent(data) : null;
            var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
            return await ProcessResponse<T>(response);
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("Request was cancelled", cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new AbacatePayException("Request timeout");
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var content = data != null ? CreateJsonContent(data) : null;
            var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);
            return await ProcessResponse<T>(response);
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("Request was cancelled", cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new AbacatePayException("Request timeout");
        }
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
            return await ProcessResponse<T>(response);
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("Request was cancelled", cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new AbacatePayException("Request timeout");
        }
    }

    public async Task<ApiResponseCustom<T>> GetCustomAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            return await ProcessCustomResponse<T>(response);
        }
        catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException("Request was cancelled", cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new AbacatePayException("Request timeout");
        }
    }

    private static HttpContent? CreateJsonContent(object data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = TryDeserializeError(responseBody);
            throw new AbacatePayException(
                errorResponse?.Message ?? $"HTTP {response.StatusCode}: {response.ReasonPhrase}",
                (int)response.StatusCode,
                errorResponse?.Code,
                responseBody
            );
        }

        try
        {
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseBody);
            if (apiResponse == null)
            {
                throw new AbacatePayException("Failed to deserialize response");
            }

            if (!apiResponse.Success && apiResponse.Error != null)
            {
                throw new AbacatePayException(
                    apiResponse.Error.Message ?? "API request failed",
                    (int)response.StatusCode,
                    apiResponse.Error.Code,
                    responseBody
                );
            }

            return apiResponse;
        }
        catch (JsonException ex)
        {
            throw new AbacatePayException($"Failed to parse response: {ex.Message}", ex);
        }
    }

    private async Task<ApiResponseCustom<T>> ProcessCustomResponse<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = TryDeserializeCustomError(responseBody);
            throw new AbacatePayException(
                errorResponse?.ToString() ?? $"HTTP {response.StatusCode}: {response.ReasonPhrase}",
                (int)response.StatusCode,
                null,
                responseBody
            );
        }

        try
        {
            var apiResponse = JsonConvert.DeserializeObject<ApiResponseCustom<T>>(responseBody);
            if (apiResponse == null)
            {
                throw new AbacatePayException("Failed to deserialize response");
            }

            if (apiResponse.Error != null)
            {
                throw new AbacatePayException(
                    apiResponse.Error.ToString() ?? "API request failed",
                    (int)response.StatusCode,
                    null,
                    responseBody
                );
            }

            return apiResponse;
        }
        catch (JsonException ex)
        {
            throw new AbacatePayException($"Failed to parse response: {ex.Message}", ex);
        }
    }

    private static ErrorDetails? TryDeserializeError(string responseBody)
    {
        try
        {
            return JsonConvert.DeserializeObject<ApiResponse<object>>(responseBody)?.Error;
        }
        catch
        {
            return null;
        }
    }

    private static object? TryDeserializeCustomError(string responseBody)
    {
        try
        {
            return JsonConvert.DeserializeObject<ApiResponseCustom<object>>(responseBody)?.Error;
        }
        catch
        {
            return null;
        }
    }
}
