using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AbacatePay.Models.Webhook;

/// <summary>
/// Webhook configuration request
/// </summary>
public class WebhookConfigRequest
{
    /// <summary>
    /// Webhook URL
    /// </summary>
    [Required]
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Events to listen for
    /// </summary>
    [JsonProperty("events")]
    public List<WebhookEventType> Events { get; set; } = new();

    /// <summary>
    /// Whether the webhook is active
    /// </summary>
    [JsonProperty("active")]
    public bool Active { get; set; } = true;

    /// <summary>
    /// Webhook secret for signature verification
    /// </summary>
    [JsonProperty("secret")]
    public string? Secret { get; set; }
}

/// <summary>
/// Webhook configuration response
/// </summary>
public class WebhookConfigResponse
{
    /// <summary>
    /// Webhook configuration ID
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Webhook URL
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Events configured for this webhook
    /// </summary>
    [JsonProperty("events")]
    public List<WebhookEventType> Events { get; set; } = new();

    /// <summary>
    /// Whether the webhook is active
    /// </summary>
    [JsonProperty("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Webhook creation timestamp
    /// </summary>
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Webhook last update timestamp
    /// </summary>
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Webhook secret (only returned when creating/updating)
    /// </summary>
    [JsonProperty("secret")]
    public string? Secret { get; set; }

    /// <summary>
    /// Webhook statistics
    /// </summary>
    [JsonProperty("stats")]
    public WebhookStats? Stats { get; set; }
}

/// <summary>
/// Webhook statistics
/// </summary>
public class WebhookStats
{
    /// <summary>
    /// Total events sent
    /// </summary>
    [JsonProperty("total_events")]
    public int TotalEvents { get; set; }

    /// <summary>
    /// Successful deliveries
    /// </summary>
    [JsonProperty("successful_deliveries")]
    public int SuccessfulDeliveries { get; set; }

    /// <summary>
    /// Failed deliveries
    /// </summary>
    [JsonProperty("failed_deliveries")]
    public int FailedDeliveries { get; set; }

    /// <summary>
    /// Last delivery timestamp
    /// </summary>
    [JsonProperty("last_delivery")]
    public DateTime? LastDelivery { get; set; }
}
