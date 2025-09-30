using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public class AchievementDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; } = default!;

        [JsonPropertyName("target_value")]
        public double TargetValue { get; set; }

        [JsonPropertyName("is_secret")]
        public bool IsSecret { get; set; }
    }
}