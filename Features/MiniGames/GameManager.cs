using System;
using System.Collections.Generic;
using System.Linq;

namespace AICQD.MiniGames
{
    public sealed class GameManager
    {
        public Dictionary<string, MiniGame> Games { get; } = new(StringComparer.OrdinalIgnoreCase)
        {
            // Original (4)
            ["craving_crusher"] = new CravingCrusher(),
            ["breath_quest"] = new BreathQuest(),
            ["vape_dodge"] = new VapeDodge(),
            ["zen_garden"] = new ZenGardenBuilder(),
            // New (10)
            ["flavor_fade"] = new FlavorFade(),
            ["habit_highway"] = new HabitHighway(),
            ["crave_canvas"] = new CraveCanvas(),
            ["hydration_hero"] = new HydrationHero(),
            ["thought_bubble_pop"] = new ThoughtBubblePop(),
            ["progress_path"] = new ProgressPath(),
            ["serenity_soundscape"] = new SerenitySoundscape(),
            ["healthy_recipe_scramble"] = new HealthyRecipeScramble(),
            ["quit_trivia"] = new QuitTrivia(),
            ["daily_affirmation_spinner"] = new DailyAffirmationSpinner()
        };

        // userId -> (gameId -> stats)
        private readonly Dictionary<string, Dictionary<string, UserGameStats>> _userStats = new();
        // Minimal time-preference “model”: userId -> hour -> aggregate
        private readonly Dictionary<string, Dictionary<int, TimePrefAgg>> _timePrefs = new();

        public sealed class UserGameStats
        {
            public int TotalScore { get; set; }
            public int PlayCount { get; set; }
            public int BestScore { get; set; }
            public DateTime? LastPlayed { get; set; }
            public GameRank Rank { get; set; } = GameRank.Novice;
        }

        private sealed class TimePrefAgg
        {
            public int PlayCount { get; set; }
            public double TotalEnjoyment { get; set; }
        }

        public sealed class Recommendation
        {
            public string GameId { get; set; } = default!;
            public string Name { get; set; } = default!;
            public string Category { get; set; } = default!;
            public int EstimatedTime { get; set; }
            public int Suitability { get; set; }
            public string Description { get; set; } = default!;
        }

        // Mirrors: recommend_games(...)
        public List<Recommendation> RecommendGames(string userId, int cravingLevel, int stressLevel, int availableTimeMinutes)
        {
            var recs = new List<Recommendation>();

            foreach (var kv in Games)
            {
                var g = kv.Value;
                int suitability = 0;

                // craving
                if (cravingLevel > 5 && (g.Category is GameCategory.Action or GameCategory.Puzzle)) suitability += 2;
                else if (cravingLevel <= 5 && (g.Category is GameCategory.Relaxation or GameCategory.Creative)) suitability += 2;

                // stress
                if (stressLevel > 5 && (g.Category is GameCategory.Relaxation or GameCategory.Creative)) suitability += 2;
                else if (stressLevel <= 5) suitability += 1;

                // time
                if (g.EstimatedTimeMinutes <= availableTimeMinutes) suitability += 2;

                // enjoyment history
                if (_userStats.TryGetValue(userId, out var byGame) &&
                    byGame.TryGetValue(g.GameId, out var stats) &&
                    stats.PlayCount > 0 &&
                    (stats.BestScore / (double)g.BaseScore) > 0.7)
                {
                    suitability += 1;
                }

                recs.Add(new Recommendation
                {
                    GameId = g.GameId,
                    Name = g.Name,
                    Category = g.Category.ToString(),
                    EstimatedTime = g.EstimatedTimeMinutes,
                    Suitability = suitability,
                    Description = g.Description
                });
            }

            return recs
                .OrderByDescending(r => r.Suitability)
                .ThenBy(r => r.EstimatedTime)
                .Take(3)
                .ToList();
        }

        // Mirrors: record_game_session(user_id, game_id, result)
        public void RecordGameSession(string userId, string gameId, Dictionary<string, object> result)
        {
            if (!Games.TryGetValue(gameId, out var game)) return;
            var score = result.TryGetValue("score", out var s) ? Convert.ToInt32(s) : 0;

            if (!_userStats.ContainsKey(userId)) _userStats[userId] = new();
            if (!_userStats[userId].ContainsKey(gameId))
                _userStats[userId][gameId] = new UserGameStats();

            var st = _userStats[userId][gameId];
            st.TotalScore += score;
            st.PlayCount += 1;
            st.BestScore = Math.Max(st.BestScore, score);
            st.LastPlayed = DateTime.Now;
            st.Rank = game.GetRank(st.TotalScore);

            UpdateTimePrefs(userId, gameId, score, game.BaseScore);
        }

        private void UpdateTimePrefs(string userId, string gameId, int score, int baseScore)
        {
            var hour = DateTime.Now.Hour;
            if (!_timePrefs.ContainsKey(userId)) _timePrefs[userId] = new();
            if (!_timePrefs[userId].ContainsKey(hour)) _timePrefs[userId][hour] = new TimePrefAgg();

            var agg = _timePrefs[userId][hour];
            agg.PlayCount += 1;
            var enjoyment = Math.Min(1.0, score / (double)baseScore);
            agg.TotalEnjoyment += enjoyment;
        }

        // Mirrors: get_user_stats(user_id)
        public UserStats GetUserStats(string userId)
        {
            if (!_userStats.ContainsKey(userId))
                return new UserStats { Error = "User not found" };

            int totalScore = 0, totalPlays = 0, maxPlays = 0;
            string? favorite = null;

            foreach (var (gid, st) in _userStats[userId])
            {
                totalScore += st.TotalScore;
                totalPlays += st.PlayCount;
                if (st.PlayCount > maxPlays) { maxPlays = st.PlayCount; favorite = gid; }
            }

            return new UserStats
            {
                TotalScore = totalScore,
                TotalPlays = totalPlays,
                GamesPlayed = _userStats[userId].Count,
                FavoriteGame = favorite,
                GameDetails = _userStats[userId].ToDictionary(kv => kv.Key, kv => (object)kv.Value)
            };
        }

        // Mirrors: get_craving_relief_recommendations(user_id, craving_intensity)
        public List<Recommendation> GetCravingReliefRecommendations(string userId, int cravingIntensity)
        {
            List<GameCategory> pref;
            if (cravingIntensity >= 7) pref = new() { GameCategory.Action, GameCategory.Puzzle };
            else if (cravingIntensity >= 4) pref = new() { GameCategory.Motivational, GameCategory.Puzzle };
            else pref = new() { GameCategory.Relaxation, GameCategory.Creative };

            return Games.Values
                .Where(g => pref.Contains(g.Category) && g.EstimatedTimeMinutes <= 5)
                .Select(g => new Recommendation
                {
                    GameId = g.GameId,
                    Name = g.Name,
                    Category = g.Category.ToString(),
                    EstimatedTime = g.EstimatedTimeMinutes,
                    Description = g.Description,
                    Suitability = 0
                })
                .Take(2)
                .ToList();
        }
    }

    public sealed class UserStats
    {
        public string? Error { get; set; }
        public int TotalScore { get; set; }
        public int TotalPlays { get; set; }
        public int GamesPlayed { get; set; }
        public string? FavoriteGame { get; set; }
        public Dictionary<string, object> GameDetails { get; set; } = new();
    }
}
