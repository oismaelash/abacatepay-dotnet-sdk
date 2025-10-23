# AbacatePay .NET SDK

[![NuGet version](https://img.shields.io/nuget/v/AbacatePay.svg)](https://www.nuget.org/packages/AbacatePay/)
[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

A comprehensive .NET SDK for integrating with the AbacatePay API - Brazil's leading payment solution supporting PIX.

## Features

- 🚀 **Complete API Coverage** - All AbacatePay endpoints implemented
- 💳 **PIX Payments** - Instant PIX payments with QR codes
- 🔒 **Secure Authentication** - Bearer token authentication
- 📦 **Easy Integration** - Simple, intuitive API design
- 🛡️ **Type Safety** - Full C# type definitions and validation
- 🔄 **Async/Await Support** - Modern async programming patterns
- 📊 **Webhook Support** - Built-in webhook handling and verification
- 👥 **Customer Management** - Create and manage customers
- 🎫 **Coupon System** - Create and manage discount coupons
- 📋 **Billing System** - Create comprehensive billing with products
- 📱 **PIX QRCode** - Generate and manage PIX QR codes
- 💰 **Withdraw System** - Create and manage withdrawals
- 🏪 **Store Management** - Get store information
- 🏗️ **Extensible** - Easy to extend and customize

## Installation

### NuGet Package Manager
```bash
Install-Package AbacatePay
```

### .NET CLI
```bash
dotnet add package AbacatePay
```

### PackageReference
```xml
<PackageReference Include="AbacatePay" Version="1.0.0" />
```

## Quick Start

### 1. Initialize the Client

```csharp
using AbacatePay;

// Using constructor with credentials
var client = new AbacatePayClient("your_bearer_token", sandbox: true);

// Or using configuration object
var config = new AbacatePayConfig
{
    ApiKey = "your_bearer_token",
    Sandbox = true // Set to false for production
};
var client = new AbacatePayClient(config);
```

### 2. Create a Payment

```csharp
// Create a PIX payment
var paymentRequest = new PaymentRequest
{
    Amount = 10000, // R$ 100.00 in cents
    Currency = "BRL",
    Description = "Payment for Order #123",
    PaymentMethod = PaymentMethod.PIX,
    Customer = new CustomerInfo
    {
        Name = "João Silva",
        Email = "joao@example.com",
        Document = "12345678901"
    },
    ExpiresIn = 3600, // 1 hour
    WebhookUrl = "https://yourapp.com/webhook"
};

var payment = await client.CreatePaymentAsync(paymentRequest);
Console.WriteLine($"Payment ID: {payment.Id}");
Console.WriteLine($"PIX QR Code: {payment.PaymentData?.Pix?.QrCode}");
```



### 5. Check Payment Status

```csharp
var payment = await client.GetPaymentAsync("payment_id_here");
Console.WriteLine($"Payment Status: {payment.Status}");
Console.WriteLine($"Amount: R$ {payment.Amount / 100.0:F2}");
```

### 6. Create a Refund

```csharp
var refundRequest = new RefundRequest
{
    PaymentId = "payment_id_here",
    Amount = 5000, // R$ 50.00 in cents (partial refund)
    Reason = "Customer requested refund",
    NotifyCustomer = true
};

var refund = await client.CreateRefundAsync(refundRequest);
Console.WriteLine($"Refund ID: {refund.Id}");
Console.WriteLine($"Refund Status: {refund.Status}");
```

### 7. Handle Webhooks

```csharp
// Configure webhook
var webhookConfig = new WebhookConfigRequest
{
    Url = "https://yourapp.com/webhook",
    Events = new List<WebhookEventType>
    {
        WebhookEventType.PAYMENT_COMPLETED,
        WebhookEventType.PAYMENT_FAILED,
        WebhookEventType.REFUND_COMPLETED
    },
    Active = true,
    Secret = "your_webhook_secret"
};

var webhook = await client.CreateWebhookAsync(webhookConfig);

// Verify webhook signature in your endpoint
public bool VerifyWebhook(string payload, string signature, string secret)
{
    return AbacatePayClient.VerifyWebhookSignature(payload, signature, secret);
}
```

### 8. Create a Customer

```csharp
var customerRequest = new CustomerRequest
{
    Name = "João Silva",
    Cellphone = "(11) 99999-9999",
    Email = "joao@example.com",
    TaxId = "123.456.789-01"
};

var customer = await client.CreateCustomerAsync(customerRequest);
Console.WriteLine($"Customer created: {customer.Id}");
```

### 9. Create a Coupon

```csharp
var couponRequest = new CouponRequest
{
    Data = new CouponData
    {
        Code = "DISCOUNT20",
        Notes = "20% discount for new customers",
        MaxRedeems = 100,
        DiscountKind = DiscountKind.PERCENTAGE,
        Discount = 20
    }
};

var coupon = await client.CreateCouponAsync(couponRequest);
Console.WriteLine($"Coupon created: {coupon.Id}");
```

### 10. Create a Billing

```csharp
var billingRequest = new BillingRequest
{
    Frequency = BillingFrequency.ONE_TIME,
    Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
    Products = new List<BillingProduct>
    {
        new BillingProduct
        {
            ExternalId = "prod-123",
            Name = "Premium Plan",
            Description = "Monthly premium subscription",
            Quantity = 1,
            Price = 5000 // R$ 50.00
        }
    },
    ReturnUrl = "https://yourapp.com/return",
    CompletionUrl = "https://yourapp.com/success",
    CustomerId = "cust_123456"
};

var billing = await client.CreateBillingAsync(billingRequest);
Console.WriteLine($"Billing created: {billing.Url}");
```

### 11. Create a PIX QRCode

```csharp
var pixRequest = new PixQrCodeRequest
{
    Amount = 10000, // R$ 100.00
    ExpiresIn = 3600, // 1 hour
    Description = "Payment for service",
    Customer = new PixQrCodeCustomer
    {
        Name = "Maria Santos",
        Cellphone = "(11) 88888-8888",
        Email = "maria@example.com",
        TaxId = "987.654.321-00"
    }
};

var pixQrCode = await client.CreatePixQrCodeAsync(pixRequest);
Console.WriteLine($"PIX QRCode: {pixQrCode.BrCode}");
Console.WriteLine($"QR Code Image: {pixQrCode.BrCodeBase64}");
```

### 12. Create a Withdraw

```csharp
var withdrawRequest = new WithdrawRequest
{
    Amount = 50000, // R$ 500.00
    PixKey = "user@email.com",
    Notes = "Monthly earnings withdrawal"
};

var withdraw = await client.CreateWithdrawAsync(withdrawRequest);
Console.WriteLine($"Withdraw created: {withdraw.Id}");
```

### 13. Get Store Information

```csharp
var store = await client.GetStoreAsync();
Console.WriteLine($"Store: {store.Name} (ID: {store.Id})");
```

## Payment Methods

### PIX
Instant payment method available 24/7. Supports QR codes and copy-paste codes.

```csharp
var pixPayment = new PaymentRequest
{
    PaymentMethod = PaymentMethod.PIX,
    PaymentOptions = new PaymentOptions
    {
        Pix = new PixOptions
        {
            PixKey = "12345678901", // CPF, email, phone, or random key
            PixKeyType = PixKeyType.CPF
        }
    }
};
```



## Error Handling

The SDK provides comprehensive error handling with detailed error information:

```csharp
try
{
    var payment = await client.CreatePaymentAsync(paymentRequest);
}
catch (AbacatePayException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation Error: {ex.Message}");
}
```

## Configuration Options

```csharp
var config = new AbacatePayConfig
{
    ApiKey = "your_bearer_token",
    BaseUrl = "https://api.abacatepay.com", // Production URL
    TimeoutSeconds = 30,
    Sandbox = false
};
```

## Advanced Usage

### List Payments with Filters

```csharp
var payments = await client.ListPaymentsAsync(
    limit: 50,
    offset: 0,
    status: PaymentStatus.COMPLETED,
    createdAfter: DateTime.Now.AddDays(-30)
);

foreach (var payment in payments)
{
    Console.WriteLine($"Payment {payment.Id}: {payment.Status} - R$ {payment.Amount / 100.0:F2}");
}
```

### Cancel Payment

```csharp
var cancelledPayment = await client.CancelPaymentAsync("payment_id_here");
Console.WriteLine($"Payment cancelled: {cancelledPayment.Status}");
```

### List Refunds

```csharp
var refunds = await client.ListRefundsAsync(
    limit: 20,
    status: RefundStatus.COMPLETED,
    paymentId: "payment_id_here"
);
```

## Requirements

- .NET 6.0 or later
- Internet connection for API calls

## Dependencies

- Newtonsoft.Json (13.0.3)
- System.ComponentModel.Annotations (5.0.0)

## License

This SDK is licensed under the MIT License.

## Support

For support and questions:
- Email: dev@abacatepay.com
- Documentation: [https://docs.abacatepay.com](https://docs.abacatepay.com)
- GitHub Issues: [https://github.com/abacatepay/dotnet-sdk/issues](https://github.com/abacatepay/dotnet-sdk/issues)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
