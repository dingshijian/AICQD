using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class AiMessageReadResponse
    {
        [JsonPropertyName("status")] public string Status { get; set; } = "success";
        [JsonPropertyName("message")] public string Message { get; set; } = "Message marked as read";
    }
}
