using System;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    public sealed class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime? Created_At { get; set; }

        // AICQD fields
        public DateTime? Quit_Date { get; set; }
        public string Substance_Type { get; set; } = "vaping";  // 'vaping' or 'smoking'
        public int Daily_Target_Puffs { get; set; }
        public int Current_Streak_Days { get; set; }
        public int Longest_Streak_Days { get; set; }
        public int Total_Points { get; set; }
        public int Level { get; set; } = 1;
        public string? Motivation_Reason { get; set; }

        // AI texting prefs
        public bool Enable_Ai_Texting { get; set; } = true;
        public bool Enable_Micro_Nudges { get; set; } = true;
        public string Preferred_Contact_Times { get; set; } = "09:00,13:00,18:00";
        public string Timezone { get; set; } = "UTC";
        public DateTime? Last_Ai_Message_Sent { get; set; }
        public string Ai_Message_Frequency { get; set; } = "daily"; // 'hourly','daily','weekly'
    }
}
