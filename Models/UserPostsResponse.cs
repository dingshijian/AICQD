using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class UserPostsResponse
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
        [JsonPropertyName("posts")] public List<CommunityPostDto> Posts { get; set; } = new();
        [JsonPropertyName("total_posts")] public int TotalPosts { get; set; }
    }
}
