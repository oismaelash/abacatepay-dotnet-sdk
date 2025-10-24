using AbacatePay.Models.Customer;
using AbacatePay.Models.Coupon;
using AbacatePay.Models.Billing;
using AbacatePay.Models.PixQrCode;
using System.ComponentModel.DataAnnotations;

namespace AbacatePay.Validators;

/// <summary>
/// Centralized validation class for all request objects
/// </summary>
public static class RequestValidator
{
    /// <summary>
    /// Validates a customer request
    /// </summary>
    /// <param name="request">Customer request to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public static void ValidateCustomerRequest(CustomerRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Customer request validation failed: {string.Join(", ", errorMessages)}");
        }
    }

    /// <summary>
    /// Validates a coupon request
    /// </summary>
    /// <param name="request">Coupon request to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public static void ValidateCouponRequest(CouponRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Coupon request validation failed: {string.Join(", ", errorMessages)}");
        }

        // Additional business logic validation beyond data annotations
        if (request.MaxRedeems < -1 || request.MaxRedeems > 100)
            throw new ArgumentException("MaxRedeems must be between -1 (unlimited) and 100", nameof(request.MaxRedeems));

        if (request.DiscountKind != "PERCENTAGE" && request.DiscountKind != "FIXED")
            throw new ArgumentException("DiscountKind must be PERCENTAGE or FIXED", nameof(request.DiscountKind));

        if (request.DiscountKind == "PERCENTAGE" && (request.Discount < 0 || request.Discount > 100))
            throw new ArgumentException("Discount must be between 0 and 100 for PERCENTAGE type", nameof(request.Discount));

        if (request.DiscountKind == "FIXED" && request.Discount < 100)
            throw new ArgumentException("Discount must be at least 100 cents for FIXED type", nameof(request.Discount));

        if (request.DiscountKind == "PERCENTAGE" && (request.Discount < 1 || request.Discount > 100))
            throw new ArgumentException("Discount must be between 1 and 100 for PERCENTAGE type", nameof(request.Discount));

        if (request.DiscountKind == "FIXED" && request.Discount < 100)
            throw new ArgumentException("Discount must be at least 100 cents for FIXED type", nameof(request.Discount));
    }

    /// <summary>
    /// Validates a billing request
    /// </summary>
    /// <param name="request">Billing request to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public static void ValidateBillingRequest(BillingRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"Billing request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (!Uri.TryCreate(request.ReturnUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid return URL format", nameof(request.ReturnUrl));

        if (!Uri.TryCreate(request.CompletionUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid completion URL format", nameof(request.CompletionUrl));

        if (request.Products == null || !request.Products.Any())
            throw new ArgumentException("At least one product is required", nameof(request.Products));

        foreach (var product in request.Products)
        {
            if (product.Quantity < 1)
                throw new ArgumentException("Product quantity must be at least 1", nameof(product.Quantity));

            if (product.Price < 100)
                throw new ArgumentException("Product price must be at least 100 cents", nameof(product.Price));
        }

        if (string.IsNullOrWhiteSpace(request.CustomerId) && request.Customer == null)
            throw new ArgumentException("Either CustomerId or Customer data must be provided");
    }

    /// <summary>
    /// Validates a PIX QRCode request
    /// </summary>
    /// <param name="request">PIX QRCode request to validate</param>
    /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
    /// <exception cref="ArgumentException">Thrown when validation fails</exception>
    public static void ValidatePixQrCodeRequest(PixQrCodeRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(request);

        if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
        {
            var errorMessages = validationResults.Select(r => r.ErrorMessage).Where(m => !string.IsNullOrEmpty(m));
            throw new ArgumentException($"PIX QRCode request validation failed: {string.Join(", ", errorMessages)}");
        }

        if (request.Amount < 100)
            throw new ArgumentException("Amount must be at least 100 cents", nameof(request.Amount));
    }
}
