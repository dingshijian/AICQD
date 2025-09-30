using System;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class MicroNudgeCreateRequest
    {
        // 'puff_pattern' | 'streak_reminder' | 'goal_progress' | 'health_tip' | 'community_engagement' (server default: 'streak_reminder')
        [JsonPropertyName("nudge_type")] public string NudgeType { get; set; } = "streak_reminder";

        [JsonPropertyName("trigger_condition")] public string? TriggerCondition { get; set; }
        [JsonPropertyName("trigger_value")] public string? TriggerValue { get; set; }

        // Optional; if omitted, server uses DateTime.UtcNow
        [JsonPropertyName("scheduled_time")] public DateTime? ScheduledTime { get; set; }

        // Optional; if provided, server uses this text directly
        [JsonPropertyName("custom_content")] public string? CustomContent { get; set; }
    }
}

