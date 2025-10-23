using AbacatePay.Models.Webhook;
using AbacatePay.Models;
using Xunit;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Integration tests for webhook operations using real AbacatePay API
/// </summary>
public class WebhookIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly AbacatePayClient _client;
    private readonly IntegrationTestFixture _fixture;

    public WebhookIntegrationTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateWebhook_WithValidRequest_ShouldReturnWebhookResponse()
    {
        // Arrange
        var request = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook",
            Events = new List<WebhookEventType>
            {
                WebhookEventType.PAYMENT_COMPLETED,
                WebhookEventType.PAYMENT_FAILED
            },
            Active = true
        };

        // Act
        var result = await _client.CreateWebhookAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal("https://example.com/webhook", result.Url);
        Assert.True(result.Active);
        Assert.Equal(2, result.Events.Count);
        Assert.Contains(WebhookEventType.PAYMENT_COMPLETED, result.Events);
        Assert.Contains(WebhookEventType.PAYMENT_FAILED, result.Events);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task CreateWebhook_WithAllEvents_ShouldReturnWebhookResponse()
    {
        // Arrange
        var request = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook-all-events",
            Events = new List<WebhookEventType>
            {
                WebhookEventType.PAYMENT_CREATED,
                WebhookEventType.PAYMENT_UPDATED,
                WebhookEventType.PAYMENT_COMPLETED,
                WebhookEventType.PAYMENT_FAILED,
                WebhookEventType.PAYMENT_CANCELLED,
                WebhookEventType.PAYMENT_EXPIRED,
                WebhookEventType.REFUND_CREATED,
                WebhookEventType.REFUND_COMPLETED,
                WebhookEventType.REFUND_FAILED
            },
            Active = true
        };

        // Act
        var result = await _client.CreateWebhookAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Id);
        Assert.Equal("https://example.com/webhook-all-events", result.Url);
        Assert.True(result.Active);
        Assert.Equal(9, result.Events.Count);
        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-5));
    }

    [Fact]
    public async Task GetWebhook_WithValidId_ShouldReturnWebhookDetails()
    {
        // Arrange - primeiro criar um webhook
        var createRequest = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook-get-test",
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED },
            Active = true
        };
        var createdWebhook = await _client.CreateWebhookAsync(createRequest);

        // Act
        var result = await _client.GetWebhookAsync(createdWebhook.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdWebhook.Id, result.Id);
        Assert.Equal("https://example.com/webhook-get-test", result.Url);
        Assert.True(result.Active);
        Assert.Single(result.Events);
        Assert.Contains(WebhookEventType.PAYMENT_COMPLETED, result.Events);
    }

    [Fact]
    public async Task UpdateWebhook_WithValidData_ShouldReturnUpdatedWebhook()
    {
        // Arrange - criar webhook inicial
        var createRequest = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook-update-test",
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED },
            Active = true
        };
        var createdWebhook = await _client.CreateWebhookAsync(createRequest);

        // Arrange - dados para atualização
        var updateRequest = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook-updated",
            Events = new List<WebhookEventType>
            {
                WebhookEventType.PAYMENT_COMPLETED,
                WebhookEventType.PAYMENT_FAILED
            },
            Active = false
        };

        // Act
        var result = await _client.UpdateWebhookAsync(createdWebhook.Id, updateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdWebhook.Id, result.Id);
        Assert.Equal("https://example.com/webhook-updated", result.Url);
        Assert.False(result.Active);
        Assert.Equal(2, result.Events.Count);
        Assert.Contains(WebhookEventType.PAYMENT_COMPLETED, result.Events);
        Assert.Contains(WebhookEventType.PAYMENT_FAILED, result.Events);
    }

    [Fact]
    public async Task ListWebhooks_ShouldReturnListOfWebhooks()
    {
        // Arrange - criar alguns webhooks
        var requests = new[]
        {
            new WebhookConfigRequest
            {
                Url = "https://example.com/webhook-list-1",
                Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED },
                Active = true
            },
            new WebhookConfigRequest
            {
                Url = "https://example.com/webhook-list-2",
                Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_FAILED },
                Active = true
            }
        };

        foreach (var request in requests)
        {
            await _client.CreateWebhookAsync(request);
        }

        // Act
        var result = await _client.ListWebhooksAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
        
        // Verificar se os webhooks criados estão na lista
        var createdWebhooks = result.Where(w => 
            w.Url == "https://example.com/webhook-list-1" || 
            w.Url == "https://example.com/webhook-list-2").ToList();
        Assert.True(createdWebhooks.Count >= 2);
    }

    [Fact]
    public async Task DeleteWebhook_WithValidId_ShouldDeleteWebhook()
    {
        // Arrange - criar webhook para deletar
        var createRequest = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook-to-delete",
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED },
            Active = true
        };
        var createdWebhook = await _client.CreateWebhookAsync(createRequest);

        // Act
        await _client.DeleteWebhookAsync(createdWebhook.Id);

        // Assert - tentar buscar o webhook deletado deve falhar
        await Assert.ThrowsAsync<AbacatePayException>(() => 
            _client.GetWebhookAsync(createdWebhook.Id));
    }

    [Fact]
    public async Task CreateWebhook_WithInvalidUrl_ShouldThrowException()
    {
        // Arrange
        var request = new WebhookConfigRequest
        {
            Url = "invalid-url", // URL inválida
            Events = new List<WebhookEventType> { WebhookEventType.PAYMENT_COMPLETED },
            Active = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.CreateWebhookAsync(request));
    }

    [Fact]
    public async Task CreateWebhook_WithEmptyEvents_ShouldThrowException()
    {
        // Arrange
        var request = new WebhookConfigRequest
        {
            Url = "https://example.com/webhook-no-events",
            Events = new List<WebhookEventType>(), // Lista vazia
            Active = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _client.CreateWebhookAsync(request));
    }

    [Fact]
    public void VerifyWebhookSignature_WithValidSignature_ShouldReturnTrue()
    {
        // Arrange
        var payload = "test payload";
        var secret = "test-secret-key";
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
        var secret = "test-secret-key";
        var invalidSignature = "invalid-signature";

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
    public void VerifyWebhookSignature_WithNullOrEmptyParameters_ShouldReturnFalse(
        string payload, string signature, string secret)
    {
        // Act
        var result = AbacatePayClient.VerifyWebhookSignature(payload, signature, secret);

        // Assert
        Assert.False(result);
    }

    private static string ComputeHmacSha256(string payload, string secret)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secret));
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
        return BitConverter.ToString(computedHash).Replace("-", "").ToLower();
    }
}
