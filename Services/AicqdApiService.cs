using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AICQD.Config;
using AICQD.Models;

namespace AICQD.Services
{
    public interface IAicqdApiService
    {
        // -------- Health --------
        Task<ApiResult<HealthResponse>> HealthAsync(CancellationToken ct = default);

        // -------- Users --------
        Task<ApiResult<UserDto>> CreateUserAsync(UserDto payload, CancellationToken ct = default);
        Task<ApiResult<UserDto>> GetUserAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<UserDto>> UpdateUserAsync(int userId, object partialUpdate, CancellationToken ct = default);

        // -------- Puffs --------
        Task<ApiResult<PuffRecordDto>> CreatePuffAsync(int userId, PuffRecordDto payload, CancellationToken ct = default);
        Task<ApiResult<List<PuffRecordDto>>> ListPuffsAsync(int userId, DateTime? start = null, DateTime? end = null, int? limit = 100, CancellationToken ct = default);
        Task<ApiResult<TodayPuffsDto>> GetTodayPuffsAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<WeeklyStatsDto>> GetWeeklyStatsAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<PuffPatternsDto>> GetPuffPatternsAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<PuffRecordDto>> UpdatePuffAsync(int userId, int puffId, object partialUpdate, CancellationToken ct = default);
        Task<ApiResult<bool>> DeletePuffAsync(int userId, int puffId, CancellationToken ct = default);

        // -------- AI Coach (chat/check-in/emergency) --------
        Task<ApiResult<ChatResponse>> ChatWithAiAsync(int userId, ChatRequest body, CancellationToken ct = default);
        Task<ApiResult<List<ConversationDto>>> GetConversationsAsync(int userId, string? type = null, int limit = 50, CancellationToken ct = default);
        Task<ApiResult<ConversationDto>> GetConversationAsync(int userId, int conversationId, CancellationToken ct = default);
        Task<ApiResult<EmergencySupportResponse>> EmergencySupportAsync(int userId, EmergencySupportRequest body, CancellationToken ct = default);
        Task<ApiResult<DailyCheckinResponse>> DailyCheckinAsync(int userId, CancellationToken ct = default);

        // -------- Community --------
        Task<ApiResult<CommunityPageResponse>> GetCommunityPostsPageAsync(int page = 1, int perPage = 20, string? type = null, CancellationToken ct = default);
        Task<ApiResult<CommunityPostDto>> CreateCommunityPostAsync(CommunityPostDto payload, CancellationToken ct = default);
        Task<ApiResult<CommunityPostDto>> GetCommunityPostAsync(int postId, CancellationToken ct = default);
        Task<ApiResult<CommunityPostDto>> UpdateCommunityPostAsync(int postId, int userId, object patch, CancellationToken ct = default);
        Task<ApiResult<bool>> DeleteCommunityPostAsync(int postId, int userId, CancellationToken ct = default);
        Task<ApiResult<LikePostResponse>> LikeCommunityPostAsync(int postId, int userId, CancellationToken ct = default);
        Task<ApiResult<UserPostsResponse>> GetUserPostsAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<WeeklyCheckinResult>> CreateWeeklyCheckinAsync(int userId, WeeklyCheckinCreateRequest body, CancellationToken ct = default);
        Task<ApiResult<CommunityPostDto>> CreateMilestonePostAsync(int userId, MilestonePostRequest body, CancellationToken ct = default);
        Task<ApiResult<CommunityPostDto>> CreateSupportRequestAsync(CreateSupportRequest body, CancellationToken ct = default);
        Task<ApiResult<TrendingPostsResponse>> GetTrendingPostsAsync(CancellationToken ct = default);
        Task<ApiResult<CommunityStatsResponse>> GetCommunityStatsAsync(CancellationToken ct = default);

        // -------- Templates --------
        Task<ApiResult<List<MessageTemplateDto>>> GetTemplatesAsync(string? type = null, CancellationToken ct = default);

