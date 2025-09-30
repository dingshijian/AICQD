using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AICQD.Models
{
    // ---------- Requests ----------
    public sealed class MusicGenerateRequest
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("prompt")] public string? Prompt { get; set; }
        [JsonPropertyName("duration")] public int DurationSeconds { get; set; } = 60; // server caps at 300
        [JsonPropertyName("genre")] public string Genre { get; set; } = "ambient";
        [JsonPropertyName("mood")] public string Mood { get; set; } = "calm";
    }

    // ---------- Responses ----------
    public sealed class MusicGenerateResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }
        [JsonPropertyName("music_id")] public string MusicId { get; set; } = string.Empty;
        [JsonPropertyName("filename")] public string Filename { get; set; } = string.Empty;
        [JsonPropertyName("duration")] public int DurationSeconds { get; set; }
        [JsonPropertyName("file_size")] public long FileSize { get; set; }
        // NOTE: server returns relative URLs without user_id query; we append user_id in service helpers
        [JsonPropertyName("download_url")] public string DownloadUrl { get; set; } = string.Empty;
        [JsonPropertyName("stream_url")] public string StreamUrl { get; set; } = string.Empty;

        [JsonPropertyName("metadata")] public MusicMeta Metadata { get; set; } = new();
        [JsonPropertyName("remaining_generations")] public int RemainingGenerations { get; set; }

        public sealed class MusicMeta
        {
            [JsonPropertyName("prompt")] public string? Prompt { get; set; }
            [JsonPropertyName("genre")] public string? Genre { get; set; }
            [JsonPropertyName("mood")] public string? Mood { get; set; }
            [JsonPropertyName("created_at")] public string? CreatedAtIso { get; set; }
        }
    }

    public sealed class MusicTrackDto
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("prompt")] public string? Prompt { get; set; }
        [JsonPropertyName("genre")] public string? Genre { get; set; }
        [JsonPropertyName("mood")] public string? Mood { get; set; }
        [JsonPropertyName("duration")] public int DurationSeconds { get; set; }
        [JsonPropertyName("filename")] public string Filename { get; set; } = string.Empty;
        [JsonPropertyName("created_at")] public string? CreatedAtIso { get; set; }
        [JsonPropertyName("file_size")] public long? FileSize { get; set; }
        [JsonPropertyName("status")] public string? Status { get; set; }
        [JsonPropertyName("download_url")] public string DownloadUrl { get; set; } = string.Empty; // relative
        [JsonPropertyName("stream_url")] public string StreamUrl { get; set; } = string.Empty;     // relative
    }

    public sealed class MusicLibraryResponse
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("music_library")] public List<MusicTrackDto> MusicLibrary { get; set; } = new();
        [JsonPropertyName("total_tracks")] public int TotalTracks { get; set; }
        [JsonPropertyName("today_generations")] public int TodayGenerations { get; set; }
        [JsonPropertyName("remaining_generations")] public int RemainingGenerations { get; set; }
        [JsonPropertyName("daily_limit")] public int DailyLimit { get; set; }
    }

    public sealed class MusicPresetsResponse
    {
        public sealed class PresetItem
        {
            [JsonPropertyName("id")] public string? Id { get; set; }
            [JsonPropertyName("name")] public string? Name { get; set; }
            [JsonPropertyName("description")] public string? Description { get; set; }
            // durations have "seconds" and "label"
            [JsonPropertyName("seconds")] public int? Seconds { get; set; }
            [JsonPropertyName("label")] public string? Label { get; set; }
        }

        [JsonPropertyName("genres")] public List<PresetItem> Genres { get; set; } = new();
        [JsonPropertyName("moods")] public List<PresetItem> Moods { get; set; } = new();
        [JsonPropertyName("durations")] public List<PresetItem> Durations { get; set; } = new();
        [JsonPropertyName("prompt_suggestions")] public List<string> PromptSuggestions { get; set; } = new();
    }

    public sealed class MusicStatsResponse
    {
        [JsonPropertyName("user_id")] public int UserId { get; set; }
        [JsonPropertyName("total_created")] public int TotalCreated { get; set; }
        [JsonPropertyName("total_duration")] public int TotalDurationSeconds { get; set; }
        [JsonPropertyName("total_duration_formatted")] public string? TotalDurationFormatted { get; set; }
        [JsonPropertyName("favorite_genre")] public string? FavoriteGenre { get; set; }
        [JsonPropertyName("favorite_mood")] public string? FavoriteMood { get; set; }
        [JsonPropertyName("creation_streak")] public int CreationStreak { get; set; }
        [JsonPropertyName("last_created")] public string? LastCreatedIso { get; set; }
        [JsonPropertyName("average_duration")] public double AverageDurationSeconds { get; set; }
    }
}

