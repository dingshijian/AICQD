using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public class UserAchievementDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; } = default!;

        [JsonPropertyName("achieved_at")]
        public string? AchievedAt { get; set; }

        [JsonPropertyName("progress")]
        public double Progress { get; set; }

        [JsonPropertyName("target")]
        public double Target { get; set; }

        [JsonPropertyName("is_achieved")]
        public bool IsAchieved { get; set; }
    }
}