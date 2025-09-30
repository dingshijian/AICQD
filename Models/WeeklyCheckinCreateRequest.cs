using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class WeeklyCheckinCreateRequest
    {
        [JsonPropertyName("personal_message")] public string? PersonalMessage { get; set; }
        [JsonPropertyName("challenges")] public string? Challenges { get; set; }
        [JsonPropertyName("victories")] public string? Victories { get; set; }
        [JsonPropertyName("next_week_goals")] public string? NextWeekGoals { get; set; }
        [JsonPropertyName("is_anonymous")] public bool? IsAnonymous { get; set; }
    }
}
