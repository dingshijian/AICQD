using System;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class ConversationDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("user_message")]
        public string UserMessage { get; set; } = default!;

        [JsonPropertyName("ai_response")]
        public string AiResponse { get; set; } = default!;

        [JsonPropertyName("conversation_type")]
        public string ConversationType { get; set; } = "general";

        // Default to "neutral" sentiment; override as needed
        [JsonPropertyName("sentiment")]
        public string Sentiment { get; set; } = "neutral";
    }
}
