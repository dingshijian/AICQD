using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AICQD.Services
{
    public interface IGeminiAIService
    {
        Task<string> GetCoachingResponse(string userMessage, UserContext context);
        Task<string> GetCravingSupport(int cravingLevel, int puffCount);
        Task<string> GetMilestoneMessage(string milestone);
        Task<List<string>> GetMicroNudges(UserBehaviorPattern pattern);
    }

    public class UserContext
    {
        public string UserName { get; set; } = "there";
        public int CurrentStreak { get; set; }
        public int TodayPuffCount { get; set; }
        public int CravingLevel { get; set; }
        public List<string> RecentTriggers { get; set; } = new();
        public DateTime QuitStartDate { get; set; }
    }

    public class UserBehaviorPattern
    {
        public int AverageDailyPuffs { get; set; }
        public List<int> PeakHours { get; set; } = new();
        public List<string> CommonTriggers { get; set; } = new();
        public int StreakDays { get; set; }
    }

    public class GeminiAIService : IGeminiAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeminiAIService> _logger;
        private const string API_KEY = "AIzaSyDsHDhwjTEVSXiids6uZXow2D6r3HNMFmk";
        private const string API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

        public GeminiAIService(HttpClient httpClient, ILogger<GeminiAIService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetCoachingResponse(string userMessage, UserContext context)
        {
            try
            {
                var systemPrompt = BuildCoachingPrompt(context);
                var prompt = $"{systemPrompt}\n\nUser: {userMessage}\n\nAI Coach:";

                return await CallGeminiAPI(prompt);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting coaching response");
                return GetFallbackCoachingResponse(userMessage, context);
            }
        }

        public async Task<string> GetCravingSupport(int cravingLevel, int puffCount)
        {
            try
            {
                var prompt = $@"You are an empathetic quit-smoking AI coach. A user is experiencing a craving at level {cravingLevel}/10. 
They've had {puffCount} puffs today. Provide immediate, compassionate support in 2-3 sentences. 
Be encouraging and suggest a specific coping strategy.";

                return await CallGeminiAPI(prompt);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting craving support");
                return GetFallbackCravingSupport(cravingLevel, puffCount);
            }
        }

        public async Task<string> GetMilestoneMessage(string milestone)
        {
            try
            {
                var prompt = $@"You are a supportive quit-smoking coach. Create a celebratory message for this milestone: {milestone}. 
Make it personal, encouraging, and mention specific health benefits they've gained. Keep it to 2-3 sentences.";

                return await CallGeminiAPI(prompt);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting milestone message");
                return GetFallbackMilestoneMessage(milestone);
            }
        }

        public async Task<List<string>> GetMicroNudges(UserBehaviorPattern pattern)
        {
            try
            {
                var prompt = $@"Generate 3 brief micro-nudge messages (one sentence each) for a user quitting vaping:
- Average daily puffs: {pattern.AverageDailyPuffs}
- Current streak: {pattern.StreakDays} days
- Common triggers: {string.Join(", ", pattern.CommonTriggers)}

Make them actionable, encouraging, and personalized. Format as a JSON array of strings.";

                var response = await CallGeminiAPI(prompt);
                
                // Try to parse JSON response
                try
                {
                    var nudges = JsonSerializer.Deserialize<List<string>>(response);
                    if (nudges != null && nudges.Count > 0)
                        return nudges;
                }
                catch
                {
                    // If JSON parsing fails, split by newlines
                    var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    return new List<string>(lines.Take(3));
                }

                return GetFallbackMicroNudges(pattern);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error getting micro-nudges");
                return GetFallbackMicroNudges(pattern);
            }
        }

        private async Task<string> CallGeminiAPI(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{API_URL}?key={API_KEY}", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObj = JsonSerializer.Deserialize<JsonElement>(responseJson);

            var text = responseObj
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "I'm here to support you!";
        }

        private string BuildCoachingPrompt(UserContext context)
        {
            return $@"You are a compassionate, evidence-based AI coach helping {context.UserName} quit vaping/smoking.

Current Context:
- Current streak: {context.CurrentStreak} days
- Today's puff count: {context.TodayPuffCount}
- Craving level: {context.CravingLevel}/10
- Recent triggers: {string.Join(", ", context.RecentTriggers)}
- Started quit journey: {context.QuitStartDate:MMM dd, yyyy}

Your approach:
1. Be warm, supportive, and non-judgmental
2. Acknowledge struggles without enabling
3. Provide evidence-based strategies
4. Celebrate progress, no matter how small
5. Keep responses conversational and under 3 sentences
6. Use specific data from their journey
7. Offer actionable advice

Respond naturally as their supportive coach.";
        }

        // Fallback responses when API is unavailable
        private string GetFallbackCoachingResponse(string userMessage, UserContext context)
        {
            var responses = new[]
            {
                $"I hear you, {context.UserName}. With {context.CurrentStreak} days behind you, you've already proven your strength. What would help most right now?",
                $"You're doing great tracking your journey - {context.TodayPuffCount} puffs today shows real awareness. Let's work through this moment together.",
                $"Every craving you ride out is building your resilience. You've got {context.CurrentStreak} days of proof that you can do this!",
                "I'm here to support you through this. Would a breathing exercise or a quick game help right now?",
                "Remember why you started this journey. You're not just quitting - you're reclaiming your health and freedom."
            };

            return responses[new Random().Next(responses.Length)];
        }

        private string GetFallbackCravingSupport(int cravingLevel, int puffCount)
        {
            if (cravingLevel >= 8)
                return $"🚨 I know this craving feels overwhelming. Take 5 deep breaths with me - in for 4, hold for 4, out for 4. This will pass in 3-5 minutes. You've got this!";
            else if (cravingLevel >= 5)
                return $"💪 Craving hitting hard? Try the 4Ds: Delay (wait 10 min), Distract (play a game), Deep breathe, Drink water. You've handled {puffCount} moments today - you can handle this one too!";
            else
                return $"👍 This is manageable! A quick walk, some water, or a craving game can help. You're doing great with {puffCount} tracked today.";
        }

        private string GetFallbackMilestoneMessage(string milestone)
        {
            return $"🎉 Congratulations on {milestone}! This is a huge achievement in your quit journey. Your body is healing, your lungs are clearing, and you're proving to yourself that you're stronger than any craving. Keep going - you're amazing!";
        }

        private List<string> GetFallbackMicroNudges(UserBehaviorPattern pattern)
        {
            return new List<string>
            {
                $"🌟 {pattern.StreakDays} days strong! Every day without vaping is a victory for your health.",
                "💧 Hydration check! Drinking water helps reduce cravings and flushes toxins.",
                $"🎮 Craving alert time! Your peak hours are coming up - ready with a game?"
            };
        }
    }
}