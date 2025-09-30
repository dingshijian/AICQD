using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class TodayPuffsDto
    {
        [JsonPropertyName("date")] public string Date { get; set; } = default!;
        [JsonPropertyName("total_puffs")] public int TotalPuffs { get; set; }
        [JsonPropertyName("target_puffs")] public int TargetPuffs { get; set; }
        [JsonPropertyName("records")] public List<PuffRecordDto> Records { get; set; } = new();
    }
}

