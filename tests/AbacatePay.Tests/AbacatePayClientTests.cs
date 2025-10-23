using AbacatePay;
using AbacatePay.Models;
using AbacatePay.Models.Customer;
using AbacatePay.Models.Coupon;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;
using AbacatePay.Models.Store;
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
    public void Constructor_WithNullConfig_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AbacatePayClient(null!));
    }

    [Fact]
    public void Constructor_WithEmptyApiKey_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "",
            BaseUrl = "https://api.abacatepay.com"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public void Constructor_WithNullApiKey_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = null!,
            BaseUrl = "https://api.abacatepay.com"
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = null!
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public void Constructor_WithEmptyBaseUrl_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = ""
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public void Constructor_WithNegativeTimeout_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com",
            TimeoutSeconds = -1
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public void Constructor_WithZeroTimeout_ShouldThrowArgumentException()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com",
            TimeoutSeconds = 0
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(config));
    }

    [Fact]
    public void Constructor_WithValidTimeout_ShouldNotThrow()
    {
        // Arrange
        var config = new AbacatePayConfig
        {
            ApiKey = "test-api-key",
            BaseUrl = "https://api.abacatepay.com",
            TimeoutSeconds = 30
        };

        // Act & Assert
        var client = new AbacatePayClient(config);
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithApiKeyAndSandbox_ShouldNotThrow()
    {
        // Act & Assert
        var client = new AbacatePayClient("test-api-key", sandbox: true);
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithApiKeyOnly_ShouldNotThrow()
    {
        // Act & Assert
        var client = new AbacatePayClient("test-api-key");
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_WithEmptyApiKeyString_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(""));
    }

    [Fact]
    public void Constructor_WithNullApiKeyString_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AbacatePayClient(null!));
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var client = new AbacatePayClient("test-api-key");

        // Act & Assert
        client.Dispose();
        client.Dispose(); // Should not throw on multiple dispose calls
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidRequest_ShouldCallHttpService()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "Test Customer",
            Email = "test@example.com",
            Cellphone = "+5511999999999",
            TaxId = "12345678900"
        };

        var expectedResponse = new CustomerResponse
        {
            Id = "customer-123",
            Metadata = new CustomerMetadata
            {
                Name = "Test Customer",
                Email = "test@example.com"
            }
        };

        var apiResponse = new ApiResponse<CustomerResponse>
        {
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.PostAsync<CustomerResponse>(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("customer-123", result.Id);
        _mockHttpService.Verify(x => x.PostAsync<CustomerResponse>(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListCustomersAsync_ShouldCallHttpService()
    {
        // Arrange
        var expectedResponse = new List<CustomerResponse>
        {
            new CustomerResponse { Id = "customer-1", Metadata = new CustomerMetadata { Name = "Customer 1" } },
            new CustomerResponse { Id = "customer-2", Metadata = new CustomerMetadata { Name = "Customer 2" } }
        };

        var apiResponse = new ApiResponse<List<CustomerResponse>>
        {
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.GetAsync<List<CustomerResponse>>(
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.ListCustomersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockHttpService.Verify(x => x.GetAsync<List<CustomerResponse>>(
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateCouponAsync_WithValidRequest_ShouldCallHttpService()
    {
        // Arrange
        var request = new CouponRequest
        {
            Data = new CouponData
            {
                Code = "TEST10",
                MaxRedeems = 10,
                DiscountKind = DiscountKind.PERCENTAGE,
                Discount = 10
            }
        };

        var expectedResponse = new CouponResponse
        {
            Id = "coupon-123",
            MaxRedeems = 10,
            DiscountKind = DiscountKind.PERCENTAGE,
            Discount = 10
        };

        var apiResponse = new ApiResponse<CouponResponse>
        {
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.PostAsync<CouponResponse>(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.CreateCouponAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("coupon-123", result.Id);
        _mockHttpService.Verify(x => x.PostAsync<CouponResponse>(
            It.IsAny<string>(), 
            It.IsAny<object>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ListCouponsAsync_ShouldCallHttpService()
    {
        // Arrange
        var expectedResponse = new List<CouponResponse>
        {
            new CouponResponse { Id = "coupon-1", MaxRedeems = 10 },
            new CouponResponse { Id = "coupon-2", MaxRedeems = 20 }
        };

        var apiResponse = new ApiResponse<List<CouponResponse>>
        {
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.GetAsync<List<CouponResponse>>(
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.ListCouponsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        _mockHttpService.Verify(x => x.GetAsync<List<CouponResponse>>(
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetStoreAsync_ShouldCallHttpService()
    {
        // Arrange
        var expectedResponse = new StoreResponse
        {
            Id = "store-123"
        };

        var apiResponse = new ApiResponse<StoreResponse>
        {
            Data = expectedResponse
        };

        _mockHttpService.Setup(x => x.GetAsync<StoreResponse>(
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _client.GetStoreAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("store-123", result.Id);
        _mockHttpService.Verify(x => x.GetAsync<StoreResponse>(
            It.IsAny<string>(), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}