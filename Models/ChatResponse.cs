using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class ChatResponse
    {
        [JsonPropertyName("conversation_id")] public int ConversationId { get; set; }
        [JsonPropertyName("user_message")] public string UserMessage { get; set; } = default!;
        [JsonPropertyName("ai_response")] public string AiResponse { get; set; } = default!;
        [JsonPropertyName("conversation_type")] public string ConversationType { get; set; } = "general";
        [JsonPropertyName("sentiment")] public string Sentiment { get; set; } = "neutral";
        [JsonPropertyName("timestamp")] public string Timestamp { get; set; } = default!;
    }
}

