using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class DailyCheckinResponse
    {
        [JsonPropertyName("conversation_id")] public int ConversationId { get; set; }
        [JsonPropertyName("ai_response")] public string AiResponse { get; set; } = default!;
        [JsonPropertyName("timestamp")] public string Timestamp { get; set; } = default!;

        public sealed class TodayStatsDto
        {
            [JsonPropertyName("puffs")] public int Puffs { get; set; }
            [JsonPropertyName("target")] public int Target { get; set; }
            [JsonPropertyName("streak")] public int Streak { get; set; }
        }

        [JsonPropertyName("today_stats")] public TodayStatsDto TodayStats { get; set; } = new();
    }
}