        // ==============================
        // Achievements (expanded)
        // ==============================
        Task<ApiResult<List<AchievementDto>>> GetAllAchievementsAsync(CancellationToken ct = default);
        Task<ApiResult<UserAchievementsSummary>> GetUserAchievementsSummaryAsync(int userId, CancellationToken ct = default);
        // Back-compat sugar: extracts `achievements` array from the summary
        Task<ApiResult<List<UserAchievementDto>>> GetUserAchievementsAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<CheckAchievementsResponse>> CheckAndAwardAchievementsAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<UserProgressResponse>> GetUserAchievementProgressAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<LeaderboardResponse>> GetLeaderboardAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<AchievementDetailsResponse>> GetAchievementDetailsAsync(int achievementId, CancellationToken ct = default);
        Task<ApiResult<DailyRewardResponse>> ClaimDailyRewardAsync(int userId, CancellationToken ct = default);

        // ===== ai_texting.py =====
        // AI Messages
        Task<ApiResult<List<AIMessageDto>>> GetAiMessagesAsync(int userId, int limit = 20, string? type = null, bool unreadOnly = false, CancellationToken ct = default);
        Task<ApiResult<AIMessageDto>> CreateAiMessageAsync(int userId, AiMessageCreateRequest body, CancellationToken ct = default);
        Task<ApiResult<AiMessageReadResponse>> MarkAiMessageReadAsync(int userId, int messageId, CancellationToken ct = default);
        Task<ApiResult<AiMessageRespondResponse>> RespondToAiMessageAsync(int userId, int messageId, AiMessageRespondRequest body, CancellationToken ct = default);

        // Micro-nudges
        Task<ApiResult<List<MicroNudgeDto>>> GetMicroNudgesAsync(int userId, int limit = 10, string? type = null, bool activeOnly = true, CancellationToken ct = default);
        Task<ApiResult<MicroNudgeDto>> CreateMicroNudgeAsync(int userId, MicroNudgeCreateRequest body, CancellationToken ct = default);
        Task<ApiResult<SimpleStatusResponse>> DismissMicroNudgeAsync(int userId, int nudgeId, CancellationToken ct = default);
        Task<ApiResult<SimpleStatusResponse>> RecordMicroNudgeActionAsync(int userId, int nudgeId, MicroNudgeActionRequest body, CancellationToken ct = default);

        // Notification preferences
        Task<ApiResult<NotificationPreferencesDto>> GetNotificationPreferencesAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<SimpleStatusResponse>> UpdateNotificationPreferencesAsync(int userId, NotificationPreferencesDto body, CancellationToken ct = default);

        // Smart generators
        Task<ApiResult<GenerateSmartMessagesResult>> GenerateSmartMessagesAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<TriggerNudgesResult>> TriggerMicroNudgesAsync(int userId, CancellationToken ct = default);

        // ==============================
        // Music (music_creation.py)
        // ==============================
        Task<ApiResult<MusicGenerateResponse>> GenerateMusicAsync(MusicGenerateRequest body, CancellationToken ct = default);
        Task<ApiResult<MusicLibraryResponse>> GetMusicLibraryAsync(int userId, CancellationToken ct = default);
        Task<ApiResult<bool>> DeleteMusicAsync(int userId, string musicId, CancellationToken ct = default);
        Task<ApiResult<MusicPresetsResponse>> GetMusicPresetsAsync(CancellationToken ct = default);
        Task<ApiResult<MusicStatsResponse>> GetMusicStatsAsync(int userId, CancellationToken ct = default);

        // URL helpers for streaming/downloading tracks
        string BuildMusicStreamUrl(string relativeStreamUrl, int userId);
        string BuildMusicDownloadUrl(string relativeDownloadUrl, int userId);
    }

