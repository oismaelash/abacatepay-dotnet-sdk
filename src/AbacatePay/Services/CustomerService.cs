using AbacatePay.Models.Customer;
using AbacatePay.Validators;

namespace AbacatePay.Services;

/// <summary>
/// Service implementation for customer operations
/// </summary>
public class CustomerService : BaseService, ICustomerService
{
    public CustomerService(IHttpService httpService) : base(httpService)
    {
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="request">Customer request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer response</returns>
    public async Task<CustomerResponseData> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken = default)
    {
        RequestValidator.ValidateCustomerRequest(request);
        
        return await ExecuteRequestAsync(
            () => HttpService.PostAsync<CustomerResponseData>("/v1/customer/create", request, cancellationToken),
            "Customer creation"
        );
    }

    /// <summary>
    /// List all customers
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of customers</returns>
    public async Task<List<CustomerResponse>> ListCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteListRequestAsync(
            () => HttpService.GetAsync<List<CustomerResponse>>("/v1/customer/list", cancellationToken),
            "Customer list"
        );
    }
}
