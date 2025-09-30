using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class AiMessageRespondRequest
    {
        [JsonPropertyName("response")] public string Response { get; set; } = string.Empty;
    }
}
