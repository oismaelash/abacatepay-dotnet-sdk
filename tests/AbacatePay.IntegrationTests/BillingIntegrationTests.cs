using AbacatePay.Models.Billing;
using AbacatePay.Models;
using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for billing operations using real AbacatePay API
/// </summary>
public class BillingIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public BillingIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateBilling_WithValidRequest_ShouldReturnValidResponse()
    {
        // Arrange
        var request = new BillingRequest
        {
            Frequency = BillingFrequency.ONE_TIME,
            Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
            ReturnUrl = "https://example.com/return",
            CompletionUrl = "https://example.com/completion",
            Products = new List<BillingProduct>
            {
                new BillingProduct
                {
                    ExternalId = "produto-teste-001",
                    Name = "Produto Teste",
                    Description = "Descrição do produto teste",
                    Price = 1000, // R$ 10,00
                    Quantity = 1
                }
            },
            Customer = new BillingCustomerData
            {
                Name = "Cliente Teste",
                Email = "cliente@example.com",
                Cellphone = "+5511987654321",
                TaxId = "12345678900"
            }
        };

        // Act
        var result = await _client.CreateBillingAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.NotEmpty(result.Url);
        Assert.NotNull(result.Products);
        Assert.Single(result.Products);
        Assert.Equal("produto-teste-001", result.Products[0].ExternalId);
        Assert.Equal(1, result.Products[0].Quantity);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task CreateBilling_WithExistingCustomer_ShouldReturnValidResponse()
    {
        // Primeiro, criar um cliente
        var customerRequest = new AbacatePay.Models.Customer.CustomerRequest
        {
            Name = "Cliente Existente",
            Email = "existente@example.com",
            Cellphone = "+5511987654321",
            TaxId = "98765432100"
        };
        
        var customer = await _client.CreateCustomerAsync(customerRequest);

        // Arrange
        var request = new BillingRequest
        {
            Frequency = BillingFrequency.ONE_TIME,
            Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
            ReturnUrl = "https://example.com/return",
            CompletionUrl = "https://example.com/completion",
            CustomerId = customer.Id,
            Products = new List<BillingProduct>
            {
                new BillingProduct
                {
                    ExternalId = "produto-cliente-existente-001",
                    Name = "Produto com Cliente Existente",
                    Description = "Descrição do produto",
                    Price = 2000, // R$ 20,00
                    Quantity = 2
                }
            }
        };

        // Act
        var result = await _client.CreateBillingAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.NotNull(result.Customer);
        Assert.Equal(customer.Id, result.Customer.Id);
        Assert.NotNull(result.Products);
        Assert.Single(result.Products);
        Assert.Equal("produto-cliente-existente-001", result.Products[0].ExternalId);
        Assert.Equal(2, result.Products[0].Quantity);
    }

    [Fact]
    public async Task GetBilling_WithValidId_ShouldReturnBillingDetails()
    {
        // Arrange
        var createRequest = new BillingRequest
        {
            Frequency = BillingFrequency.ONE_TIME,
            Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
            ReturnUrl = "https://example.com/return",
            CompletionUrl = "https://example.com/completion",
            Products = new List<BillingProduct>
            {
                new BillingProduct
                {
                    ExternalId = "produto-consulta-001",
                    Name = "Produto para Consulta",
                    Description = "Descrição do produto",
                    Price = 1500, // R$ 15,00
                    Quantity = 1
                }
            },
            Customer = new BillingCustomerData
            {
                Name = "Cliente Consulta",
                Email = "consulta@example.com",
                Cellphone = "+5511987654321",
                TaxId = "11122233344"
            }
        };
        
        var created = await _client.CreateBillingAsync(createRequest);

        // Act
        var result = await _client.GetBillingAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.NotEmpty(result.Url);
        Assert.NotNull(result.Products);
        Assert.Single(result.Products);
        Assert.Equal("produto-consulta-001", result.Products[0].ExternalId);
    }

    [Fact]
    public async Task ListBillings_ShouldReturnListOfBillings()
    {
        // Act
        var result = await _client.ListBillingsAsync();

        // Assert
        Assert.NotNull(result);
        // A lista pode estar vazia, mas não deve ser null
    }

    [Fact]
    public async Task CreateBilling_WithInvalidPrice_ShouldThrowException()
    {
        // Arrange
        var request = new BillingRequest
        {
            Frequency = BillingFrequency.ONE_TIME,
            Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
            ReturnUrl = "https://example.com/return",
            CompletionUrl = "https://example.com/completion",
            Products = new List<BillingProduct>
            {
                new BillingProduct
                {
                    ExternalId = "produto-preco-invalido-001",
                    Name = "Produto com Preço Inválido",
                    Description = "Descrição do produto",
                    Price = 50, // Preço muito baixo (menor que 100 centavos)
                    Quantity = 1
                }
            },
            Customer = new BillingCustomerData
            {
                Name = "Cliente Teste",
                Email = "cliente@example.com",
                Cellphone = "+5511987654321",
                TaxId = "12345678900"
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreateBillingAsync(request));
    }

    [Fact]
    public async Task CreateBilling_WithMissingRequiredFields_ShouldThrowException()
    {
        // Arrange
        var request = new BillingRequest
        {
            Frequency = BillingFrequency.ONE_TIME,
            Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
            // ReturnUrl não definido
            CompletionUrl = "https://example.com/completion",
            Products = new List<BillingProduct>
            {
                new BillingProduct
                {
                    ExternalId = "produto-teste-002",
                    Name = "Produto Teste",
                    Description = "Descrição do produto",
                    Price = 1000,
                    Quantity = 1
                }
            },
            Customer = new BillingCustomerData
            {
                Name = "Cliente Teste",
                Email = "cliente@example.com",
                Cellphone = "+5511987654321",
                TaxId = "12345678900"
            }
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _client.CreateBillingAsync(request));
    }
}
