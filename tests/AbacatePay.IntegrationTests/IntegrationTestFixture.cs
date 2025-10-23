using AbacatePay;
using AbacatePay.Models;
using DotNetEnv;

namespace AbacatePay.IntegrationTests;

/// <summary>
/// Fixture for integration tests that provides a configured AbacatePay client
/// </summary>
public class IntegrationTestFixture : IDisposable
{
    public AbacatePayClient Client { get; }
    public string ApiKey { get; }
    public bool Sandbox { get; }
    public string BaseUrl { get; }

    public IntegrationTestFixture()
    {
        // Load environment variables from .env file
        Env.Load();

        // Get configuration from environment variables
        ApiKey = Environment.GetEnvironmentVariable("ABACATEPAY_API_KEY") 
                 ?? throw new InvalidOperationException(
                     "ABACATEPAY_API_KEY environment variable is required. " +
                     "Please create a .env file with your API credentials. " +
                     "See env.example for reference.");

        var sandboxValue = Environment.GetEnvironmentVariable("ABACATEPAY_SANDBOX")?.ToLower();
        Sandbox = sandboxValue == "true" || sandboxValue == "1";

        BaseUrl = Environment.GetEnvironmentVariable("ABACATEPAY_BASE_URL") 
                  ?? "https://api.abacatepay.com";

        // Create client with configuration
        var config = new AbacatePayConfig
        {
            ApiKey = ApiKey,
            Sandbox = Sandbox,
            BaseUrl = BaseUrl,
            TimeoutSeconds = int.Parse(Environment.GetEnvironmentVariable("ABACATEPAY_TIMEOUT") ?? "30")
        };

        Client = new AbacatePayClient(config);

        // Validate connection
        ValidateConnection();
    }

    private async void ValidateConnection()
    {
        try
        {
            // Try to get store information to validate the connection
            await Client.GetStoreAsync();
            Console.WriteLine($"✅ Connected to AbacatePay API (Sandbox: {Sandbox})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to connect to AbacatePay API: {ex.Message}");
            throw new InvalidOperationException(
                "Failed to connect to AbacatePay API. Please check your API key and network connection.", ex);
        }
    }

    public void Dispose()
    {
        Client?.Dispose();
    }
}
