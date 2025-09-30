using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class DailyRewardResponse
    {
        [JsonPropertyName("reward_points")] public int RewardPoints { get; set; }
        [JsonPropertyName("total_points")] public int TotalPoints { get; set; }
        [JsonPropertyName("new_level")] public int NewLevel { get; set; }
        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    }
}

