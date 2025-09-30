using System;

namespace AICQD.Models
{
    public sealed class PuffEvent
    {
        public DateTime Time { get; set; }
        public string Title { get; set; } = "Puff";
        public string? Note { get; set; }
        public int Intensity { get; set; } // optional
    }
}
