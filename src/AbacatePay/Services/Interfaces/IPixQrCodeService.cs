using AbacatePay.Models.PixQrCode;

namespace AbacatePay.Services;

/// <summary>
/// Service interface for PIX QRCode operations
/// </summary>
public interface IPixQrCodeService
{
    /// <summary>
    /// Create a new PIX QRCode
    /// </summary>
    /// <param name="request">PIX QRCode request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    Task<PixQrCodeData> CreatePixQrCodeAsync(PixQrCodeRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check PIX QRCode status
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode status response</returns>
    Task<PixQrCodeStatusData> CheckPixQrCodeStatusAsync(string pixQrCodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Simulate PIX QRCode payment (Dev Mode only)
    /// </summary>
    /// <param name="pixQrCodeId">PIX QRCode ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>PIX QRCode response</returns>
    Task<PixQrCodeData> SimulatePixQrCodePaymentAsync(string pixQrCodeId, CancellationToken cancellationToken = default);
}
