using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class UserAchievementsSummary
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("total_achievements")] public int TotalAchievements { get; set; }
        [JsonPropertyName("total_points")] public int TotalPoints { get; set; }
        [JsonPropertyName("level")] public int Level { get; set; }
        [JsonPropertyName("achievements")] public List<UserAchievementDto> Achievements { get; set; } = new();
    }
}
