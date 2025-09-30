using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class LikePostResponse
    {
        [JsonPropertyName("post_id")] public int PostId { get; set; }
        [JsonPropertyName("likes_count")] public int LikesCount { get; set; }
        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    }
}
