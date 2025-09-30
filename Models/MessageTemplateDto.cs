using System.Text.Json.Serialization;
using System;

namespace AICQD.Models
{
    public sealed class MessageTemplateDto
    {
        public int Id { get; set; }
        [JsonPropertyName("template_name")] public string TemplateName { get; set; } = default!;
        [JsonPropertyName("message_type")] public string MessageType { get; set; } = default!;
        [JsonPropertyName("content_template")] public string ContentTemplate { get; set; } = default!;
        [JsonPropertyName("trigger_conditions")] public string? TriggerConditions { get; set; } // JSON string
        [JsonPropertyName("personalization_fields")] public string? PersonalizationFields { get; set; } // JSON string
        [JsonPropertyName("is_active")] public bool IsActive { get; set; } = true;
        [JsonPropertyName("usage_count")] public int UsageCount { get; set; }
        [JsonPropertyName("effectiveness_rating")] public double EffectivenessRating { get; set; }
        [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")] public DateTime UpdatedAt { get; set; }
    }
}
