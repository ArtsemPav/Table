// Scripts/Data/DailyQuest.cs
using System;

namespace Game.Data
{
    [Serializable]
    public class DailyQuest
    {
        public string id;
        public string title;
        public string description;
        public int rewardCoins;
        public int rewardXP;
        public bool isCompleted;
        public int currentProgress;
        public int requiredProgress;

        // Пример: Сбор 10 монет, убийство 5 врагов и т.д.
    }
}