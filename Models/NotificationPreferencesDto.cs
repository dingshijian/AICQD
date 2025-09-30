using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class NotificationPreferencesDto
    {
        [JsonPropertyName("enable_ai_texting")] public bool EnableAiTexting { get; set; }
        [JsonPropertyName("enable_micro_nudges")] public bool EnableMicroNudges { get; set; }

        // Server returns string[]; send either string[] or comma-separated string (service handles both)
        [JsonPropertyName("preferred_contact_times")] public List<string> PreferredContactTimes { get; set; } = new();

        [JsonPropertyName("timezone")] public string Timezone { get; set; } = "UTC";
        [JsonPropertyName("ai_message_frequency")] public string AiMessageFrequency { get; set; } = "daily";
        [JsonPropertyName("last_ai_message_sent")] public DateTime? LastAiMessageSent { get; set; }
    }
}

