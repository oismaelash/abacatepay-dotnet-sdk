using AbacatePay.Models.Customer;

namespace AbacatePay.Services;

/// <summary>
/// Service interface for customer operations
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="request">Customer request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer response</returns>
    Task<CustomerResponseData> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// List all customers
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers</returns>
    Task<List<CustomerResponse>> ListCustomersAsync(CancellationToken cancellationToken = default);
}
