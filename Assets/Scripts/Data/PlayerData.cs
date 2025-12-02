// Scripts/Data/PlayerData.cs
using System;
using System.Collections.Generic;

namespace Game.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public int coins = 0;
        public int xp = 0;
        public int level = 1;
        public IslandConfig LastSelectedIsLand;

        public LevelConfig LastSelectedLevel;

        public GameSettings settings = new GameSettings();
        public LevelStarsDict LevelStarsData;
        public UnlockedLevelsDict UnlockedLevelsData;


        //      public Dictionary<string, int> levelStars = new Dictionary<string, int>();
        //      public HashSet<string> unlockedIslands = new HashSet<string>();
        //     public HashSet<string> unlockedModes = new HashSet<string>();
        //      public Dictionary<string, int> inventory = new Dictionary<string, int>();

        public List<DailyQuest> dailyQuests = new List<DailyQuest>();
        public string dailyQuestDate = "";

        public HashSet<string> unlockedAchievements = new HashSet<string>();
        public int totalDailyQuestsCompleted = 0;
        public PlayerData()
        {
            LevelStarsData = new LevelStarsDict();
            UnlockedLevelsData = new UnlockedLevelsDict();
            UnlockedLevelsData.Dictionary["Level1"] = true;
            // Инициализация при создании нового сохранения
            //          completedLevels = new List<string> { "Level1" };
            //           unlockedAbilities = new List<string> { "BasicAttack" };
            //           lastPlayed = DateTime.Now;
            //           settings = new GameSettings(); // Убеждаемся, что настройки созданы
        }

        public void PrepareForSerialization()
        {
            // Проверяем инициализацию перед обновлением
            if (LevelStarsData == null) LevelStarsData = new LevelStarsDict();
            if (UnlockedLevelsData == null) UnlockedLevelsData = new UnlockedLevelsDict();

            LevelStarsData.Initialize();
            UnlockedLevelsData.Initialize();

            LevelStarsData.UpdateLists();
            UnlockedLevelsData.UpdateLists();
        }

        public Dictionary<string, int> LevelStars
        {
            get
            {
                if (LevelStarsData == null) LevelStarsData = new LevelStarsDict();
                LevelStarsData.Initialize();
                return LevelStarsData.Dictionary;
            }
        }

        public Dictionary<string, bool> UnlockedLevels
        {
            get
            {
                if (UnlockedLevelsData == null) UnlockedLevelsData = new UnlockedLevelsDict();
                UnlockedLevelsData.Initialize();
                return UnlockedLevelsData.Dictionary;
            }
        }
    }
}