using AbacatePay.Models.Customer;
using AbacatePay.Models;
using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for customer operations using real AbacatePay API
/// </summary>
public class CustomerIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public CustomerIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateCustomer_WithValidData_ShouldReturnCustomerResponse()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "Maria Silva",
            Email = "maria@example.com",
            Cellphone = "+5511987654321",
            TaxId = "98765432100"
        };

        // Act
        var result = await _client.CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.NotNull(result.Metadata);
        Assert.Equal("Maria Silva", result.Metadata.Name);
        Assert.Equal("maria@example.com", result.Metadata.Email);
        Assert.Equal("+5511987654321", result.Metadata.Cellphone);
        Assert.Equal("98765432100", result.Metadata.TaxId);
    }

    [Fact]
    public async Task CreateCustomer_WithMinimalData_ShouldReturnCustomerResponse()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "João Santos",
            Email = "joao.santos@example.com",
            Cellphone = "+5511123456789",
            TaxId = "12345678901"
        };

        // Act
        var result = await _client.CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.NotNull(result.Metadata);
        Assert.Equal("João Santos", result.Metadata.Name);
        Assert.Equal("joao.santos@example.com", result.Metadata.Email);
    }

    [Fact]
    public async Task ListCustomers_ShouldReturnListOfCustomers()
    {
        // Arrange - criar alguns clientes primeiro
        var requests = new[]
        {
            new CustomerRequest
            {
                Name = "Cliente Teste 1",
                Email = "cliente1@example.com",
                Cellphone = "+5511111111111",
                TaxId = "11111111111"
            },
            new CustomerRequest
            {
                Name = "Cliente Teste 2",
                Email = "cliente2@example.com",
                Cellphone = "+5511222222222",
                TaxId = "22222222222"
            }
        };

        foreach (var request in requests)
        {
            await _client.CreateCustomerAsync(request);
        }

        // Act
        var result = await _client.ListCustomersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
        
        // Verificar se os clientes criados estão na lista
        var createdCustomers = result.Where(c => 
            c.Metadata?.Name == "Cliente Teste 1" || c.Metadata?.Name == "Cliente Teste 2").ToList();
        Assert.True(createdCustomers.Count >= 2);
    }

    [Fact]
    public async Task CreateCustomer_WithInvalidEmail_ShouldThrowException()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "Cliente Inválido",
            Email = "email-invalido", // Email inválido
            Cellphone = "+5511999999999",
            TaxId = "12345678901"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.CreateCustomerAsync(request));
    }

    [Fact]
    public async Task CreateCustomer_WithMissingRequiredFields_ShouldThrowException()
    {
        // Arrange
        var request = new CustomerRequest
        {
            Name = "Cliente Incompleto",
            Email = "incompleto@example.com",
            Cellphone = "", // Campo obrigatório vazio
            TaxId = "12345678901"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.CreateCustomerAsync(request));
    }

    [Fact]
    public async Task CreateCustomer_WithDuplicateTaxId_ShouldHandleGracefully()
    {
        // Arrange - criar primeiro cliente
        var firstRequest = new CustomerRequest
        {
            Name = "Primeiro Cliente",
            Email = "primeiro@example.com",
            Cellphone = "+5511333333333",
            TaxId = "33333333333"
        };
        await _client.CreateCustomerAsync(firstRequest);

        // Arrange - tentar criar segundo cliente com mesmo CPF
        var secondRequest = new CustomerRequest
        {
            Name = "Segundo Cliente",
            Email = "segundo@example.com",
            Cellphone = "+5511444444444",
            TaxId = "33333333333" // Mesmo CPF
        };

        // Act & Assert - pode lançar exceção ou retornar cliente existente
        // dependendo da implementação da API
        try
        {
            var result = await _client.CreateCustomerAsync(secondRequest);
            Assert.NotNull(result);
        }
        catch (AbacatePayException ex)
        {
            // Esperado se a API não permite CPFs duplicados
            Assert.Contains("duplicate", ex.Message.ToLower());
        }
    }
}
