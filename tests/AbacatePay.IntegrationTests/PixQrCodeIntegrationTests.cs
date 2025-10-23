using AbacatePay.Models.PixQrCode;
using AbacatePay.Models;
using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for PIX QRCode operations using real AbacatePay API
/// </summary>
public class PixQrCodeIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public PixQrCodeIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreatePixQrCode_WithValidRequest_ShouldReturnValidResponse()
    {
        // Arrange
        var request = new PixQrCodeRequest
        {
            Amount = 1000, // R$ 10,00
            Description = "Teste de integração - PIX QRCode",
            ExpiresIn = 3600 // 1 hora
        };

        // Act
        var result = await _client.CreatePixQrCodeAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(1000, result.Amount);
        Assert.NotNull(result.BrCode);
        Assert.NotNull(result.BrCodeBase64);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
        Assert.NotNull(result.ExpiresAt);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task CheckPixQrCodeStatus_WithValidId_ShouldReturnStatus()
    {
        // Arrange
        var createRequest = new PixQrCodeRequest
        {
            Amount = 1000,
            Description = "Teste de status",
            ExpiresIn = 3600
        };
        
        var created = await _client.CreatePixQrCodeAsync(createRequest);

        // Act
        var result = await _client.CheckPixQrCodeStatusAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Status);
        Assert.NotNull(result.ExpiresAt);
    }

    [Fact]
    public async Task SimulatePixQrCodePayment_WithValidId_ShouldReturnResponse()
    {
        // Arrange
        var createRequest = new PixQrCodeRequest
        {
            Amount = 1000,
            Description = "Teste de simulação",
            ExpiresIn = 3600
        };
        
        var created = await _client.CreatePixQrCodeAsync(createRequest);
        
        var simulateRequest = new PixQrCodeSimulateRequest
        {
            Metadata = new Dictionary<string, object> { { "test", "simulation" } }
        };

        // Act
        var result = await _client.SimulatePixQrCodePaymentAsync(created.Id, simulateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(1000, result.Amount);
        Assert.NotNull(result.BrCode);
        Assert.NotNull(result.BrCodeBase64);
    }

    [Fact]
    public async Task CreatePixQrCode_WithInvalidAmount_ShouldThrowException()
    {
        // Arrange
        var request = new PixQrCodeRequest
        {
            Amount = 50, // Valor muito baixo (menor que 100 centavos)
            Description = "Teste com valor inválido",
            ExpiresIn = 3600
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreatePixQrCodeAsync(request));
    }

    [Fact]
    public async Task CreatePixQrCode_WithMissingRequiredFields_ShouldThrowException()
    {
        // Arrange
        var request = new PixQrCodeRequest
        {
            // Amount não definido
            Description = "Teste sem amount",
            ExpiresIn = 3600
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreatePixQrCodeAsync(request));
    }
}
