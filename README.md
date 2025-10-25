# AbacatePay .NET SDK

[![NuGet Version](https://img.shields.io/nuget/v/AbacatePay.SDK.svg)](https://www.nuget.org/packages/AbacatePay.SDK)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.1-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

A comprehensive .NET SDK for the AbacatePay API - a Brazilian payment solution that supports PIX payments, billing management, customer handling, and more.

## Features

- 🚀 **Easy Integration**: Simple and intuitive API design
- 🏗️ **Modular Architecture**: Domain-specific services for better organization
- 💳 **PIX Payments**: Create and manage PIX QR codes for instant payments
- 👥 **Customer Management**: Create and manage customer profiles
- 📄 **Billing System**: Handle recurring and one-time billings
- 🎫 **Coupon System**: Create and manage discount coupons
- 🏪 **Store Management**: Access store information and balance
- 💰 **Withdrawals**: Manage fund withdrawals
- 🔒 **Type Safety**: Full .NET type safety with comprehensive models
- ✅ **Validation**: Built-in request validation
- 🌐 **Async/Await**: Modern async programming support
- 🔧 **Dependency Injection**: Full support for DI containers
- 🧪 **Testability**: Easy mocking and unit testing
- 📦 **NuGet Package**: Easy installation via NuGet

## Installation

### Package Manager
```bash
Install-Package AbacatePay.SDK
```

### .NET CLI
```bash
dotnet add package AbacatePay.SDK
```

### PackageReference
```xml
<PackageReference Include="AbacatePay.SDK" Version="1.0.0" />
```

## Quick Start

### 1. Initialize the Client

```csharp
using AbacatePay;

// Simple initialization with API key
var client = new AbacatePayClient("your-api-key-here");

// Or with full configuration
var config = new AbacatePayConfig
{
    ApiKey = "your-api-key-here",
    BaseUrl = "https://api.abacatepay.com",
    Sandbox = false, // Set to true for testing
    TimeoutSeconds = 30
};
var client = new AbacatePayClient(config);
```

### 2. Create a Customer

```csharp
var customerRequest = new CustomerRequest
{
    Name = "João Silva",
    Email = "joao@example.com",
    Cellphone = "+5511999999999",
    TaxId = "12345678901" // CPF or CNPJ
};

// Traditional way (still supported)
var customer = await client.CreateCustomerAsync(customerRequest);

// New modular way (recommended)
var customer = await client.Customers.CreateCustomerAsync(customerRequest);

Console.WriteLine($"Customer created with ID: {customer.Id}");
```

### 3. Create a PIX QR Code

```csharp
var pixRequest = new PixQrCodeRequest
{
    Amount = 10000, // R$ 100.00 in cents
    Description = "Payment for services",
    ExpiresIn = 3600, // 1 hour
    Customer = new PixQrCodeCustomer
    {
        Name = "João Silva",
        Email = "joao@example.com",
        Cellphone = "+5511999999999",
        TaxId = "12345678901"
    }
};

// Traditional way (still supported)
var pixQrCode = await client.CreatePixQrCodeAsync(pixRequest);

// New modular way (recommended)
var pixQrCode = await client.PixQrCodes.CreatePixQrCodeAsync(pixRequest);

Console.WriteLine($"PIX QR Code created: {pixQrCode.QrCode}");
```

### 4. Create a Billing

```csharp
var billingRequest = new BillingRequest
{
    Frequency = "ONE_TIME",
    Methods = new List<string> { "PIX" },
    Products = new List<BillingProduct>
    {
        new BillingProduct
        {
            ExternalId = "prod-001",
            Name = "Premium Plan",
            Description = "Monthly subscription",
            Quantity = 1,
            Price = 5000 // R$ 50.00 in cents
        }
    },
    ReturnUrl = "https://yoursite.com/return",
    CompletionUrl = "https://yoursite.com/success",
    CustomerId = customer.Id,
    AllowCoupons = true
};

// Traditional way (still supported)
var billing = await client.CreateBillingAsync(billingRequest);

// New modular way (recommended)
var billing = await client.Billings.CreateBillingAsync(billingRequest);

Console.WriteLine($"Billing created: {billing.Id}");
```

## Modular Architecture

The SDK now features a modular architecture with domain-specific services for better organization and testability.

### Available Services

- **`ICustomerService`** - Customer management operations
- **`IBillingService`** - Billing and payment operations  
- **`IPixQrCodeService`** - PIX QR Code operations
- **`ICouponService`** - Coupon management operations
- **`IStoreService`** - Store information operations
- **`IWithdrawService`** - Withdrawal operations

### Using Domain Services

```csharp
var client = new AbacatePayClient("your-api-key");

// Access domain-specific services
var customer = await client.Customers.CreateCustomerAsync(request);
var billing = await client.Billings.CreateBillingAsync(billingRequest);
var pix = await client.PixQrCodes.CreatePixQrCodeAsync(pixRequest);
var coupon = await client.Coupons.CreateCouponAsync(couponRequest);
var store = await client.Store.GetStoreAsync();
var withdraw = await client.Withdraws.CreateWithdrawAsync(withdrawRequest);
```

### Dependency Injection Support

```csharp
// Register services in your DI container
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IBillingService, BillingService>();
services.AddScoped<IPixQrCodeService, PixQrCodeService>();
services.AddScoped<ICouponService, CouponService>();
services.AddScoped<IStoreService, StoreService>();
services.AddScoped<IWithdrawService, WithdrawService>();

// Use in your controllers
public class PaymentController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IBillingService _billingService;

    public PaymentController(ICustomerService customerService, IBillingService billingService)
    {
        _customerService = customerService;
        _billingService = billingService;
    }
}
```

### Direct Service Usage

```csharp
// Use services directly without the main client
var httpService = new HttpService(httpClient, config);
var customerService = new CustomerService(httpService);
var billingService = new BillingService(httpService);

var customer = await customerService.CreateCustomerAsync(request);
var billing = await billingService.CreateBillingAsync(billingRequest);
```

### Testing with Mocks

```csharp
[Test]
public async Task CreateCustomer_ShouldReturnCustomer()
{
    // Arrange
    var mockCustomerService = new Mock<ICustomerService>();
    mockCustomerService.Setup(x => x.CreateCustomerAsync(It.IsAny<CustomerRequest>(), It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new CustomerResponseData());

    // Act
    var result = await mockCustomerService.Object.CreateCustomerAsync(request);

    // Assert
    Assert.IsNotNull(result);
}
```

## API Reference

### Customer Management

#### Create Customer
```csharp
Task<CustomerResponseData> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default)
```

#### List Customers
```csharp
Task<List<CustomerResponse>> ListCustomersAsync(CancellationToken cancellationToken = default)
```

### PIX QR Code Management

#### Create PIX QR Code
```csharp
Task<PixQrCodeData> CreatePixQrCodeAsync(PixQrCodeRequest request, CancellationToken cancellationToken = default)
```

#### Check PIX Status
```csharp
Task<PixQrCodeStatusData> CheckPixQrCodeStatusAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
```

#### Simulate Payment (Dev Mode)
```csharp
Task<PixQrCodeData> SimulatePixQrCodePaymentAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
```

### Billing Management

#### Create Billing
```csharp
Task<BillingData> CreateBillingAsync(BillingRequest request, CancellationToken cancellationToken = default)
```

#### Get Billing
```csharp
Task<BillingData> GetBillingAsync(string billingId, CancellationToken cancellationToken = default)
```

#### List Billings
```csharp
Task<List<BillingData>> ListBillingsAsync(CancellationToken cancellationToken = default)
```

### Coupon Management

#### Create Coupon
```csharp
Task<CouponData> CreateCouponAsync(CouponRequest request, CancellationToken cancellationToken = default)
```

#### List Coupons
```csharp
Task<List<CouponData>> ListCouponsAsync(CancellationToken cancellationToken = default)
```

### Store Management

#### Get Store Information
```csharp
Task<StoreData> GetStoreAsync(CancellationToken cancellationToken = default)
```

### Withdrawal Management

#### Create Withdrawal
```csharp
Task<WithdrawData> CreateWithdrawAsync(WithdrawRequest request, CancellationToken cancellationToken = default)
```

#### Get Withdrawal
```csharp
Task<WithdrawData> GetWithdrawAsync(string withdrawId, CancellationToken cancellationToken = default)
```

#### List Withdrawals
```csharp
Task<WithdrawData> ListWithdrawsAsync(CancellationToken cancellationToken = default)
```

## Migration Guide

### From Version 1.x to 2.x

The new version is **100% backward compatible**. Your existing code will continue to work without any changes.

#### Option 1: Keep Using Traditional API (No Changes Required)
```csharp
// Your existing code continues to work
var client = new AbacatePayClient("api-key");
var customer = await client.CreateCustomerAsync(request);
var billing = await client.CreateBillingAsync(billingRequest);
```

#### Option 2: Migrate to Modular API (Recommended)
```csharp
// Old way
var customer = await client.CreateCustomerAsync(request);

// New way (recommended)
var customer = await client.Customers.CreateCustomerAsync(request);
```

#### Option 3: Use Dependency Injection
```csharp
// Register services
services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IBillingService, BillingService>();

// Use in controllers
public class PaymentController : ControllerBase
{
    private readonly ICustomerService _customerService;
    
    public PaymentController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    public async Task<IActionResult> CreateCustomer(CustomerRequest request)
    {
        var customer = await _customerService.CreateCustomerAsync(request);
        return Ok(customer);
    }
}
```

### Benefits of Migration

- **Better Organization**: Domain-specific services
- **Easier Testing**: Mock individual services
- **Dependency Injection**: Clean architecture support
- **Single Responsibility**: Each service has one purpose
- **Future-Proof**: Easy to extend and maintain

## Configuration

### AbacatePayConfig Properties

| Property | Type | Description | Default |
|----------|------|-------------|---------|
| `ApiKey` | `string` | Your Bearer token from AbacatePay dashboard | Required |
| `BaseUrl` | `string` | Base URL for the API | `"https://api.abacatepay.com"` |
| `Sandbox` | `bool` | Whether to use sandbox mode | `false` |
| `TimeoutSeconds` | `int` | HTTP request timeout in seconds | `30` |

## Error Handling

The SDK throws `AbacatePayException` for API errors:

```csharp
try
{
    var customer = await client.CreateCustomerAsync(customerRequest);
}
catch (AbacatePayException ex)
{
    Console.WriteLine($"API Error: {ex.Message}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation Error: {ex.Message}");
}
```

## Validation

The SDK includes built-in validation for all request models:

- **Required fields**: Automatically validated
- **Data types**: Type safety enforced
- **Custom validation**: Business rules validated (e.g., frequency values, price minimums)
- **String length**: Maximum length constraints enforced

## Examples

### Real-World Project Example

Check out a complete backend implementation using this SDK:
- 🚀 **[AbacatePay PIX .NET Backend](https://github.com/oismaelash/abacatepay-pix-dotnet-backend)** - Full-featured backend API demonstrating SDK usage

### Complete Payment Flow

```csharp
using AbacatePay;
using AbacatePay.Models;

// Initialize client
var client = new AbacatePayClient("your-api-key");

try
{
    // 1. Create PIX payment
    var pixPayment = await client.CreatePixQrCodeAsync(new PixQrCodeRequest
    {
        Amount = 9700, // R$ 25.00
        Description = "SDK Product",
        Customer = {
            Name = "Ismael Ash",
            Cellphone = "11967435133",
            Email = "contato@ismaelnascimento.com",
            TsxId = "01364181045"
        }
    });

    Console.WriteLine($"Payment created! Code: {pixPayment.BrCode}");

    // 2. Check payment status
    var status = await client.CheckPixQrCodeStatusAsync(pixPayment.Id);
    Console.WriteLine($"Payment status: {status.Status}");
}
catch (AbacatePayException ex)
{
    Console.WriteLine($"Payment failed: {ex.Message}");
}
finally
{
    client.Dispose();
}
```

### Billing Flow Example

```csharp
var billingRequest = new BillingRequest
{
    Frequency = "ONE_TIME",
    Methods = new List<string> { "PIX" },
    Products = new List<BillingProduct>
    {
        new BillingProduct
        {
            ExternalId = "subscription-001",
            Name = "Premium Subscription",
            Description = "Monthly premium access",
            Quantity = 1,
            Price = 9900 // R$ 99.00
        }
    },
    ReturnUrl = "https://yoursite.com/billing/return",
    CompletionUrl = "https://yoursite.com/billing/success",
    CustomerId = "customer-id-here", // The ID of a customer already registered in your store.
    AllowCoupons = true,
    Coupons = new List<string> { "WELCOME10" }
};

var billing = await client.CreateBillingAsync(billingRequest);
Console.WriteLine($"Billing link: {billing.Url}");

```

## Development

### Building the SDK

```bash
# Clone the repository
git clone https://github.com/oismaelash/abacatepay-dotnet-sdk.git
cd abacatepay-dotnet-sdk

# Run the build script
sh ./build.sh
```

### Project Structure

```
src/
├── AbacatePay/
│   ├── AbacatePayClient.cs          # Main client class (facade)
│   ├── Models/                      # Data models
│   │   ├── Common/                  # Common models
│   │   ├── Customer/                # Customer models
│   │   ├── Billing/                 # Billing models
│   │   ├── PixQrCode/               # PIX QR Code models
│   │   ├── Coupon/                  # Coupon models
│   │   ├── Store/                   # Store models
│   │   └── Withdraw/                # Withdrawal models
│   ├── Enums/                       # Enumerations
│   ├── Services/                    # Domain services
│   │   ├── BaseService.cs           # Base service with common logic
│   │   ├── CustomerService.cs       # Customer operations
│   │   ├── BillingService.cs        # Billing operations
│   │   ├── PixQrCodeService.cs     # PIX operations
│   │   ├── CouponService.cs         # Coupon operations
│   │   ├── StoreService.cs          # Store operations
│   │   ├── WithdrawService.cs       # Withdrawal operations
│   │   ├── HttpService.cs           # HTTP communication
│   │   └── Interfaces/              # Service interfaces
│   │       ├── ICustomerService.cs
│   │       ├── IBillingService.cs
│   │       ├── IPixQrCodeService.cs
│   │       ├── ICouponService.cs
│   │       ├── IStoreService.cs
│   │       ├── IWithdrawService.cs
│   │       └── IHttpService.cs
│   ├── Validators/                  # Request validators
│   └── Attributes/                  # Custom validation attributes
```

## Requirements

- .NET Standard 2.1 or higher
- .NET 5.0 or higher
- .NET Core 3.1 or higher
- .NET Framework 4.8 or higher

## Dependencies

- `Newtonsoft.Json` (13.0.3)
- `System.ComponentModel.Annotations` (5.0.0)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Learn More

- 📺 **[YouTube Tutorial](https://youtube.com/watch?v=YOUR_VIDEO_ID)** - Watch the complete project walkthrough and SDK implementation
- 🚀 **[Example Backend Project](https://github.com/oismaelash/abacatepay-pix-dotnet-backend)** - See the SDK in action with a real backend implementation

## Support

- 📚 [Documentation](https://docs.abacatepay.com/)
- 🐛 [Issue Tracker](https://github.com/oismaelash/abacatepay-dotnet-sdk/issues)
- 💬 [Support](https://abacatepay.com)

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Changelog

### Version 2.0.0
- 🏗️ **Modular Architecture**: Domain-specific services for better organization
- 🔧 **Dependency Injection**: Full support for DI containers
- 🧪 **Enhanced Testability**: Easy mocking and unit testing
- 🔄 **Backward Compatibility**: All existing code continues to work
- 📦 **Service Interfaces**: Clean abstractions for all domain operations
- 🎯 **Single Responsibility**: Each service handles one domain
- ⚡ **Performance**: Optimized service layer with common base class

### Version 1.0.0
- Initial release
- Customer management
- PIX QR Code payments
- Billing system
- Coupon management
- Store information
- Withdrawal management
- Comprehensive validation
- Async/await support

---

Made with ❤️ by [Ismael Ash](https://github.com/oismaelash) for the Brazilian fintech ecosystem.
