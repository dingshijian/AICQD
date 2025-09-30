using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class TrendingPostsResponse
    {
        [JsonPropertyName("trending_posts")] public List<CommunityPostDto> TrendingPosts { get; set; } = new();
        [JsonPropertyName("period")] public string Period { get; set; } = "7 days";
    }
}

