using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for store operations using real AbacatePay API
/// </summary>
public class StoreIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public StoreIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task GetStore_ShouldReturnStoreInformation()
    {
        // Act
        var result = await _client.GetStoreAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.NotEmpty(result.Name);
        Assert.True(result.CreatedAt > DateTime.MinValue);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddYears(-10)); // Store deve ter sido criado nos últimos 10 anos
    }

    [Fact]
    public async Task GetStore_ShouldReturnValidStoreData()
    {
        // Act
        var result = await _client.GetStoreAsync();

        // Assert
        Assert.NotNull(result);
        
        // Verificar se os campos obrigatórios estão preenchidos
        Assert.NotEmpty(result.Id);
        Assert.NotEmpty(result.Name);
        
        // Verificar se as datas são válidas
        Assert.True(result.CreatedAt > DateTime.MinValue);
        Assert.True(result.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task GetStore_MultipleCalls_ShouldReturnConsistentData()
    {
        // Act
        var result1 = await _client.GetStoreAsync();
        var result2 = await _client.GetStoreAsync();

        // Assert
        Assert.NotNull(result1);
        Assert.NotNull(result2);
        
        // Os dados devem ser consistentes entre as chamadas
        Assert.Equal(result1.Id, result2.Id);
        Assert.Equal(result1.Name, result2.Name);
        Assert.Equal(result1.CreatedAt, result2.CreatedAt);
    }
}
