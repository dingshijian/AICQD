using System;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class PuffRecordDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        // Flask creates timestamp on insert; API GET returns ISO timestamp string
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        // Default to 1 puff unless otherwise specified
        [JsonPropertyName("puff_count")]
        public int PuffCount { get; set; } = 1;

        [JsonPropertyName("mood_before")]
        public string? MoodBefore { get; set; }

        [JsonPropertyName("mood_after")]
        public string? MoodAfter { get; set; }

        [JsonPropertyName("trigger")]
        public string? Trigger { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }
}
