using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class AchievementProgressEntry
    {
        [JsonPropertyName("achievement")] public AchievementDto Achievement { get; set; } = new();
        [JsonPropertyName("current_value")] public double CurrentValue { get; set; }
        [JsonPropertyName("required_value")] public double RequiredValue { get; set; }
        [JsonPropertyName("progress_percentage")] public double ProgressPercentage { get; set; }
    }
}

