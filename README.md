# AbacatePay .NET SDK

[![NuGet version](https://img.shields.io/nuget/v/AbacatePay.SDK.svg)](https://www.nuget.org/packages/AbacatePay.SDK/)
[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.1-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/oismaelash/abacatepay-dotnet-sdk)

A comprehensive .NET SDK for integrating with the AbacatePay API - Brazil's leading payment solution supporting PIX payments.

## Features

- 🚀 **Complete API Coverage** - All AbacatePay endpoints implemented
- 💳 **PIX QRCode Support** - Create and manage PIX QR codes
- 🔒 **Secure Authentication** - Bearer token authentication
- 📦 **Easy Integration** - Simple, intuitive API design
- 🛡️ **Type Safety** - Full C# type definitions and validation
- 🔄 **Async/Await Support** - Modern async programming patterns
- 👥 **Customer Management** - Create and manage customers
- 🎫 **Coupon System** - Create and manage discount coupons
- 📋 **Billing System** - Create comprehensive billing with products
- 📱 **PIX QRCode** - Generate and manage PIX QR codes with simulation
- 🏪 **Store Management** - Get store information
- 🏗️ **Extensible** - Easy to extend and customize
- ✅ **Comprehensive Validation** - Built-in request validation
- 🔧 **Cancellation Support** - Full cancellation token support

## Installation

### NuGet Package Manager
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

## Local Development

### Using the Package Locally

To test the SDK locally during development, you can reference the project directly or use a local package:

#### Option 1: Direct Project Reference

Add a direct reference to the project in your `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\abacatepay-dotnet-sdk\src\AbacatePay\AbacatePay.csproj" />
  </ItemGroup>
</Project>
```

#### Option 2: Local Package (Recommended)

1. **Generate the package locally:**
```bash
cd abacatepay-dotnet-sdk
dotnet pack --configuration Release --output ./packages
```

2. **Add local NuGet source:**
```bash
dotnet nuget add source ./packages --name "LocalPackages"
```

3. **Install the local package:**
```bash
dotnet add package AbacatePay.SDK --source LocalPackages
```

#### Option 3: Using NuGet.config

Create a `nuget.config` file in your project root:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="LocalPackages" value="C:\path\to\abacatepay-dotnet-sdk\packages" />
  </packageSources>
</configuration>
```

### Development and Testing

To facilitate development, you can use the included build script:

```bash
# Full build with tests
./build.sh

# Build only
dotnet build

# Tests only
dotnet test
```

### Configuration for Local Testing

To test the SDK locally, you'll need to configure the API credentials:

#### 1. Create .env file (for integration tests)

Create a `.env` file in the `tests/AbacatePay.IntegrationTests/` folder:

```bash
# Copy the example file
cp tests/AbacatePay.IntegrationTests/env.example tests/AbacatePay.IntegrationTests/.env

# Edit the .env file with your credentials
ABACATEPAY_API_KEY=your_api_key_here
ABACATEPAY_SANDBOX=true
```

#### 2. Configuration in your test project

```csharp
// In your test project
var config = new AbacatePayConfig
{
    ApiKey = Environment.GetEnvironmentVariable("ABACATEPAY_API_KEY") ?? "your_api_key",
    Sandbox = true, // Use sandbox for testing
    BaseUrl = "https://api.abacatepay.com",
    TimeoutSeconds = 30
};

var client = new AbacatePayClient(config);
```

#### 3. Test project example

```xml
<!-- In your test project -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.0.0" />
    <PackageReference Include="xunit" Version="2.4.1" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.3" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\abacatepay-dotnet-sdk\src\AbacatePay\AbacatePay.csproj" />
  </ItemGroup>
</Project>
```

### Troubleshooting - Local Usage

#### Issue: "Package not found" when using local package

**Solution:**
```bash
# Check if the local source is configured
dotnet nuget list source

# If not listed, add it again
dotnet nuget add source ./packages --name "LocalPackages"
```

#### Issue: "Version conflict" with local packages

**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Reinstall the package
dotnet remove package AbacatePay.SDK
dotnet add package AbacatePay.SDK --source LocalPackages
```

#### Issue: "Build failed" when referencing project directly

**Solution:**
```bash
# Make sure the SDK project is built
cd abacatepay-dotnet-sdk
dotnet build

# Then reference in your project
cd ../your-project
dotnet build
```

#### Issue: "API Key not found" in tests

**Solution:**
```bash
# Check if the .env file exists and has correct credentials
cat tests/AbacatePay.IntegrationTests/.env

# If it doesn't exist, copy from example
cp tests/AbacatePay.IntegrationTests/env.example tests/AbacatePay.IntegrationTests/.env
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
    Sandbox = true, // Set to false for production
    BaseUrl = "https://api.abacatepay.com",
    TimeoutSeconds = 30
};
var client = new AbacatePayClient(config);
```

### 2. Create a Customer

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

### 3. List Customers

```csharp
var customers = await client.ListCustomersAsync();
foreach (var customer in customers)
{
    Console.WriteLine($"Customer: {customer.Name} - {customer.Email}");
}
```

### 4. Create a Coupon

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

### 5. Create a Billing

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

### 6. Create a PIX QRCode

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

### 7. Check PIX QRCode Status

```csharp
var status = await client.CheckPixQrCodeStatusAsync("pix_qrcode_id");
Console.WriteLine($"PIX QRCode Status: {status.Status}");
```

### 8. Simulate PIX QRCode Payment (Dev Mode)

```csharp
var simulateRequest = new PixQrCodeSimulateRequest
{
    // Add simulation parameters as needed
};

var result = await client.SimulatePixQrCodePaymentAsync("pix_qrcode_id", simulateRequest);
Console.WriteLine($"Simulation result: {result.Status}");
```

### 9. Get Store Information

```csharp
var store = await client.GetStoreAsync();
Console.WriteLine($"Store: {store.Name} (ID: {store.Id})");
```

### 10. List Billings

```csharp
var billings = await client.ListBillingsAsync();
foreach (var billing in billings)
{
    Console.WriteLine($"Billing: {billing.Id} - {billing.Status}");
}
```

### 11. List Coupons

```csharp
var coupons = await client.ListCouponsAsync();
foreach (var coupon in coupons)
{
    Console.WriteLine($"Coupon: {coupon.Data.Code} - {coupon.Data.Discount}% off");
}
```

### 12. Get Billing Details

```csharp
var billing = await client.GetBillingAsync("billing_id");
Console.WriteLine($"Billing: {billing.Id} - Status: {billing.Status}");
```

## Available Methods

### Customer Management
- `CreateCustomerAsync()` - Create a new customer
- `ListCustomersAsync()` - List all customers

### Coupon Management  
- `CreateCouponAsync()` - Create a new coupon
- `ListCouponsAsync()` - List all coupons

### Billing Management
- `CreateBillingAsync()` - Create a new billing
- `GetBillingAsync()` - Get billing details by ID
- `ListBillingsAsync()` - List all billings

### PIX QRCode Management
- `CreatePixQrCodeAsync()` - Create a new PIX QRCode
- `CheckPixQrCodeStatusAsync()` - Check PIX QRCode status
- `SimulatePixQrCodePaymentAsync()` - Simulate PIX QRCode payment (Dev Mode)

### Store Management
- `GetStoreAsync()` - Get store information



## Error Handling

The SDK provides comprehensive error handling with detailed error information:

```csharp
try
{
    var customer = await client.CreateCustomerAsync(customerRequest);
}
catch (AbacatePayException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Status Code: {ex.StatusCode}");
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
    Console.WriteLine($"Response Body: {ex.ResponseBody}");
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
    Sandbox = false // Set to true for testing
};
```

## Advanced Usage

### Using Cancellation Tokens

```csharp
var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;

try
{
    var customer = await client.CreateCustomerAsync(customerRequest, cancellationToken);
    Console.WriteLine($"Customer created: {customer.Id}");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled due to timeout");
}
```

### Validation and Error Handling

The SDK includes comprehensive validation for all request objects:

```csharp
try
{
    var billingRequest = new BillingRequest
    {
        // Missing required fields will throw ArgumentException
        Frequency = BillingFrequency.ONE_TIME,
        Methods = new List<BillingPaymentMethod> { BillingPaymentMethod.PIX },
        Products = new List<BillingProduct>(), // Empty list will cause validation error
        ReturnUrl = "invalid-url", // Invalid URL format will cause validation error
        CompletionUrl = "https://valid-url.com"
    };
    
    var billing = await client.CreateBillingAsync(billingRequest);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
}
```

## Requirements

- .NET Standard 2.1 or later (.NET 6.0+ recommended)
- Internet connection for API calls

## Dependencies

- Newtonsoft.Json (13.0.3)
- System.ComponentModel.Annotations (5.0.0)

## API Reference

For complete API documentation, visit: [https://docs.abacatepay.com](https://docs.abacatepay.com)

### Available Endpoints

- **Customers**: Create and list customers
- **Coupons**: Create and list discount coupons  
- **Billing**: Create, get, and list comprehensive billing with products
- **PIX QR Codes**: Create, check status, and simulate PIX QR codes
- **Store**: Get store information

## License

This SDK is licensed under the MIT License.

## Support

For support and questions:
- 📧 Email: dev@abacatepay.com
- 📚 Documentation: [https://docs.abacatepay.com](https://docs.abacatepay.com)
- 🐛 GitHub Issues: [https://github.com/oismaelash/abacatepay-dotnet-sdk/issues](https://github.com/oismaelash/abacatepay-dotnet-sdk/issues)
- 💬 Discord: [Join our community](https://discord.gg/abacatepay)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Setup

1. Clone the repository
2. Install dependencies: `dotnet restore`
3. Run tests: `dotnet test`
4. Build the project: `dotnet build`

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Include unit tests for new features

## Changelog

### Version 1.0.0
- Initial release
- Complete API coverage
- PIX payment support
- Customer management
- Coupon system
- Billing system
- Webhook support
