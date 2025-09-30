using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class WeeklyStatsDto
    {
        [JsonPropertyName("start_date")] public string StartDate { get; set; } = default!;
        [JsonPropertyName("end_date")] public string EndDate { get; set; } = default!;
        [JsonPropertyName("total_puffs")] public int TotalPuffs { get; set; }
        [JsonPropertyName("average_daily")] public double AverageDaily { get; set; }
        [JsonPropertyName("daily_stats")] public List<DailyStat> DailyStats { get; set; } = new();

        public sealed class DailyStat
        {
            [JsonPropertyName("date")] public string Date { get; set; } = default!;
            [JsonPropertyName("puff_count")] public int PuffCount { get; set; }
            [JsonPropertyName("records")] public List<PuffRecordDto> Records { get; set; } = new();
        }
    }
}

