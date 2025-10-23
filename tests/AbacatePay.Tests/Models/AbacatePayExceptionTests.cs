using AbacatePay.Models;
using Xunit;

namespace AbacatePay.Tests.Models;

public class AbacatePayExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Test exception message";

        // Act
        var exception = new AbacatePayException(message);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(0, exception.StatusCode);
        Assert.Null(exception.ErrorCode);
        Assert.Null(exception.ResponseBody);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        // Arrange
        var message = "Test exception message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new AbacatePayException(message, innerException);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
        Assert.Equal(0, exception.StatusCode);
        Assert.Null(exception.ErrorCode);
        Assert.Null(exception.ResponseBody);
    }

    [Fact]
    public void Constructor_WithAllParameters_ShouldSetAllProperties()
    {
        // Arrange
        var message = "Test exception message";
        var statusCode = 400;
        var errorCode = "INVALID_REQUEST";
        var responseBody = "{\"error\":\"Invalid request\"}";

        // Act
        var exception = new AbacatePayException(message, statusCode, errorCode, responseBody);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(statusCode, exception.StatusCode);
        Assert.Equal(errorCode, exception.ErrorCode);
        Assert.Equal(responseBody, exception.ResponseBody);
    }

    [Fact]
    public void Constructor_WithStatusCodeOnly_ShouldSetStatusCode()
    {
        // Arrange
        var message = "Test exception message";
        var statusCode = 500;

        // Act
        var exception = new AbacatePayException(message, statusCode);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(statusCode, exception.StatusCode);
        Assert.Null(exception.ErrorCode);
        Assert.Null(exception.ResponseBody);
    }
}
