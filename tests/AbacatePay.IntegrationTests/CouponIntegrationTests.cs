using AbacatePay.Models.Coupon;
using AbacatePay.Models;
using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for coupon operations using real AbacatePay API
/// </summary>
public class CouponIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public CouponIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateCoupon_WithValidRequest_ShouldReturnValidResponse()
    {
        // Arrange
        var request = new CouponRequest
        {
            Data = new CouponData
            {
                Code = $"TEST{DateTime.UtcNow.Ticks}",
                Notes = "Cupom de teste para integração",
                MaxRedeems = 10,
                DiscountKind = DiscountKind.PERCENTAGE,
                Discount = 20, // 20% de desconto
                Metadata = new Dictionary<string, object> { { "test", "integration" } }
            }
        };

        // Act
        var result = await _client.CreateCouponAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal("Cupom de teste para integração", result.Notes);
        Assert.Equal(10, result.MaxRedeems);
        Assert.Equal(DiscountKind.PERCENTAGE, result.DiscountKind);
        Assert.Equal(20, result.Discount);
        Assert.Equal("ACTIVE", result.Status);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task CreateCoupon_WithUnlimitedRedeems_ShouldReturnValidResponse()
    {
        // Arrange
        var request = new CouponRequest
        {
            Data = new CouponData
            {
                Code = $"UNLIMITED{DateTime.UtcNow.Ticks}",
                Notes = "Cupom ilimitado para teste",
                MaxRedeems = -1, // Ilimitado
                DiscountKind = DiscountKind.FIXED,
                Discount = 500, // R$ 5,00 de desconto fixo
                Metadata = new Dictionary<string, object> { { "type", "unlimited" } }
            }
        };

        // Act
        var result = await _client.CreateCouponAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal("Cupom ilimitado para teste", result.Notes);
        Assert.Equal(-1, result.MaxRedeems);
        Assert.Equal(DiscountKind.FIXED, result.DiscountKind);
        Assert.Equal(500, result.Discount);
        Assert.Equal("ACTIVE", result.Status);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task ListCoupons_ShouldReturnListOfCoupons()
    {
        // Act
        var result = await _client.ListCouponsAsync();

        // Assert
        Assert.NotNull(result);
        // A lista pode estar vazia, mas não deve ser null
    }

    [Fact]
    public async Task CreateCoupon_WithInvalidMaxRedeems_ShouldThrowException()
    {
        // Arrange
        var request = new CouponRequest
        {
            Data = new CouponData
            {
                Code = $"INVALID{DateTime.UtcNow.Ticks}",
                Notes = "Cupom com MaxRedeems inválido",
                DiscountKind = DiscountKind.PERCENTAGE,
                Discount = 1000,
                MaxRedeems = -2 // Valor inválido (deve ser -1 ou >= 0)
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreateCouponAsync(request));
    }

    [Fact]
    public async Task CreateCoupon_WithMissingRequiredFields_ShouldThrowException()
    {
        // Arrange
        var request = new CouponRequest
        {
            Data = new CouponData
            {
                // Code não definido
                Notes = "Cupom sem código",
                DiscountKind = DiscountKind.PERCENTAGE,
                Discount = 1000,
                MaxRedeems = 10
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreateCouponAsync(request));
    }
}
