using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class MicroNudgeActionRequest
    {
        [JsonPropertyName("action_taken")] public string ActionTaken { get; set; } = string.Empty;
        [JsonPropertyName("effectiveness_score")] public int EffectivenessScore { get; set; } = 3; // default per backend
    }
}

