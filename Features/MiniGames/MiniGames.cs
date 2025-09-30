using System;
using System.Collections.Generic;

namespace AICQD.MiniGames
{
    public enum GameCategory { Action, Puzzle, Relaxation, Educational, Motivational, Creative }
    public enum GameDifficulty { Easy, Medium, Hard, Expert }
    public enum GameRank { Novice, Apprentice, Expert, Master }

    public abstract class MiniGame
    {
        protected static readonly Random Rng = new();

        public string GameId { get; }
        public string Name { get; }
        public GameCategory Category { get; }
        public string Description { get; }
        public int EstimatedTimeMinutes { get; }     // in minutes
        public int BaseScore { get; }
        public GameDifficulty Difficulty { get; private set; } = GameDifficulty.Medium;

        // Ranking tiers (total_score thresholds)
        public readonly Dictionary<GameRank, int> RankingTiers = new()
        {
            { GameRank.Novice, 0 },
            { GameRank.Apprentice, 500 },
            { GameRank.Expert, 1500 },
            { GameRank.Master, 3000 }
        };

        protected MiniGame(string gameId, string name, GameCategory category, string description, int estimatedTime, int baseScore)
        {
            GameId = gameId;
            Name = name;
            Category = category;
            Description = description;
            EstimatedTimeMinutes = estimatedTime;
            BaseScore = baseScore;
        }

        // Mirrors: calculate_score(performance_metric, time_taken_seconds)
        public int CalculateScore(double performanceMetric, int timeTakenSeconds)
        {
            var denom = EstimatedTimeMinutes * 60.0 * 2.0;
            var timeFactor = Math.Max(0.5, 1.0 - (timeTakenSeconds / denom));
            var score = (int)(BaseScore * performanceMetric * timeFactor);
            return Math.Max(100, score);
        }

        // Mirrors: get_rank(total_score)
        public GameRank GetRank(int totalScore)
        {
            if (totalScore >= RankingTiers[GameRank.Master]) return GameRank.Master;
            if (totalScore >= RankingTiers[GameRank.Expert]) return GameRank.Expert;
            if (totalScore >= RankingTiers[GameRank.Apprentice]) return GameRank.Apprentice;
            return GameRank.Novice;
        }

        // Mirrors: adjust_difficulty(user_skill, quit_streak)
        public void AdjustDifficulty(double userSkill, int quitStreak)
        {
            // Fixed: Changed quit_streak to quitStreak (parameter name)
            if (userSkill > 0.8 && quitStreak > 30) Difficulty = GameDifficulty.Expert;
            else if (userSkill > 0.6 && quitStreak > 14) Difficulty = GameDifficulty.Hard;
            else if (userSkill > 0.4 && quitStreak > 7) Difficulty = GameDifficulty.Medium;
            else Difficulty = GameDifficulty.Easy;
        }

        // Each concrete game returns a result dictionary with at least "score" and "duration"
        public abstract Dictionary<string, object> Play();
    }

    // ---------- Original games (4) ----------
    public sealed class CravingCrusher : MiniGame
    {
        public CravingCrusher() : base("craving_crusher", "Craving Crusher", GameCategory.Action,
            "Smash craving clouds before they reach the top", 3, 150)
        { }

        public override Dictionary<string, object> Play()
        {
            int duration = 120;
            int cloudsSpawned = Rng.Next(15, 31);
            int cloudsHit = Rng.Next(5, cloudsSpawned + 1);
            double accuracy = cloudsSpawned > 0 ? (double)cloudsHit / cloudsSpawned : 0.0;
            int score = CalculateScore(accuracy, duration);
            return new()
            {
                ["score"] = score,
                ["clouds_spawned"] = cloudsSpawned,
                ["clouds_hit"] = cloudsHit,
                ["accuracy"] = accuracy,
                ["duration"] = duration
            };
        }
    }

    public sealed class BreathQuest : MiniGame
    {
        public BreathQuest() : base("breath_quest", "Breath Quest", GameCategory.Relaxation,
            "Follow breathing patterns to calm your mind", 5, 120)
        { }

        public override Dictionary<string, object> Play()
        {
            int patternsCompleted = Rng.Next(3, 9);
            int perfectPatterns = Rng.Next(1, patternsCompleted + 1);
            double accuracy = patternsCompleted > 0 ? (double)perfectPatterns / patternsCompleted : 0.0;
            int duration = patternsCompleted * 30;
            int score = CalculateScore(accuracy, duration);
            return new()
            {
                ["score"] = score,
                ["patterns_completed"] = patternsCompleted,
                ["perfect_patterns"] = perfectPatterns,
                ["accuracy"] = accuracy,
                ["duration"] = duration
            };
        }
    }

    public sealed class VapeDodge : MiniGame
    {
        public VapeDodge() : base("vape_dodge", "Vape Dodge", GameCategory.Action,
            "Dodge vaping temptations in this obstacle course", 4, 180)
        { }

