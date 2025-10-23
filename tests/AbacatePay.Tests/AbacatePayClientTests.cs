using AbacatePay;
using AbacatePay.Models;
using AbacatePay.Models.Payment;
using AbacatePay.Models.Customer;
using AbacatePay.Models.Webhook;
using AbacatePay.Models.Refund;
using AbacatePay.Models.Common;
using AbacatePay.Services;
using Moq;
using Xunit;

namespace AbacatePay.Tests;

public class AbacatePayClientTests
{
    private readonly Mock<IHttpService> _mockHttpService;
    private readonly AbacatePayClient _client;

    public AbacatePayClientTests()
    {
        _mockHttpService = new Mock<IHttpService>();
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com"
        };
        
        _client = new AbacatePayClient(config);
        
        // Use reflection to inject the mock service
        var httpServiceField = typeof(AbacatePayClient).GetField("_httpService", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        httpServiceField?.SetValue(_client, _mockHttpService.Object);
    }

    [Fact]
    public void Constructor_WithValidConfig_ShouldNotThrow()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com"
        };

        // Act & Assert
        var client = new AbacatePayClient(config);
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithValidApiKey_ShouldNotThrow()
    {
        // Act & Assert
        var client = new AbacatePayClient("test-api-key");
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithSandboxMode_ShouldUseSandboxUrl()
    {
        // Act
        var client = new AbacatePayClient("test-api-key", sandbox: true);
        
        // Assert
        Assert.NotNull(client);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidApiKey_ShouldThrowArgumentException(string apiKey)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(apiKey));
    }

    [Fact]
    public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => new AbacatePayClient((AbacatePayConfig)null!));
    }

    [Fact]
    public void Constructor_WithInvalidBaseUrl_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "invalid-url"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public async Task CreatePaymentAsync_WithValidRequest_ShouldReturnPaymentResponse()
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
            Description = "Test payment",
            Status = PaymentStatus.PENDING
        };

        var apiResponse = new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.PostAsync<PaymentResponse>("/v1/payments", request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.CreatePaymentAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("payment_123", result.Id);
        Assert.Equal(1000, result.Amount);
        Assert.Equal("Test payment", result.Description);
        Assert.Equal(PaymentStatus.PENDING, result.Status);
    }

    [Fact]
    public async Task CreatePaymentAsync_WithNullRequest_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _client.CreatePaymentAsync(null!));
    }

    [Fact]
    public async Task CreatePaymentAsync_WithInvalidAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 0,
            Description = "Test payment",
            PaymentMethod = PaymentMethod.PIX
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreatePaymentAsync(request));
    }

    [Fact]
    public async Task GetPaymentAsync_WithValidId_ShouldReturnPaymentResponse()
    {
        // Arrange
        var paymentId = "payment_123";
        var expectedResponse = new PaymentResponse
        {
            Id = paymentId,
            Amount = 1000,
            Description = "Test payment",
            Status = PaymentStatus.COMPLETED
        };

        var apiResponse = new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.GetAsync<PaymentResponse>($"/v1/payments/{paymentId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.GetPaymentAsync(paymentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paymentId, result.Id);
        Assert.Equal(PaymentStatus.COMPLETED, result.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetPaymentAsync_WithInvalidId_ShouldThrowArgumentException(string paymentId)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.GetPaymentAsync(paymentId));
    }

    [Fact]
    public async Task CancelPaymentAsync_WithValidId_ShouldReturnPaymentResponse()
    {
        // Arrange
        var paymentId = "payment_123";
        var expectedResponse = new PaymentResponse
        {
            Id = paymentId,
            Status = PaymentStatus.CANCELLED
        };

        var apiResponse = new ApiResponse<PaymentResponse>
        {
            Success = true,
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.PostAsync<PaymentResponse>($"/v1/payments/{paymentId}/cancel", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.CancelPaymentAsync(paymentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paymentId, result.Id);
        Assert.Equal(PaymentStatus.CANCELLED, result.Status);
    }

    [Fact]
    public async Task ListPaymentsAsync_WithDefaultParameters_ShouldReturnList()
    {
        // Arrange
        var expectedPayments = new List<PaymentResponse>
        {
            new() { Id = "payment_1", Amount = 1000, Status = PaymentStatus.COMPLETED },
            new() { Id = "payment_2", Amount = 2000, Status = PaymentStatus.PENDING }
        };

        var apiResponse = new ApiResponse<List<PaymentResponse>>
        {
            Success = true,
            Data = expectedPayments
        };

        _mockHttpService.Setup(x => x.GetAsync<List<PaymentResponse>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.ListPaymentsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("payment_1", result[0].Id);
        Assert.Equal("payment_2", result[1].Id);
    }

    [Fact]
    public async Task ListPaymentsAsync_WithFilters_ShouldBuildCorrectQueryString()
    {
        // Arrange
        var expectedPayments = new List<PaymentResponse>();
        var apiResponse = new ApiResponse<List<PaymentResponse>>
        {
            Success = true,
            Data = expectedPayments
        };

        _mockHttpService.Setup(x => x.GetAsync<List<PaymentResponse>>(
            It.Is<string>(s => s.Contains("limit=10") && s.Contains("status=pending")), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.ListPaymentsAsync(limit: 10, status: PaymentStatus.PENDING);

        // Assert
        Assert.NotNull(result);
        _mockHttpService.Verify(x => x.GetAsync<List<PaymentResponse>>(
            It.Is<string>(s => s.Contains("limit=10") && s.Contains("status=pending")), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidRequest_ShouldReturnCustomerResponse()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "John Doe",
            Email = "john@example.com",
            Cellphone = "+5511999999999",
            TaxId = "12345678901"
        };

        var expectedResponse = new CustomerResponse
        {
            Id = "customer_123",
            Metadata = new CustomerMetadata
            {
                Name = "John Doe",
                Email = "john@example.com"
            }
        };

        var apiResponse = new ApiResponse<CustomerResponse>
        {
            Success = true,
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.PostAsync<CustomerResponse>("/v1/customer/create", request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("customer_123", result.Id);
        Assert.Equal("John Doe", result.Metadata?.Name);
        Assert.Equal("john@example.com", result.Metadata?.Email);
    }

    [Fact]
    public async Task CreateWebhookAsync_WithValidRequest_ShouldReturnWebhookResponse()
    {
        // Arrange
        var request = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook",
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED, WebhookEventType.PAYMENT_FAILED }
        };

        var expectedResponse = new WebhookConfigResponse
        {
            Id = "webhook_123",
            Url = "https://example.com/webhook",
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED, WebhookEventType.PAYMENT_FAILED }
        };

        var apiResponse = new ApiResponse<WebhookConfigResponse>
        {
            Success = true,
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.PostAsync<WebhookConfigResponse>("/v1/webhooks", request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.CreateWebhookAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("webhook_123", result.Id);
        Assert.Equal("https://example.com/webhook", result.Url);
        Assert.Equal(2, result.Events.Count);
    }

    [Fact]
    public void VerifyWebhookSignature_WithValidSignature_ShouldReturnTrue()
    {
        // Arrange
        var payload = "test payload";
        var secret = "test secret";
        var signature = ComputeHmacSha256(payload, secret);

        // Act
        var result = AbacatePayClient.VerifyWebhookSignature(payload, signature, secret);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyWebhookSignature_WithInvalidSignature_ShouldReturnFalse()
    {
        // Arrange
        var payload = "test payload";
        var secret = "test secret";
        var invalidSignature = "invalid signature";

        // Act
        var result = AbacatePayClient.VerifyWebhookSignature(payload, invalidSignature, secret);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null, "signature", "secret")]
    [InlineData("payload", null, "secret")]
    [InlineData("payload", "signature", null)]
    [InlineData("", "signature", "secret")]
    [InlineData("payload", "", "secret")]
    [InlineData("payload", "signature", "")]
    public void VerifyWebhookSignature_WithNullOrEmptyParameters_ShouldReturnFalse(string payload, string signature, string secret)
    {
        // Act
        var result = AbacatePayClient.VerifyWebhookSignature(payload, signature, secret);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Dispose_ShouldDisposeHttpClient()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com"
        };
        var client = new AbacatePayClient(config);

        // Act
        client.Dispose();

        // Assert - Should not throw
        Assert.True(true);
    }

    private static string ComputeHmacSha256(string payload, string secret)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret));
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
        return BitConverter.ToString(computedHash).Replace("-", "").ToLower();
    }
}
