using AbacatePay.Models.Payment;
using Xunit;

namespace AbacatePay.Tests.Models.Payment;

public class PaymentRequestTests
{
    [Fact]
    public void DefaultValues_ShouldBeSetCorrectly()
    {
        // Act
        var request = new PaymentRequest();

        // Assert
        Assert.Equal(0, request.Amount);
        Assert.Equal("BRL", request.Currency);
        Assert.Equal(string.Empty, request.Description);
        Assert.Equal(3600, request.ExpiresIn);
        Assert.Null(request.Customer);
        Assert.Null(request.Metadata);
        Assert.Null(request.WebhookUrl);
        Assert.Null(request.RedirectUrl);
        Assert.Null(request.PaymentOptions);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var request = new PaymentRequest();
        var customer = new CustomerInfo
        {
            Name = "John Doe",
            Email = "john@example.com"
        };
        var metadata = new Dictionary<string, object>
        {
            { "order_id", "12345" },
            { "source", "website" }
        };

        // Act
        request.Amount = 1000;
        request.Currency = "USD";
        request.Description = "Test payment";
        request.PaymentMethod = PaymentMethod.PIX;
        request.Customer = customer;
        request.Metadata = metadata;
        request.ExpiresIn = 7200;
        request.WebhookUrl = "https://example.com/webhook";
        request.RedirectUrl = "https://example.com/redirect";

        // Assert
        Assert.Equal(1000, request.Amount);
        Assert.Equal("USD", request.Currency);
        Assert.Equal("Test payment", request.Description);
        Assert.Equal(PaymentMethod.PIX, request.PaymentMethod);
        Assert.Equal(customer, request.Customer);
        Assert.Equal(metadata, request.Metadata);
        Assert.Equal(7200, request.ExpiresIn);
        Assert.Equal("https://example.com/webhook", request.WebhookUrl);
        Assert.Equal("https://example.com/redirect", request.RedirectUrl);
    }
}