        public override Dictionary<string, object> Play()
        {
            int duration = 120;
            int obstacles = Rng.Next(20, 41);
            int dodged = Rng.Next(10, obstacles + 1);
            double accuracy = obstacles > 0 ? (double)dodged / obstacles : 0.0;
            int score = CalculateScore(accuracy, duration);
            return new()
            {
                ["score"] = score,
                ["obstacles"] = obstacles,
                ["dodged"] = dodged,
                ["accuracy"] = accuracy,
                ["duration"] = duration
            };
        }
    }

    public sealed class ZenGardenBuilder : MiniGame
    {
        public ZenGardenBuilder() : base("zen_garden", "Zen Garden Builder", GameCategory.Creative,
            "Create a peaceful zen garden to reduce stress", 8, 200)
        { }

        public override Dictionary<string, object> Play()
        {
            int elementsPlaced = 15; // default in python
            double creativity = Rng.NextDouble() * 0.5 + 0.5; // 0.5..1.0
            double harmony = Rng.NextDouble() * 0.5 + 0.5;    // 0.5..1.0
            double perf = (creativity + harmony) / 2.0;
            int duration = elementsPlaced * 20;
            int score = CalculateScore(perf, duration);
            return new()
            {
                ["score"] = score,
                ["elements_placed"] = elementsPlaced,
                ["creativity"] = creativity,
                ["harmony"] = harmony,
                ["duration"] = duration
            };
        }
    }

    // ---------- New games added (10) ----------
    public sealed class FlavorFade : MiniGame
    {
        public FlavorFade() : base("flavor_fade", "Flavor Fade", GameCategory.Puzzle,
            "Memory matching game to eliminate vape flavors", 5, 160)
        { }

        public override Dictionary<string, object> Play()
        {
            int pairs = 12;
            int moves = Rng.Next(pairs, pairs * 2 + 1);
            int perfectMoves = pairs;
            double efficiency = moves > 0 ? (double)perfectMoves / moves : 0.0;
            int duration = moves * 5;
            int score = CalculateScore(efficiency, duration);
            return new()
            {
                ["score"] = score,
                ["pairs"] = pairs,
                ["moves"] = moves,
                ["efficiency"] = efficiency,
                ["duration"] = duration
            };
        }
    }

    public sealed class HabitHighway : MiniGame
    {
        public HabitHighway() : base("habit_highway", "Habit Highway", GameCategory.Motivational,
            "Collect positive habits, avoid negative ones", 4, 170)
        { }

        public override Dictionary<string, object> Play()
        {
            int duration = 120;
            int positiveCollected = Rng.Next(10, 26);
            int negativeAvoided = Rng.Next(5, 21);
            int collisions = Rng.Next(0, 6);
            double performance = (positiveCollected + negativeAvoided - collisions) / 30.0;
            int score = CalculateScore(performance, duration);
            return new()
            {
                ["score"] = score,
                ["positive_collected"] = positiveCollected,
                ["negative_avoided"] = negativeAvoided,
                ["collisions"] = collisions,
                ["duration"] = duration
            };
        }
    }

    public sealed class CraveCanvas : MiniGame
    {
        public CraveCanvas() : base("crave_canvas", "Crave Canvas", GameCategory.Relaxation,
            "Digital coloring for relaxation and mindfulness", 10, 180)
        { }

        public override Dictionary<string, object> Play()
        {
            int sectionsColored = 8;
            double precision = Rng.NextDouble() * 0.3 + 0.7;  // 0.7..1.0
            double creativity = Rng.NextDouble() * 0.4 + 0.6; // 0.6..1.0
            double perf = (precision + creativity) / 2.0;
            int duration = sectionsColored * 45;
            int score = CalculateScore(perf, duration);
            return new()
            {
                ["score"] = score,
                ["sections_colored"] = sectionsColored,
                ["precision"] = precision,
                ["creativity"] = creativity,
                ["duration"] = duration
            };
        }
    }

    public sealed class HydrationHero : MiniGame
    {
        public HydrationHero() : base("hydration_hero", "Hydration Hero", GameCategory.Puzzle,
            "Connect pipes to water plants and stay hydrated", 6, 190)
        { }

        public override Dictionary<string, object> Play()
        {
            int puzzlesSolved = 5;
            double efficiency = Rng.NextDouble() * 0.4 + 0.6; // 0.6..1.0
            int timePerPuzzle = Rng.Next(20, 61);             // seconds
            int duration = puzzlesSolved * timePerPuzzle;
            int score = CalculateScore(efficiency, duration);
            return new()
            {
                ["score"] = score,
                ["puzzles_solved"] = puzzlesSolved,
                ["efficiency"] = efficiency,
                ["duration"] = duration
            };
        }
    }

    public sealed class ThoughtBubblePop : MiniGame
    {
        public ThoughtBubblePop() : base("thought_bubble_pop", "Thought Bubble Pop", GameCategory.Action,
            "Pop negative thoughts before they escape", 3, 170)
        { }

