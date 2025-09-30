using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class CommunityPageResponse
    {
        [JsonPropertyName("posts")] public List<CommunityPostDto> Posts { get; set; } = new();
        [JsonPropertyName("total")] public int Total { get; set; }
        [JsonPropertyName("pages")] public int Pages { get; set; }
        [JsonPropertyName("current_page")] public int CurrentPage { get; set; }
        [JsonPropertyName("per_page")] public int PerPage { get; set; }
        [JsonPropertyName("has_next")] public bool HasNext { get; set; }
        [JsonPropertyName("has_prev")] public bool HasPrev { get; set; }
    }
}

