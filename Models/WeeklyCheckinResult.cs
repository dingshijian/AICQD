using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class WeeklyCheckinResult
    {
        [JsonPropertyName("post")] public CommunityPostDto Post { get; set; } = new();

        public sealed class WeeklyStats
        {
            [JsonPropertyName("streak_days")] public int StreakDays { get; set; }
            [JsonPropertyName("weekly_puffs")] public int WeeklyPuffs { get; set; }
            [JsonPropertyName("level")] public int Level { get; set; }
            [JsonPropertyName("points")] public int Points { get; set; }
        }

        [JsonPropertyName("weekly_stats")] public WeeklyStats Stats { get; set; } = new();
    }
}

