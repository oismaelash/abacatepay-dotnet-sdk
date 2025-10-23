using AbacatePay.Models.Payment;
using AbacatePay.Models;
using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for payment operations using real AbacatePay API
/// </summary>
public class PaymentIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public PaymentIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreatePayment_WithValidRequest_ShouldReturnValidResponse()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 1000, // R$ 10,00
            Description = "Teste de integração - Pagamento PIX",
            PaymentMethod = PaymentMethod.PIX,
            ExpiresIn = 3600 // 1 hora
        };

        // Act
        var result = await _client.CreatePaymentAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(1000, result.Amount);
        Assert.Equal("Teste de integração - Pagamento PIX", result.Description);
        Assert.Equal(PaymentMethod.PIX, result.PaymentMethod);
        Assert.Equal(PaymentStatus.PENDING, result.Status);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
        Assert.NotNull(result.ExpiresAt);
        Assert.True(result.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task CreatePayment_WithCustomerInfo_ShouldIncludeCustomerData()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 2000, // R$ 20,00
            Description = "Teste com dados do cliente",
            PaymentMethod = PaymentMethod.PIX,
            Customer = new CustomerInfo
            {
                Name = "João Silva",
                Email = "joao@example.com",
                Phone = "+5511999999999",
                Document = "12345678901"
            }
        };

        // Act
        var result = await _client.CreatePaymentAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(2000, result.Amount);
        Assert.NotNull(result.Customer);
        Assert.Equal("João Silva", result.Customer.Name);
        Assert.Equal("joao@example.com", result.Customer.Email);
    }

    [Fact]
    public async Task CreatePayment_WithMetadata_ShouldIncludeMetadata()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 1500, // R$ 15,00
            Description = "Teste com metadados",
            PaymentMethod = PaymentMethod.PIX,
            Metadata = new Dictionary<string, object>
            {
                { "order_id", "12345" },
                { "source", "integration_test" },
                { "test_type", "metadata_test" }
            }
        };

        // Act
        var result = await _client.CreatePaymentAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal(1500, result.Amount);
        Assert.NotNull(result.Metadata);
        Assert.Equal("12345", result.Metadata["order_id"]);
        Assert.Equal("integration_test", result.Metadata["source"]);
        Assert.Equal("metadata_test", result.Metadata["test_type"]);
    }

    [Fact]
    public async Task GetPayment_WithValidId_ShouldReturnPaymentDetails()
    {
        // Arrange - primeiro criar um pagamento
        var createRequest = new PaymentRequest
        {
            Amount = 3000, // R$ 30,00
            Description = "Teste de busca de pagamento",
            PaymentMethod = PaymentMethod.PIX
        };
        var createdPayment = await _client.CreatePaymentAsync(createRequest);

        // Act
        var result = await _client.GetPaymentAsync(createdPayment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdPayment.Id, result.Id);
        Assert.Equal(3000, result.Amount);
        Assert.Equal("Teste de busca de pagamento", result.Description);
        Assert.Equal(PaymentMethod.PIX, result.PaymentMethod);
        Assert.Equal(PaymentStatus.PENDING, result.Status);
    }

    [Fact]
    public async Task GetPayment_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var invalidId = "invalid_payment_id_12345";

        // Act & Assert
        await Assert.ThrowsAsync<AbacatePayException>(() => 
            _client.GetPaymentAsync(invalidId));
    }

    [Fact]
    public async Task ListPayments_ShouldReturnListOfPayments()
    {
        // Arrange - criar alguns pagamentos primeiro
        var requests = new[]
        {
            new PaymentRequest
            {
                Amount = 1000,
                Description = "Pagamento 1",
                PaymentMethod = PaymentMethod.PIX
            },
            new PaymentRequest
            {
                Amount = 2000,
                Description = "Pagamento 2",
                PaymentMethod = PaymentMethod.PIX
            }
        };

        foreach (var request in requests)
        {
            await _client.CreatePaymentAsync(request);
        }

        // Act
        var result = await _client.ListPaymentsAsync(limit: 10);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
        
        // Verificar se os pagamentos criados estão na lista
        var createdPayments = result.Where(p => 
            p.Description == "Pagamento 1" || p.Description == "Pagamento 2").ToList();
        Assert.True(createdPayments.Count >= 2);
    }

    [Fact]
    public async Task ListPayments_WithFilters_ShouldReturnFilteredResults()
    {
        // Arrange - criar pagamento com status específico
        var request = new PaymentRequest
        {
            Amount = 5000, // R$ 50,00
            Description = "Pagamento para filtro",
            PaymentMethod = PaymentMethod.PIX
        };
        var createdPayment = await _client.CreatePaymentAsync(request);

        // Act - buscar apenas pagamentos pendentes
        var result = await _client.ListPaymentsAsync(
            limit: 20, 
            status: PaymentStatus.PENDING);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count > 0);
        
        // Todos os pagamentos retornados devem estar pendentes
        foreach (var payment in result)
        {
            Assert.Equal(PaymentStatus.PENDING, payment.Status);
        }
    }

    [Fact]
    public async Task CancelPayment_WithValidId_ShouldCancelPayment()
    {
        // Arrange - criar um pagamento
        var request = new PaymentRequest
        {
            Amount = 4000, // R$ 40,00
            Description = "Pagamento para cancelamento",
            PaymentMethod = PaymentMethod.PIX
        };
        var createdPayment = await _client.CreatePaymentAsync(request);

        // Act
        var result = await _client.CancelPaymentAsync(createdPayment.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdPayment.Id, result.Id);
        Assert.Equal(PaymentStatus.CANCELLED, result.Status);
    }

    [Fact]
    public async Task CancelPayment_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var invalidId = "invalid_payment_id_12345";

        // Act & Assert
        await Assert.ThrowsAsync<AbacatePayException>(() => 
            _client.CancelPaymentAsync(invalidId));
    }

    [Fact]
    public async Task CreatePayment_WithInvalidAmount_ShouldThrowException()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 0, // Valor inválido
            Description = "Pagamento inválido",
            PaymentMethod = PaymentMethod.PIX
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.CreatePaymentAsync(request));
    }

    [Fact]
    public async Task CreatePayment_WithMissingRequiredFields_ShouldThrowException()
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = 1000,
            Description = "", // Descrição vazia
            PaymentMethod = PaymentMethod.PIX
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.CreatePaymentAsync(request));
    }
}
