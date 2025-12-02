// Scripts/Data/DailyQuest.cs
using System;

namespace Game.Data
{
    [Serializable]
    public class AchievementDef
    {
        public string id;
        public string name;
        public AchievementType type;
        public int value;
    }

    public enum AchievementType
    {
        TotalStars, LevelsCompleted, BossesDefeated, DailyQuestsDone, PlayerLevelReach
    }
}