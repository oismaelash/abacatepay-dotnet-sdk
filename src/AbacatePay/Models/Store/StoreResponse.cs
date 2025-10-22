using Newtonsoft.Json;

namespace AbacatePay.Models.Store;

/// <summary>
/// Store response from API
/// </summary>
public class StoreResponse
{
    /// <summary>
    /// Store ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Store name
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Store creation timestamp
    /// </summary>
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }
}
