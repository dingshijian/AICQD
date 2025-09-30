using System.Text.Json.Serialization;
using System;

namespace AICQD.Models
{
    public sealed class AIMessageDto
    {
        public int Id { get; set; }
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("message_content")] public string MessageContent { get; set; } = default!;
        [JsonPropertyName("message_type")] public string MessageType { get; set; } = "motivational";
        [JsonPropertyName("scheduled_time")] public DateTime ScheduledTime { get; set; }
        [JsonPropertyName("sent_time")] public DateTime? SentTime { get; set; }
        [JsonPropertyName("is_sent")] public bool IsSent { get; set; }
        [JsonPropertyName("is_read")] public bool IsRead { get; set; }
        [JsonPropertyName("user_response")] public string? UserResponse { get; set; }
        [JsonPropertyName("response_time")] public DateTime? ResponseTime { get; set; }
        [JsonPropertyName("priority")] public string Priority { get; set; } = "normal";
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    }
}
