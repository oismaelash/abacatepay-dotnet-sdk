using AbacatePay.Models.Payment;
using Xunit;

namespace AbacatePay.Tests.Models.Payment;

public class PaymentResponseTests
{
    [Fact]
    public void DefaultValues_ShouldBeSetCorrectly()
    {
        // Act
        var response = new PaymentResponse();

        // Assert
        Assert.Equal(string.Empty, response.Id);
        Assert.Equal(0, response.Amount);
        Assert.Equal(string.Empty, response.Currency);
        Assert.Equal(string.Empty, response.Description);
        Assert.Equal(default(DateTime), response.CreatedAt);
        Assert.Null(response.ExpiresAt);
        Assert.Null(response.PaidAt);
        Assert.Null(response.Metadata);
        Assert.Null(response.PaymentData);
        Assert.Null(response.Customer);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var response = new PaymentResponse();
        var createdAt = DateTime.UtcNow;
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var paidAt = DateTime.UtcNow.AddMinutes(30);
        var metadata = new Dictionary<string, object>
        {
            { "order_id", "12345" }
        };
        var paymentData = new PaymentData
        {
            Pix = new PixData
            {
                QrCode = "test-qr-code",
                CopyPasteCode = "test-copy-paste-code"
            }
        };
        var customer = new CustomerInfo
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Act
        response.Id = "payment_123";
        response.Status = PaymentStatus.COMPLETED;
        response.Amount = 1000;
        response.Currency = "BRL";
        response.Description = "Test payment";
        response.PaymentMethod = PaymentMethod.PIX;
        response.CreatedAt = createdAt;
        response.ExpiresAt = expiresAt;
        response.PaidAt = paidAt;
        response.Metadata = metadata;
        response.PaymentData = paymentData;
        response.Customer = customer;

        // Assert
        Assert.Equal("payment_123", response.Id);
        Assert.Equal(PaymentStatus.COMPLETED, response.Status);
        Assert.Equal(1000, response.Amount);
        Assert.Equal("BRL", response.Currency);
        Assert.Equal("Test payment", response.Description);
        Assert.Equal(PaymentMethod.PIX, response.PaymentMethod);
        Assert.Equal(createdAt, response.CreatedAt);
        Assert.Equal(expiresAt, response.ExpiresAt);
        Assert.Equal(paidAt, response.PaidAt);
        Assert.Equal(metadata, response.Metadata);
        Assert.Equal(paymentData, response.PaymentData);
        Assert.Equal(customer, response.Customer);
    }
}
