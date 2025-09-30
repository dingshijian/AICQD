using System.Text.Json.Serialization;
using System;

namespace AICQD.Models
{
    public sealed class CommunityPostDto
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("username")] public string Username { get; set; } = default!;
        [JsonPropertyName("title")] public string Title { get; set; } = default!;
        [JsonPropertyName("content")] public string Content { get; set; } = default!;
        [JsonPropertyName("post_type")] public string PostType { get; set; } = "general";
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("likes_count")] public int LikesCount { get; set; }
        [JsonPropertyName("is_anonymous")] public bool IsAnonymous { get; set; }
    }
}
