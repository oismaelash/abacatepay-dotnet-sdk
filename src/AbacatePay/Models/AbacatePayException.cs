namespace AbacatePay.Models;

/// <summary>
/// Exception thrown by AbacatePay SDK
/// </summary>
public class AbacatePayException : Exception
{
    /// <summary>
    /// HTTP status code of the response
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Error code from the API response
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Raw response body
    /// </summary>
    public string? ResponseBody { get; }

    public AbacatePayException(string message) : base(message)
    {
    }

    public AbacatePayException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public AbacatePayException(string message, int statusCode, string? errorCode = null, string? responseBody = null) 
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ResponseBody = responseBody;
    }
}
