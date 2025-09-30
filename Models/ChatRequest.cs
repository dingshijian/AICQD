namespace AICQD.Models
{
    public sealed class ChatRequest
    {
        public string Message { get; set; } = string.Empty; // POST body: "message"
        public string Type { get; set; } = "general";       // "general" | "craving" | "milestone" | "checkin"
    }
}
