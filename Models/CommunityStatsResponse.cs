using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class CommunityStatsResponse
    {
        [JsonPropertyName("total_posts")] public int TotalPosts { get; set; }
        [JsonPropertyName("total_users")] public int TotalUsers { get; set; }

        public sealed class PostsByType
        {
            [JsonPropertyName("checkin")] public int Checkin { get; set; }
            [JsonPropertyName("milestone")] public int Milestone { get; set; }
            [JsonPropertyName("support")] public int Support { get; set; }
            [JsonPropertyName("general")] public int General { get; set; }
        }

        [JsonPropertyName("posts_by_type")] public PostsByType ByType { get; set; } = new();

        public sealed class RecentActivity
        {
            [JsonPropertyName("posts_last_week")] public int PostsLastWeek { get; set; }
        }

        [JsonPropertyName("recent_activity")] public RecentActivity Recent { get; set; } = new();
    }
}
