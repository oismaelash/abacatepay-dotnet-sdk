using Newtonsoft.Json;

namespace AbacatePay.Models.Common;

/// <summary>
/// Base API response wrapper
/// </summary>
/// <typeparam name="T">Type of the data payload</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Response message
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Response data
    /// </summary>
    [JsonProperty("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Error details if any
    /// </summary>
    [JsonProperty("error")]
    public ErrorDetails? Error { get; set; }
}

/// <summary>
/// Error details in API response
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Error code
    /// </summary>
    [JsonProperty("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Additional error details
    /// </summary>
    [JsonProperty("details")]
    public object? Details { get; set; }
}
