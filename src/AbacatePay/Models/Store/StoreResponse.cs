using Newtonsoft.Json;
using AbacatePay.Models;

namespace AbacatePay.Models.Store;

/// <summary>
/// Store data from API
/// </summary>
public class StoreData
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

    /// <summary>
    /// Store balance information
    /// </summary>
    [JsonProperty("balance")]
    public Balance Balance { get; set; } = new Balance();
}

/// <summary>
/// Store response from API with error and data wrapper
/// </summary>
public class StoreResponse : ApiResponseCustom<StoreData>
{
}
