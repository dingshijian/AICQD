using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class SimpleStatusResponse
    {
        [JsonPropertyName("status")] public string Status { get; set; } = "success";
        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    }
}

