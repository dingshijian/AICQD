using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class EmergencySupportResponse
    {
        [JsonPropertyName("conversation_id")] 
        public int? ConversationId { get; set; }
        
        [JsonPropertyName("ai_response")] 
        public string AiResponse { get; set; } = default!;
        
        [JsonPropertyName("emergency_tips")] 
        public List<string> EmergencyTips { get; set; } = new();
        
        [JsonPropertyName("timestamp")] 
        public string? Timestamp { get; set; }
        
        [JsonPropertyName("error")] 
        public string? Error { get; set; }
    }
}