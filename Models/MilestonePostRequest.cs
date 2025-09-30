using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class MilestonePostRequest
    {
        // "streak" | "reduction" | "quit_date" | other
        [JsonPropertyName("milestone_type")] public string MilestoneType { get; set; } = "streak";
        [JsonPropertyName("milestone_value")] public string MilestoneValue { get; set; } = string.Empty;
        [JsonPropertyName("personal_message")] public string? PersonalMessage { get; set; }
        [JsonPropertyName("is_anonymous")] public bool? IsAnonymous { get; set; }
    }
}
