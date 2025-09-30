using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class EmergencySupportRequest
    {
        [JsonPropertyName("emergency_type")]
        public string EmergencyType { get; set; } = default!;
        
        [JsonPropertyName("location")]
        public string Location { get; set; } = default!;
        
        [JsonPropertyName("details")]
        public string Details { get; set; } = default!;
        
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = default!;
        
        [JsonPropertyName("device_info")]
        public string DeviceInfo { get; set; } = default!;
        
        [JsonPropertyName("coordinates")]
        public string Coordinates { get; set; } = default!;
    }
}