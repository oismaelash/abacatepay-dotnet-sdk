using Newtonsoft.Json;

namespace AbacatePay.Models.Store;

/// <summary>
/// Store balance information
/// </summary>
public class Balance
{
    /// <summary>
    /// Available balance
    /// </summary>
    [JsonProperty("available")]
    public decimal Available { get; set; }

    /// <summary>
    /// Pending balance
    /// </summary>
    [JsonProperty("pending")]
    public decimal Pending { get; set; }

    /// <summary>
    /// Blocked balance
    /// </summary>
    [JsonProperty("blocked")]
    public decimal Blocked { get; set; }
}
