using Newtonsoft.Json;

namespace AbacatePay.Models.Common;

/// <summary>
/// Base API response wrapper
/// </summary>
/// <typeparam name="T">Type of the data payload</typeparam>
public class ApiResponse<T>
{
        /// <summary>
    /// Error information if any
    /// </summary>
    [JsonProperty("error")]
    public object? Error { get; set; }

    /// <summary>
    /// Response data payload
    /// </summary>
    [JsonProperty("data")]
    public T? Data { get; set; }
}