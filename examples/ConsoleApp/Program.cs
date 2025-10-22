using AbacatePay;
using AbacatePay.Models;
using AbacatePay.Models.Payment;
using AbacatePay.Models.Webhook;
using AbacatePay.Models.Customer;
using AbacatePay.Models.Coupon;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;
using AbacatePay.Models.Withdraw;
using AbacatePay.Models.Store;

namespace AbacatePay.Examples.ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("AbacatePay .NET SDK Example");
        Console.WriteLine("===========================");

        // Initialize the client (replace with your actual credentials)
        var client = new AbacatePayClient("your_bearer_token", sandbox: true);

        try
        {
            // Example 1: Create a PIX payment
            await CreatePixPaymentExample(client);

            // Example 2: Create a Boleto payment
            await CreateBoletoPaymentExample(client);

            // Example 3: List payments
            await ListPaymentsExample(client);

            // Example 4: Webhook configuration
            await WebhookExample(client);

            // Example 5: Customer management
            await CustomerExample(client);

            // Example 6: Coupon management
            await CouponExample(client);

            // Example 7: Billing management
            await BillingExample(client);

            // Example 8: PIX QRCode
            await PixQrCodeExample(client);

            // Example 9: Withdraw management
            await WithdrawExample(client);

            // Example 10: Store information
            await StoreExample(client);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            client.Dispose();
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task CreatePixPaymentExample(AbacatePayClient client)
    {
        Console.WriteLine("\n1. Creating PIX Payment...");

        var paymentRequest = new PaymentRequest
        {
            Amount = 10000, // R$ 100.00
            Currency = "BRL",
            Description = "Example PIX Payment",
            PaymentMethod = PaymentMethod.PIX,
            Customer = new CustomerInfo
            {
                Name = "João Silva",
                Email = "joao@example.com",
                Document = "12345678901",
                Phone = "+5511999999999"
            },
            PaymentOptions = new PaymentOptions
            {
                Pix = new PixOptions
                {
                    PixKey = "joao@example.com",
                    PixKeyType = PixKeyType.EMAIL
                }
            },
            ExpiresIn = 3600, // 1 hour
            Metadata = new Dictionary<string, object>
            {
                { "order_id", "12345" },
                { "customer_id", "67890" }
            }
        };

        var payment = await client.CreatePaymentAsync(paymentRequest);

        Console.WriteLine($"Payment created successfully!");
        Console.WriteLine($"Payment ID: {payment.Id}");
        Console.WriteLine($"Status: {payment.Status}");
        Console.WriteLine($"Amount: R$ {payment.Amount / 100.0:F2}");
        Console.WriteLine($"PIX QR Code: {payment.PaymentData?.Pix?.QrCode}");
        Console.WriteLine($"Copy & Paste Code: {payment.PaymentData?.Pix?.CopyPasteCode}");
    }

    static async Task CreateBoletoPaymentExample(AbacatePayClient client)
    {
        Console.WriteLine("\n2. Creating Boleto Payment...");

        var boletoRequest = new PaymentRequest
        {
            Amount = 25000, // R$ 250.00
            Currency = "BRL",
            Description = "Example Boleto Payment",
            PaymentMethod = PaymentMethod.BOLETO,
            Customer = new CustomerInfo
            {
                Name = "Maria Santos",
                Email = "maria@example.com",
                Document = "98765432100",
                Address = new AddressInfo
                {
                    Street = "Rua das Flores",
                    Number = "123",
                    Neighborhood = "Centro",
                    City = "São Paulo",
                    State = "SP",
                    ZipCode = "01234567"
                }
            },
            PaymentOptions = new PaymentOptions
            {
                Boleto = new BoletoOptions
                {
                    ExpiresAt = DateTime.Now.AddDays(3),
                    Instructions = "Pay until the expiration date to avoid interest"
                }
            },
            ExpiresIn = 86400 // 24 hours
        };

        var boletoPayment = await client.CreatePaymentAsync(boletoRequest);

        Console.WriteLine($"Boleto payment created successfully!");
        Console.WriteLine($"Payment ID: {boletoPayment.Id}");
        Console.WriteLine($"Status: {boletoPayment.Status}");
        Console.WriteLine($"Amount: R$ {boletoPayment.Amount / 100.0:F2}");
        Console.WriteLine($"Boleto Number: {boletoPayment.PaymentData?.Boleto?.BoletoNumber}");
        Console.WriteLine($"Barcode: {boletoPayment.PaymentData?.Boleto?.Barcode}");
        Console.WriteLine($"Digitable Line: {boletoPayment.PaymentData?.Boleto?.DigitableLine}");
        Console.WriteLine($"PDF URL: {boletoPayment.PaymentData?.Boleto?.PdfUrl}");
    }

    static async Task ListPaymentsExample(AbacatePayClient client)
    {
        Console.WriteLine("\n3. Listing Recent Payments...");

        var payments = await client.ListPaymentsAsync(
            limit: 10,
            offset: 0,
            createdAfter: DateTime.Now.AddDays(-7)
        );

        Console.WriteLine($"Found {payments.Count} payments:");
        foreach (var payment in payments)
        {
            Console.WriteLine($"- {payment.Id}: {payment.Status} - R$ {payment.Amount / 100.0:F2} ({payment.PaymentMethod})");
        }
    }

    static async Task WebhookExample(AbacatePayClient client)
    {
        Console.WriteLine("\n4. Configuring Webhook...");

        var webhookConfig = new WebhookConfigRequest
        {
            Url = "https://yourapp.com/webhook",
            Events = new List<WebhookEventType>
            {
                WebhookEventType.PAYMENT_COMPLETED,
                WebhookEventType.PAYMENT_FAILED,
                WebhookEventType.PAYMENT_CANCELLED,
                WebhookEventType.REFUND_COMPLETED
            },
            Active = true,
            Secret = "your_webhook_secret_here"
        };

        try
        {
            var webhook = await client.CreateWebhookAsync(webhookConfig);
            Console.WriteLine($"Webhook configured successfully!");
            Console.WriteLine($"Webhook ID: {webhook.Id}");
            Console.WriteLine($"URL: {webhook.Url}");
            Console.WriteLine($"Events: {string.Join(", ", webhook.Events)}");
            Console.WriteLine($"Active: {webhook.Active}");
        }
        catch (AbacatePayException ex)
        {
            Console.WriteLine($"Webhook configuration failed: {ex.Message}");
        }
    }

    static async Task CustomerExample(AbacatePayClient client)
    {
        Console.WriteLine("\n5. Customer Management...");

        try
        {
            // Create a customer
            var customerRequest = new CustomerRequest
            {
                Name = "João Silva",
                Cellphone = "(11) 99999-9999",
                Email = "joao@example.com",
                TaxId = "123.456.789-01"
            };

            var customer = await client.CreateCustomerAsync(customerRequest);
            Console.WriteLine($"Customer created: {customer.Id}");

            // List customers
            var customers = await client.ListCustomersAsync();
            Console.WriteLine($"Total customers: {customers.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Customer example failed: {ex.Message}");
        }
    }

    static async Task CouponExample(AbacatePayClient client)
    {
        Console.WriteLine("\n6. Coupon Management...");

        try
        {
            // Create a coupon
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

            // List coupons
            var coupons = await client.ListCouponsAsync();
            Console.WriteLine($"Total coupons: {coupons.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Coupon example failed: {ex.Message}");
        }
    }

    static async Task BillingExample(AbacatePayClient client)
    {
        Console.WriteLine("\n7. Billing Management...");

        try
        {
            // Create a billing
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
                ReturnUrl = "https://example.com/return",
                CompletionUrl = "https://example.com/success",
                Customer = new BillingCustomerData
                {
                    Name = "Maria Santos",
                    Cellphone = "(11) 88888-8888",
                    Email = "maria@example.com",
                    TaxId = "987.654.321-00"
                }
            };

            var billing = await client.CreateBillingAsync(billingRequest);
            Console.WriteLine($"Billing created: {billing.Url}");

            // List billings
            var billings = await client.ListBillingsAsync();
            Console.WriteLine($"Total billings: {billings.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Billing example failed: {ex.Message}");
        }
    }

    static async Task PixQrCodeExample(AbacatePayClient client)
    {
        Console.WriteLine("\n8. PIX QRCode Management...");

        try
        {
            // Create a PIX QRCode
            var pixRequest = new PixQrCodeRequest
            {
                Amount = 10000, // R$ 100.00
                ExpiresIn = 3600, // 1 hour
                Description = "Payment for service",
                Customer = new PixQrCodeCustomer
                {
                    Name = "Carlos Oliveira",
                    Cellphone = "(11) 77777-7777",
                    Email = "carlos@example.com",
                    TaxId = "111.222.333-44"
                }
            };

            var pixQrCode = await client.CreatePixQrCodeAsync(pixRequest);
            Console.WriteLine($"PIX QRCode created: {pixQrCode.Id}");
            Console.WriteLine($"BR Code: {pixQrCode.BrCode}");

            // Check status
            var status = await client.CheckPixQrCodeStatusAsync(pixQrCode.Id);
            Console.WriteLine($"PIX Status: {status.Status}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"PIX QRCode example failed: {ex.Message}");
        }
    }

    static async Task WithdrawExample(AbacatePayClient client)
    {
        Console.WriteLine("\n9. Withdraw Management...");

        try
        {
            // Create a withdraw
            var withdrawRequest = new WithdrawRequest
            {
                Amount = 50000, // R$ 500.00
                PixKey = "user@email.com",
                Notes = "Monthly earnings withdrawal"
            };

            var withdraw = await client.CreateWithdrawAsync(withdrawRequest);
            Console.WriteLine($"Withdraw created: {withdraw.Id}");

            // List withdraws
            var withdraws = await client.ListWithdrawsAsync();
            Console.WriteLine($"Total withdraws: {withdraws.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Withdraw example failed: {ex.Message}");
        }
    }

    static async Task StoreExample(AbacatePayClient client)
    {
        Console.WriteLine("\n10. Store Information...");

        try
        {
            // Get store information
            var store = await client.GetStoreAsync();
            Console.WriteLine($"Store: {store.Name} (ID: {store.Id})");
            Console.WriteLine($"Created at: {store.CreatedAt}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Store example failed: {ex.Message}");
        }
    }
}
