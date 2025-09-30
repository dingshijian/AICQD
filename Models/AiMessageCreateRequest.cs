using System;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class AiMessageCreateRequest
    {
        // 'motivational' | 'reminder' | 'check_in' | 'milestone' | 'craving_support'
        [JsonPropertyName("message_type")] public string MessageType { get; set; } = "motivational";

        // Optional; if omitted, server schedules at next preferred contact time (UTC next day hh:mm)
        [JsonPropertyName("scheduled_time")] public DateTime? ScheduledTime { get; set; }

        [JsonPropertyName("priority")] public string Priority { get; set; } = "normal";

        // Optional; if provided, server will use this content directly
        [JsonPropertyName("custom_content")] public string? CustomContent { get; set; }
    }
}

