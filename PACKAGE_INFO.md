# AbacatePay .NET SDK Package Information

## Package Details

- **Package Name**: AbacatePay
- **Version**: 1.0.0
- **Target Framework**: .NET 6.0
- **Package Type**: NuGet Package
- **License**: MIT

## Package Location

The compiled NuGet package is located at:
```
packages/AbacatePay.1.0.0.nupkg
```

## Installation

### From Local Package
```bash
dotnet add package AbacatePay --source /path/to/packages
```

### From NuGet.org (when published)
```bash
dotnet add package AbacatePay
```

## Package Contents

### Core Components
- `AbacatePayClient` - Main client class for API interactions
- `AbacatePayConfig` - Configuration class
- `AbacatePayException` - Custom exception handling

### Models
- **Payment Models**: `PaymentRequest`, `PaymentResponse`, `PaymentStatus`
- **Refund Models**: `RefundRequest`, `RefundResponse`, `RefundStatus`
- **Webhook Models**: `WebhookEvent`, `WebhookConfigRequest`, `WebhookConfigResponse`
- **Customer Models**: `CustomerRequest`, `CustomerResponse`, `CustomerMetadata`
- **Coupon Models**: `CouponRequest`, `CouponResponse`, `CouponData`, `DiscountKind`
- **Billing Models**: `BillingRequest`, `BillingResponse`, `BillingProduct`, `BillingFrequency`
- **PIX QRCode Models**: `PixQrCodeRequest`, `PixQrCodeResponse`, `PixQrCodeStatusResponse`
- **Withdraw Models**: `WithdrawRequest`, `WithdrawResponse`
- **Store Models**: `StoreResponse`
- **Common Models**: `ApiResponse`, `CustomerInfo`, `AddressInfo`

### Services
- `IHttpService` - HTTP service interface
- `HttpService` - HTTP service implementation

## Features Implemented

✅ **Payment Processing**
- PIX payments with QR codes and copy-paste codes
- Boleto bancário with customizable expiration

✅ **Payment Management**
- Create payments
- Get payment details
- List payments with filters
- Cancel payments

✅ **Refund Processing**
- Create refunds (full or partial)
- Get refund details
- List refunds with filters

✅ **Webhook Support**
- Configure webhooks
- Handle webhook events
- Verify webhook signatures
- Support for all payment and refund events

✅ **Customer Management**
- Create customers
- List customers
- Customer data validation

✅ **Coupon System**
- Create discount coupons
- List coupons
- Support for percentage and fixed discounts

✅ **Billing System**
- Create comprehensive billing with products
- Support for one-time and recurring payments
- Integration with customer management

✅ **PIX QRCode**
- Generate PIX QR codes
- Check QR code status
- Simulate payments (dev mode)

✅ **Withdraw System**
- Create withdrawals
- List withdrawals
- PIX key integration

✅ **Store Management**
- Get store information
- Store metadata access

✅ **Authentication & Security**
- Bearer token authentication
- Secure HTTP communication
- Webhook signature verification

✅ **Error Handling**
- Comprehensive exception handling
- Detailed error information
- Input validation

✅ **Configuration**
- Sandbox and production environments
- Configurable timeouts
- Custom base URLs

## Dependencies

- Newtonsoft.Json (13.0.3)
- System.ComponentModel.Annotations (5.0.0)

## Examples

The package includes comprehensive examples in the `examples/` directory:
- Console application example
- Webhook handler example for ASP.NET Core

## Build Information

- **Build Date**: Generated on build
- **Build Configuration**: Release
- **Package Size**: ~50KB (estimated)
- **Assembly Size**: ~200KB (estimated)

## Testing

To test the package locally:

1. Build the package:
```bash
dotnet build src/AbacatePay/AbacatePay.csproj --configuration Release
```

2. Create a test project:
```bash
dotnet new console -n TestApp
cd TestApp
dotnet add package AbacatePay --source /path/to/packages
```

3. Use the package:
```csharp
using AbacatePay;

var client = new AbacatePayClient("your_api_key", "your_api_secret", sandbox: true);
```

## Publishing

To publish to NuGet.org:

1. Get a NuGet API key from [nuget.org](https://www.nuget.org)
2. Configure the API key:
```bash
dotnet nuget add source --name nuget.org https://api.nuget.org/v3/index.json
dotnet nuget push packages/AbacatePay.1.0.0.nupkg --api-key YOUR_API_KEY --source nuget.org
```

## Support

For questions and support:
- Email: dev@abacatepay.com
- Documentation: [README.md](README.md)
- Examples: [examples/](examples/)
