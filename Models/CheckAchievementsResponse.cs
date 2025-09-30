using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class CheckAchievementsResponse
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("newly_earned")] public List<AchievementDto> NewlyEarned { get; set; } = new();
        [JsonPropertyName("total_new_points")] public int TotalNewPoints { get; set; }
        [JsonPropertyName("new_level")] public int NewLevel { get; set; }
        [JsonPropertyName("total_points")] public int TotalPoints { get; set; }
    }
}
