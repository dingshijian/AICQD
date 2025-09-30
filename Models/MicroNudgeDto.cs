using System.Text.Json.Serialization;
using System;

namespace AICQD.Models
{
    public sealed class MicroNudgeDto
    {
        public int Id { get; set; }
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("nudge_content")] public string NudgeContent { get; set; } = default!;
        [JsonPropertyName("nudge_type")] public string NudgeType { get; set; } = default!;
        [JsonPropertyName("trigger_condition")] public string? TriggerCondition { get; set; }
        [JsonPropertyName("trigger_value")] public string? TriggerValue { get; set; }
        [JsonPropertyName("scheduled_time")] public DateTime ScheduledTime { get; set; }
        [JsonPropertyName("sent_time")] public DateTime? SentTime { get; set; }
        [JsonPropertyName("is_sent")] public bool IsSent { get; set; }
        [JsonPropertyName("is_dismissed")] public bool IsDismissed { get; set; }
        [JsonPropertyName("is_acted_upon")] public bool IsActedUpon { get; set; }
        [JsonPropertyName("action_taken")] public string? ActionTaken { get; set; }
        [JsonPropertyName("effectiveness_score")] public int? EffectivenessScore { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    }
}

