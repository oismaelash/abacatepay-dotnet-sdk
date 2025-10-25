using AbacatePay.Models.PixQrCode;
using AbacatePay.Validators;

namespace AbacatePay.Services;

/// <summary>
/// Service implementation for PIX QRCode operations
/// </summary>
public class PixQrCodeService : BaseService, IPixQrCodeService
{
    public PixQrCodeService(IHttpService httpService) : base(httpService)
    {
    }

    /// <summary>
    /// Create a new PIX QRCode
    /// </summary>
    /// <param name="request">PIX QRCode request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    public async Task<PixQrCodeData> CreatePixQrCodeAsync(PixQrCodeRequest request, CancellationToken cancellationToken = default)
    {
        RequestValidator.ValidatePixQrCodeRequest(request);
        
        return await ExecuteRequestAsync(
            () => HttpService.PostAsync<PixQrCodeData>("/v1/pixQrCode/create", request, cancellationToken),
            "PIX QRCode creation"
        );
    }

    /// <summary>
    /// Check PIX QRCode status
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode status response</returns>
    public async Task<PixQrCodeStatusData> CheckPixQrCodeStatusAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pixQrCodeId))
            throw new ArgumentException("PIX QRCode ID is required", nameof(pixQrCodeId));

        return await ExecuteRequestAsync(
            () => HttpService.GetAsync<PixQrCodeStatusData>($"/v1/pixQrCode/check?id={pixQrCodeId}", cancellationToken),
            "PIX QRCode status check"
        );
    }

    /// <summary>
    /// Simulate PIX QRCode payment (Dev Mode only)
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    public async Task<PixQrCodeData> SimulatePixQrCodePaymentAsync(string pixQrCodeId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pixQrCodeId))
            throw new ArgumentException("PIX QRCode ID is required", nameof(pixQrCodeId));

        return await ExecuteRequestAsync(
            () => HttpService.PostAsync<PixQrCodeData>($"/v1/pixQrCode/simulate-payment?id={pixQrCodeId}", cancellationToken),
            "PIX QRCode payment simulation"
        );
    }
}
