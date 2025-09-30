using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class HealthResponse
    {
        [JsonPropertyName("status")] public string? Status { get; set; }
        [JsonPropertyName("message")] public string? Message { get; set; }
    }
}