    public sealed class AicqdApiService : IAicqdApiService
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true
        };

        public AicqdApiService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress ??= new Uri(AppSettings.ApiBaseUrl);
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        // ---------- helpers ----------
        private static ApiResult<T> Fail<T>(string msg) => ApiResult<T>.FromError(msg);

        private async Task<ApiResult<T>> SendJsonAsync<T>(HttpMethod method, string url, object? payload, CancellationToken ct)
        {
            try
            {
                using var req = new HttpRequestMessage(method, url);
                if (payload is not null)
                {
                    var json = JsonSerializer.Serialize(payload, JsonOpts);
                    req.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                using var res = await _http.SendAsync(req, ct);
                var text = await res.Content.ReadAsStringAsync(ct);
                if (!res.IsSuccessStatusCode) return Fail<T>($"{(int)res.StatusCode} {text}");

                if (typeof(T) == typeof(bool))
                    return ApiResult<T>.FromData((T)(object)true);

                var data = JsonSerializer.Deserialize<T>(text, JsonOpts);
                return data is null ? Fail<T>("Empty response") : ApiResult<T>.FromData(data);
            }
            catch (Exception ex) { return Fail<T>(ex.Message); }
        }

        private static string Q(params (string key, string? val)[] kv)
        {
            var parts = new List<string>();
            foreach (var (k, v) in kv)
                if (!string.IsNullOrWhiteSpace(v))
                    parts.Add($"{Uri.EscapeDataString(k)}={Uri.EscapeDataString(v)}");
            return parts.Count == 0 ? "" : "?" + string.Join("&", parts);
        }

        private static object Merge(object a, object b)
        {
            var da = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(a)) ?? new();
            var db = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(b)) ?? new();
            foreach (var kv in db) da[kv.Key] = kv.Value;
            return da;
        }

        // ---------- Health ----------
        public Task<ApiResult<HealthResponse>> HealthAsync(CancellationToken ct = default)
            => SendJsonAsync<HealthResponse>(HttpMethod.Get, "/api/health", null, ct);

        // ---------- Users ----------
        public Task<ApiResult<UserDto>> CreateUserAsync(UserDto payload, CancellationToken ct = default)
            => SendJsonAsync<UserDto>(HttpMethod.Post, "/api/users", payload, ct);

        public Task<ApiResult<UserDto>> GetUserAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<UserDto>(HttpMethod.Get, $"/api/users/{userId}", null, ct);

        public Task<ApiResult<UserDto>> UpdateUserAsync(int userId, object partialUpdate, CancellationToken ct = default)
            => SendJsonAsync<UserDto>(HttpMethod.Patch, $"/api/users/{userId}", partialUpdate, ct);

        // ---------- Puffs ----------
        public Task<ApiResult<PuffRecordDto>> CreatePuffAsync(int userId, PuffRecordDto payload, CancellationToken ct = default)
            => SendJsonAsync<PuffRecordDto>(HttpMethod.Post, $"/api/users/{userId}/puffs", payload, ct);

        public Task<ApiResult<List<PuffRecordDto>>> ListPuffsAsync(int userId, DateTime? start = null, DateTime? end = null, int? limit = 100, CancellationToken ct = default)
            => SendJsonAsync<List<PuffRecordDto>>(HttpMethod.Get,
                $"/api/users/{userId}/puffs" + Q(
                    ("start_date", start?.ToUniversalTime().ToString("o")),
                    ("end_date", end?.ToUniversalTime().ToString("o")),
                    ("limit", limit?.ToString())), null, ct);

        public Task<ApiResult<TodayPuffsDto>> GetTodayPuffsAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<TodayPuffsDto>(HttpMethod.Get, $"/api/users/{userId}/puffs/today", null, ct);

        public Task<ApiResult<WeeklyStatsDto>> GetWeeklyStatsAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<WeeklyStatsDto>(HttpMethod.Get, $"/api/users/{userId}/puffs/weekly-stats", null, ct);

        public Task<ApiResult<PuffPatternsDto>> GetPuffPatternsAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<PuffPatternsDto>(HttpMethod.Get, $"/api/users/{userId}/puffs/patterns", null, ct);

        public Task<ApiResult<PuffRecordDto>> UpdatePuffAsync(int userId, int puffId, object partialUpdate, CancellationToken ct = default)
            => SendJsonAsync<PuffRecordDto>(HttpMethod.Put, $"/api/users/{userId}/puffs/{puffId}", partialUpdate, ct);

        public Task<ApiResult<bool>> DeletePuffAsync(int userId, int puffId, CancellationToken ct = default)
            => SendJsonAsync<bool>(HttpMethod.Delete, $"/api/users/{userId}/puffs/{puffId}", null, ct);

        // ---------- AI Coach (chat/check-in/emergency) ----------
        public Task<ApiResult<ChatResponse>> ChatWithAiAsync(int userId, ChatRequest body, CancellationToken ct = default)
            => SendJsonAsync<ChatResponse>(HttpMethod.Post, $"/api/users/{userId}/chat", body, ct);

        public Task<ApiResult<List<ConversationDto>>> GetConversationsAsync(int userId, string? type = null, int limit = 50, CancellationToken ct = default)
            => SendJsonAsync<List<ConversationDto>>(HttpMethod.Get, $"/api/users/{userId}/conversations" + Q(("limit", limit.ToString()), ("type", type)), null, ct);

        public Task<ApiResult<ConversationDto>> GetConversationAsync(int userId, int conversationId, CancellationToken ct = default)
            => SendJsonAsync<ConversationDto>(HttpMethod.Get, $"/api/users/{userId}/conversations/{conversationId}", null, ct);

        public Task<ApiResult<EmergencySupportResponse>> EmergencySupportAsync(int userId, EmergencySupportRequest body, CancellationToken ct = default)
            => SendJsonAsync<EmergencySupportResponse>(HttpMethod.Post, $"/api/users/{userId}/emergency-support", body, ct);

        public Task<ApiResult<DailyCheckinResponse>> DailyCheckinAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<DailyCheckinResponse>(HttpMethod.Post, $"/api/users/{userId}/daily-checkin", null, ct);

        // ---------- Community ----------
        public Task<ApiResult<CommunityPageResponse>> GetCommunityPostsPageAsync(int page = 1, int perPage = 20, string? type = null, CancellationToken ct = default)
            => SendJsonAsync<CommunityPageResponse>(HttpMethod.Get, "/api/community/posts" + Q(("page", page.ToString()), ("per_page", perPage.ToString()), ("type", type)), null, ct);

        public Task<ApiResult<CommunityPostDto>> CreateCommunityPostAsync(CommunityPostDto payload, CancellationToken ct = default)
            => SendJsonAsync<CommunityPostDto>(HttpMethod.Post, "/api/community/posts", payload, ct);

        public Task<ApiResult<CommunityPostDto>> GetCommunityPostAsync(int postId, CancellationToken ct = default)
            => SendJsonAsync<CommunityPostDto>(HttpMethod.Get, $"/api/community/posts/{postId}", null, ct);

        public Task<ApiResult<CommunityPostDto>> UpdateCommunityPostAsync(int postId, int userId, object patch, CancellationToken ct = default)
            => SendJsonAsync<CommunityPostDto>(HttpMethod.Put, $"/api/community/posts/{postId}", Merge(patch, new { user_id = userId }), ct);

        public Task<ApiResult<bool>> DeleteCommunityPostAsync(int postId, int userId, CancellationToken ct = default)
            => SendJsonAsync<bool>(HttpMethod.Delete, $"/api/community/posts/{postId}", new { user_id = userId }, ct);

        public Task<ApiResult<LikePostResponse>> LikeCommunityPostAsync(int postId, int userId, CancellationToken ct = default)
            => SendJsonAsync<LikePostResponse>(HttpMethod.Post, $"/api/community/posts/{postId}/like", new { user_id = userId }, ct);

        public Task<ApiResult<UserPostsResponse>> GetUserPostsAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<UserPostsResponse>(HttpMethod.Get, $"/api/users/{userId}/posts", null, ct);

        public Task<ApiResult<WeeklyCheckinResult>> CreateWeeklyCheckinAsync(int userId, WeeklyCheckinCreateRequest body, CancellationToken ct = default)
            => SendJsonAsync<WeeklyCheckinResult>(HttpMethod.Post, $"/api/users/{userId}/weekly-checkin", body, ct);

        public Task<ApiResult<CommunityPostDto>> CreateMilestonePostAsync(int userId, MilestonePostRequest body, CancellationToken ct = default)
            => SendJsonAsync<CommunityPostDto>(HttpMethod.Post, $"/api/users/{userId}/milestone-post", body, ct);

        public Task<ApiResult<CommunityPostDto>> CreateSupportRequestAsync(CreateSupportRequest body, CancellationToken ct = default)
            => SendJsonAsync<CommunityPostDto>(HttpMethod.Post, "/api/community/support-request", body, ct);

        public Task<ApiResult<TrendingPostsResponse>> GetTrendingPostsAsync(CancellationToken ct = default)
            => SendJsonAsync<TrendingPostsResponse>(HttpMethod.Get, "/api/community/trending", null, ct);

        public Task<ApiResult<CommunityStatsResponse>> GetCommunityStatsAsync(CancellationToken ct = default)
            => SendJsonAsync<CommunityStatsResponse>(HttpMethod.Get, "/api/community/stats", null, ct);

        // ---------- Templates ----------
        public Task<ApiResult<List<MessageTemplateDto>>> GetTemplatesAsync(string? type = null, CancellationToken ct = default)
            => SendJsonAsync<List<MessageTemplateDto>>(HttpMethod.Get, "/api/templates" + Q(("type", type)), null, ct);

        // ==============================
        // Achievements (expanded)
        // ==============================
        public Task<ApiResult<List<AchievementDto>>> GetAllAchievementsAsync(CancellationToken ct = default)
            => SendJsonAsync<List<AchievementDto>>(HttpMethod.Get, "/api/achievements", null, ct);

        public Task<ApiResult<UserAchievementsSummary>> GetUserAchievementsSummaryAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<UserAchievementsSummary>(HttpMethod.Get, $"/api/users/{userId}/achievements", null, ct);

        public async Task<ApiResult<List<UserAchievementDto>>> GetUserAchievementsAsync(int userId, CancellationToken ct = default)
        {
            var res = await GetUserAchievementsSummaryAsync(userId, ct);
            if (!res.Ok || res.Data is null)
                return ApiResult<List<UserAchievementDto>>.FromError(res.Error ?? "Unknown error");
            return ApiResult<List<UserAchievementDto>>.FromData(res.Data.Achievements);
        }

        public Task<ApiResult<CheckAchievementsResponse>> CheckAndAwardAchievementsAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<CheckAchievementsResponse>(HttpMethod.Post, $"/api/users/{userId}/check-achievements", null, ct);

        public Task<ApiResult<UserProgressResponse>> GetUserAchievementProgressAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<UserProgressResponse>(HttpMethod.Get, $"/api/users/{userId}/progress", null, ct);

        public Task<ApiResult<LeaderboardResponse>> GetLeaderboardAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<LeaderboardResponse>(HttpMethod.Get, $"/api/users/{userId}/leaderboard", null, ct);

        public Task<ApiResult<AchievementDetailsResponse>> GetAchievementDetailsAsync(int achievementId, CancellationToken ct = default)
            => SendJsonAsync<AchievementDetailsResponse>(HttpMethod.Get, $"/api/achievements/{achievementId}", null, ct);

        public Task<ApiResult<DailyRewardResponse>> ClaimDailyRewardAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<DailyRewardResponse>(HttpMethod.Post, $"/api/users/{userId}/daily-reward", null, ct);

        // ==============================
        // AI Texting (ai_texting.py)
        // ==============================
        public Task<ApiResult<List<AIMessageDto>>> GetAiMessagesAsync(int userId, int limit = 20, string? type = null, bool unreadOnly = false, CancellationToken ct = default)
            => SendJsonAsync<List<AIMessageDto>>(HttpMethod.Get, $"/api/users/{userId}/ai-messages" + Q(("limit", limit.ToString()), ("type", type), ("unread_only", unreadOnly.ToString())), null, ct);

        public Task<ApiResult<AIMessageDto>> CreateAiMessageAsync(int userId, AiMessageCreateRequest body, CancellationToken ct = default)
            => SendJsonAsync<AIMessageDto>(HttpMethod.Post, $"/api/users/{userId}/ai-messages", body, ct);

        public Task<ApiResult<AiMessageReadResponse>> MarkAiMessageReadAsync(int userId, int messageId, CancellationToken ct = default)
            => SendJsonAsync<AiMessageReadResponse>(HttpMethod.Post, $"/api/users/{userId}/ai-messages/{messageId}/read", null, ct);

        public Task<ApiResult<AiMessageRespondResponse>> RespondToAiMessageAsync(int userId, int messageId, AiMessageRespondRequest body, CancellationToken ct = default)
            => SendJsonAsync<AiMessageRespondResponse>(HttpMethod.Post, $"/api/users/{userId}/ai-messages/{messageId}/respond", body, ct);

        public Task<ApiResult<List<MicroNudgeDto>>> GetMicroNudgesAsync(int userId, int limit = 10, string? type = null, bool activeOnly = true, CancellationToken ct = default)
            => SendJsonAsync<List<MicroNudgeDto>>(HttpMethod.Get, $"/api/users/{userId}/micro-nudges" + Q(("limit", limit.ToString()), ("type", type), ("active_only", activeOnly.ToString())), null, ct);



        public Task<ApiResult<MicroNudgeDto>> CreateMicroNudgeAsync(int userId, MicroNudgeCreateRequest body, CancellationToken ct = default)
            => SendJsonAsync<MicroNudgeDto>(HttpMethod.Post, $"/api/users/{userId}/micro-nudges", body, ct);

        public Task<ApiResult<SimpleStatusResponse>> DismissMicroNudgeAsync(int userId, int nudgeId, CancellationToken ct = default)
            => SendJsonAsync<SimpleStatusResponse>(HttpMethod.Post, $"/api/users/{userId}/micro-nudges/{nudgeId}/dismiss", null, ct);

        public Task<ApiResult<SimpleStatusResponse>> RecordMicroNudgeActionAsync(int userId, int nudgeId, MicroNudgeActionRequest body, CancellationToken ct = default)
            => SendJsonAsync<SimpleStatusResponse>(HttpMethod.Post, $"/api/users/{userId}/micro-nudges/{nudgeId}/action", body, ct);

        public Task<ApiResult<NotificationPreferencesDto>> GetNotificationPreferencesAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<NotificationPreferencesDto>(HttpMethod.Get, $"/api/users/{userId}/notification-preferences", null, ct);

        public Task<ApiResult<SimpleStatusResponse>> UpdateNotificationPreferencesAsync(int userId, NotificationPreferencesDto body, CancellationToken ct = default)
            => SendJsonAsync<SimpleStatusResponse>(HttpMethod.Put, $"/api/users/{userId}/notification-preferences", body, ct);

        public Task<ApiResult<GenerateSmartMessagesResult>> GenerateSmartMessagesAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<GenerateSmartMessagesResult>(HttpMethod.Post, $"/api/users/{userId}/generate-smart-messages", null, ct);

        public Task<ApiResult<TriggerNudgesResult>> TriggerMicroNudgesAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<TriggerNudgesResult>(HttpMethod.Post, $"/api/users/{userId}/trigger-micro-nudges", null, ct);


        // ==============================
        // Music (music_creation.py)
        // ==============================
        public Task<ApiResult<MusicGenerateResponse>> GenerateMusicAsync(MusicGenerateRequest body, CancellationToken ct = default)
            => SendJsonAsync<MusicGenerateResponse>(HttpMethod.Post, "/api/music/generate", body, ct);

        public Task<ApiResult<MusicLibraryResponse>> GetMusicLibraryAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<MusicLibraryResponse>(HttpMethod.Get, $"/api/users/{userId}/music", null, ct);

        public Task<ApiResult<bool>> DeleteMusicAsync(int userId, string musicId, CancellationToken ct = default)
            => SendJsonAsync<bool>(HttpMethod.Delete, $"/api/users/{userId}/music/{Uri.EscapeDataString(musicId)}", null, ct);

        public Task<ApiResult<MusicPresetsResponse>> GetMusicPresetsAsync(CancellationToken ct = default)
            => SendJsonAsync<MusicPresetsResponse>(HttpMethod.Get, "/api/music/presets", null, ct);

        public Task<ApiResult<MusicStatsResponse>> GetMusicStatsAsync(int userId, CancellationToken ct = default)
            => SendJsonAsync<MusicStatsResponse>(HttpMethod.Get, $"/api/users/{userId}/music/stats", null, ct);

        public string BuildMusicStreamUrl(string relativeStreamUrl, int userId)
        {
            var baseUri = _http.BaseAddress?.ToString()?.TrimEnd('/') ?? "";
            var rel = relativeStreamUrl.TrimStart('/');
            var sep = rel.Contains('?') ? "&" : "?";
            return $"{baseUri}/{rel}{sep}user_id={userId}";
        }

        public string BuildMusicDownloadUrl(string relativeDownloadUrl, int userId)
        {
            var baseUri = _http.BaseAddress?.ToString()?.TrimEnd('/') ?? "";
            var rel = relativeDownloadUrl.TrimStart('/');
            var sep = rel.Contains('?') ? "&" : "?";
            return $"{baseUri}/{rel}{sep}user_id={userId}";
        }
    }
}