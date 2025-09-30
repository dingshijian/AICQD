using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class GenerateSmartMessagesResult
    {
        [JsonPropertyName("status")] public string Status { get; set; } = "success";
        [JsonPropertyName("messages_created")] public int MessagesCreated { get; set; }
        [JsonPropertyName("messages")] public List<AIMessageDto> Messages { get; set; } = new();
    }
}

