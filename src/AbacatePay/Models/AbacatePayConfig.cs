namespace AbacatePay.Models;

/// <summary>
/// Configuration for AbacatePay client
/// </summary>
public class AbacatePayConfig
{
    /// <summary>
    /// Your Bearer token from AbacatePay dashboard
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Base URL for the API (defaults to production)
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.abacatepay.com";

    /// <summary>
    /// Timeout for HTTP requests in seconds (default: 30)
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Whether to use sandbox mode (default: false)
    /// </summary>
    public bool Sandbox { get; set; } = false;
}
