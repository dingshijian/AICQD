using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class AiMessageRespondResponse
    {
        [JsonPropertyName("status")] public string Status { get; set; } = "success";
        [JsonPropertyName("message")] public string Message { get; set; } = "Response recorded";
        // Present if backend generates a follow-up
        [JsonPropertyName("ai_follow_up")] public string? AiFollowUp { get; set; }
    }
}
