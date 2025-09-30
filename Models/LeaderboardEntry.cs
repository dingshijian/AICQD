using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class LeaderboardEntry
    {
        [JsonPropertyName("rank")] public int Rank { get; set; }
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
        [JsonPropertyName("points")] public int Points { get; set; }
        [JsonPropertyName("level")] public int Level { get; set; }
        [JsonPropertyName("current_streak")] public int CurrentStreak { get; set; }
        [JsonPropertyName("longest_streak")] public int LongestStreak { get; set; }
        [JsonPropertyName("is_current_user")] public bool IsCurrentUser { get; set; }
    }

    public sealed class LeaderboardResponse
    {
        [JsonPropertyName("leaderboard")] public System.Collections.Generic.List<LeaderboardEntry> Leaderboard { get; set; } = new();
        [JsonPropertyName("current_user_rank")] public int CurrentUserRank { get; set; }
        [JsonPropertyName("current_user_points")] public int CurrentUserPoints { get; set; }
        [JsonPropertyName("total_users")] public int TotalUsers { get; set; }
    }
}
