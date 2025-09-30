using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class AchievementDetailsResponse
    {
        public sealed class RecentEarner
        {
            [JsonPropertyName("user_id")] public int UserId { get; set; }
            [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
            [JsonPropertyName("earned_at")] public DateTime EarnedAt { get; set; }
        }

        [JsonPropertyName("achievement")] public AchievementDto Achievement { get; set; } = new();
        [JsonPropertyName("total_earned_by")] public int TotalEarnedBy { get; set; }
        [JsonPropertyName("recent_earners")] public List<RecentEarner> RecentEarners { get; set; } = new();
    }
}

