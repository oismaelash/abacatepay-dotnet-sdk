using AbacatePay.Models;
using AbacatePay.Models.Common;
using AbacatePay.Models.Payment;
using AbacatePay.Models.Webhook;
using AbacatePay.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using Xunit;

namespace AbacatePay.Tests.Services;

public class HttpServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AbacatePayConfig _config;
    private readonly HttpService _httpService;

    public HttpServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com",
            TimeoutSeconds = 30
        };
        _httpService = new HttpService(_httpClient, _config);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldNotThrow()
    {
        // Act & Assert
        var service = new HttpService(_httpClient, _config);
        Assert.NotNull(service);
    }

    [Fact]
    public async Task GetAsync_WithSuccessfulResponse_ShouldReturnApiResponse()
    {
        // Arrange
        var expectedResponse = new PaymentResponse
        {
            Id = "payment_123",
            Amount = 1000,
            Status = PaymentStatus.COMPLETED
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = expectedResponse
        });

        SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpService.GetAsync<PaymentResponse>("/v1/payments/123");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("payment_123", result.Data.Id);
        Assert.Equal(1000, result.Data.Amount);
        Assert.Equal(PaymentStatus.COMPLETED, result.Data.Status);
    }

    [Fact]
    public async Task PostAsync_WithSuccessfulResponse_ShouldReturnApiResponse()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 1000,
            Description = "Test payment",
            PaymentMethod = PaymentMethod.PIX
        };

        var expectedResponse = new PaymentResponse
        {
            Id = "payment_123",
            Amount = 1000,
            Status = PaymentStatus.PENDING
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = expectedResponse
        });

        SetupHttpResponse(HttpStatusCode.Created, jsonResponse);

        // Act
        var result = await _httpService.PostAsync<PaymentResponse>("/v1/payments", request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("payment_123", result.Data.Id);
        Assert.Equal(PaymentStatus.PENDING, result.Data.Status);
    }

    [Fact]
    public async Task PutAsync_WithSuccessfulResponse_ShouldReturnApiResponse()
    {
        // Arrange
        var request = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook",
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED }
        };

        var expectedResponse = new WebhookConfigResponse
        {
            Id = "webhook_123",
            Url = "https://example.com/webhook"
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new ApiResponse<WebhookConfigResponse>
        {
            Success = true,
            Data = expectedResponse
        });

        SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpService.PutAsync<WebhookConfigResponse>("/v1/webhooks/123", request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("webhook_123", result.Data.Id);
    }

    [Fact]
    public async Task DeleteAsync_WithSuccessfulResponse_ShouldReturnApiResponse()
    {
        // Arrange
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new ApiResponse<object>
        {
            Success = true
        });

        SetupHttpResponse(HttpStatusCode.NoContent, jsonResponse);

        // Act
        var result = await _httpService.DeleteAsync<object>("/v1/webhooks/123");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetAsync_WithErrorResponse_ShouldThrowAbacatePayException()
    {
        // Arrange
        var errorResponse = new ApiResponse<object>
        {
            Success = false,
            Error = new ErrorDetails
            {
                Code = "INVALID_REQUEST",
                Message = "Invalid request parameters"
            }
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorResponse);
        SetupHttpResponse(HttpStatusCode.BadRequest, jsonResponse);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AbacatePayException>(
            () => _httpService.GetAsync<PaymentResponse>("/v1/payments/invalid"));

        Assert.Equal("Invalid request parameters", exception.Message);
        Assert.Equal(400, exception.StatusCode);
        Assert.Equal("INVALID_REQUEST", exception.ErrorCode);
    }

    [Fact]
    public async Task PostAsync_WithTimeout_ShouldThrowAbacatePayException()
    {
        // Arrange
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<AbacatePayException>(
            () => _httpService.PostAsync<PaymentResponse>("/v1/payments", new PaymentRequest()));
    }

    [Fact]
    public async Task GetAsync_WithCancellation_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource();
        cancellationToken.Cancel();

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _httpService.GetAsync<PaymentResponse>("/v1/payments", cancellationToken.Token));
    }

    [Fact]
    public async Task PostAsync_WithNullData_ShouldSendRequestWithoutContent()
    {
        // Arrange
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(new ApiResponse<object>
        {
            Success = true
        });

        SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpService.PostAsync<object>("/v1/payments/123/cancel", null);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    private void SetupHttpResponse(HttpStatusCode statusCode, string content)
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });
    }
}
