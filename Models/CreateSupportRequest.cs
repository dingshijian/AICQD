using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class CreateSupportRequest
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; } = "Looking for Support";
        [JsonPropertyName("situation")] public string Situation { get; set; } = "Going through a tough time right now.";
        [JsonPropertyName("specific_help")] public string SpecificHelp { get; set; } = "Any advice or encouragement would be appreciated.";
        [JsonPropertyName("is_anonymous")] public bool? IsAnonymous { get; set; }
    }
}

