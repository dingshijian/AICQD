using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class TriggerNudgesResult
    {
        [JsonPropertyName("status")] public string Status { get; set; } = "success";
        [JsonPropertyName("nudges_created")] public int NudgesCreated { get; set; }
        [JsonPropertyName("nudges")] public List<MicroNudgeDto> Nudges { get; set; } = new();
    }
}