        public override Dictionary<string, object> Play()
        {
            int duration = 120;
            int bubblesSpawned = Rng.Next(15, 31);
            int bubblesPopped = Rng.Next(5, bubblesSpawned + 1);
            double accuracy = bubblesSpawned > 0 ? (double)bubblesPopped / bubblesSpawned : 0.0;
            int score = CalculateScore(accuracy, duration);
            return new()
            {
                ["score"] = score,
                ["bubbles_spawned"] = bubblesSpawned,
                ["bubbles_popped"] = bubblesPopped,
                ["accuracy"] = accuracy,
                ["duration"] = duration
            };
        }
    }

    public sealed class ProgressPath : MiniGame
    {
        public ProgressPath() : base("progress_path", "Progress Path", GameCategory.Educational,
            "Board game representing your quit journey", 15, 250)
        { }

        public override Dictionary<string, object> Play()
        {
            int spacesMoved = 20;
            int challengesCompleted = Rng.Next(3, 9);
            double knowledgeGained = Rng.NextDouble() * 0.5 + 0.5;
            double perf = ((challengesCompleted / 8.0) + knowledgeGained) / 2.0;
            int duration = spacesMoved * 30;
            int score = CalculateScore(perf, duration);
            return new()
            {
                ["score"] = score,
                ["spaces_moved"] = spacesMoved,
                ["challenges_completed"] = challengesCompleted,
                ["knowledge_gained"] = knowledgeGained,
                ["duration"] = duration
            };
        }
    }

    public sealed class SerenitySoundscape : MiniGame
    {
        public SerenitySoundscape() : base("serenity_soundscape", "Serenity Soundscape", GameCategory.Relaxation,
            "Mix ambient sounds for calm and relaxation", 8, 160)
        { }

        public override Dictionary<string, object> Play()
        {
            int soundLayers = 4;
            double harmony = Rng.NextDouble() * 0.4 + 0.6;
            double relaxation = Rng.NextDouble() * 0.3 + 0.7;
            double perf = (harmony + relaxation) / 2.0;
            int duration = soundLayers * 60;
            int score = CalculateScore(perf, duration);
            return new()
            {
                ["score"] = score,
                ["sound_layers"] = soundLayers,
                ["harmony"] = harmony,
                ["relaxation"] = relaxation,
                ["duration"] = duration
            };
        }
    }

    public sealed class HealthyRecipeScramble : MiniGame
    {
        public HealthyRecipeScramble() : base("healthy_recipe_scramble", "Healthy Recipe Scramble", GameCategory.Puzzle,
            "Unscramble healthy ingredients and recipes", 7, 180)
        { }

        public override Dictionary<string, object> Play()
        {
            int recipesSolved = 5;
            double accuracy = Rng.NextDouble() * 0.3 + 0.7;
            int timePerRecipe = Rng.Next(30, 91);
            int duration = recipesSolved * timePerRecipe;
            int score = CalculateScore(accuracy, duration);
            return new()
            {
                ["score"] = score,
                ["recipes_solved"] = recipesSolved,
                ["accuracy"] = accuracy,
                ["duration"] = duration
            };
        }
    }

    public sealed class QuitTrivia : MiniGame
    {
        public QuitTrivia() : base("quit_trivia", "Quit Trivia", GameCategory.Educational,
            "Test knowledge about quitting benefits and facts", 8, 200)
        { }

        public override Dictionary<string, object> Play()
        {
            int questionsAnswered = 10;
            int correctAnswers = Rng.Next(5, questionsAnswered + 1);
            double accuracy = (double)correctAnswers / questionsAnswered;
            int duration = questionsAnswered * 15;
            int score = CalculateScore(accuracy, duration);
            return new()
            {
                ["score"] = score,
                ["questions_answered"] = questionsAnswered,
                ["correct_answers"] = correctAnswers,
                ["accuracy"] = accuracy,
                ["duration"] = duration
            };
        }
    }

    public sealed class DailyAffirmationSpinner : MiniGame
    {
        public DailyAffirmationSpinner() : base("daily_affirmation_spinner", "Daily Affirmation Spinner", GameCategory.Motivational,
            "Spin for motivation and daily challenges", 3, 150)
        { }

        public override Dictionary<string, object> Play()
        {
            int spins = 5;
            int positiveAffirmations = Rng.Next(3, spins + 1);
            int challengesCompleted = Rng.Next(1, spins + 1);
            double perf = (positiveAffirmations + challengesCompleted) / (spins * 2.0);
            int duration = spins * 20;
            int score = CalculateScore(perf, duration);
            return new()
            {
                ["score"] = score,
                ["spins"] = spins,
                ["positive_affirmations"] = positiveAffirmations,
                ["challenges_completed"] = challengesCompleted,
                ["duration"] = duration
            };
        }
    }
}