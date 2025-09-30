using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class PuffPatternsDto
    {
        [JsonPropertyName("period_days")] public int PeriodDays { get; set; }
        [JsonPropertyName("total_records")] public int TotalRecords { get; set; }

        // [{ "trigger": "...", "count": 12 }]
        [JsonPropertyName("top_triggers")] public List<NamedCount> TopTriggers { get; set; } = new();

        // [{ "mood": "...", "count": 8 }]
        [JsonPropertyName("top_moods")] public List<MoodCount> TopMoods { get; set; } = new();

        // 24-length array of integers (0..23)
        [JsonPropertyName("hourly_patterns")] public List<int> HourlyPatterns { get; set; } = new();

        public sealed class NamedCount
        {
            [JsonPropertyName("trigger")] public string Trigger { get; set; } = default!;
            [JsonPropertyName("count")] public int Count { get; set; }
        }

        public sealed class MoodCount
        {
            [JsonPropertyName("mood")] public string Mood { get; set; } = default!;
            [JsonPropertyName("count")] public int Count { get; set; }
        }
    }
}

