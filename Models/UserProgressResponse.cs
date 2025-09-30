using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class UserProgressResponse
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("progress_towards_achievements")] public List<AchievementProgressEntry> Progress { get; set; } = new();
        [JsonPropertyName("current_level")] public int CurrentLevel { get; set; }
        [JsonPropertyName("current_points")] public int CurrentPoints { get; set; }
        [JsonPropertyName("points_to_next_level")] public int PointsToNextLevel { get; set; }
    }
}
