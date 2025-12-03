using System;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Data
{
[Serializable]
    public class LevelProgressData
    {
        public string levelId;           // ID уровня (например "add_01", "boss_golem_01")

        public int starsEarned;          // Количество полученных звезд (0-3)
        public bool isCompleted;         // Уровень пройден хотя бы раз
        public bool isUnlocked;           // Уровень заблокирован

        public float bestTime;           // Лучшее время прохождения (в секундах)
        public DateTime lastPlayedTime;  // Время последнего прохождения
        public int attemptsCount;        // Количество попыток прохождения

        public int totalCoinsEarned;     // Всего монет заработано на этом уровне
        public int totalXpEarned;        // Всего опыта заработано на этом уровне
    }

    [Serializable]
    public class IslandProgressData
    {
        public string islandId;          // ID острова (например "addition_island")

        public bool isUnlocked;          // Остров открыт
        public int totalStars;           // Общее количество звезд на острове
        public int completedLevels;      // Количество пройденных уровней

        public LevelProgressData[] levelProgresses; // Прогресс по каждому уровню острова
    }

    [Serializable]
    public class PlayerData
    {
        public int totalCoins;           // Общее количество монет
        public int totalXp;              // Общий опыт
        public int playerLevel;          // Уровень игрока
        public LevelConfig LastSelectedLevel;
        public IslandConfig LastSelectedIsLand;

        public IslandProgressData[] islandsProgress;

        [Header("Статистика")]
        public int totalPlayTime;        // Общее время игры (в секундах)
        public int totalLevelsCompleted; // Всего пройдено уровней
        public DateTime firstPlayDate;   // Дата первой игры
        public DateTime lastSaveDate;    // Дата последнего сохранения

        // Метод для инициализации нового прогресса
        public static PlayerData CreateNew()
        {
            var progress = new PlayerData();
            progress.totalCoins = 0;
            progress.totalXp = 0;
            progress.playerLevel = 1;
            progress.totalPlayTime = 0;
            progress.totalLevelsCompleted = 0;
            progress.firstPlayDate = DateTime.Now;
            progress.lastSaveDate = DateTime.Now;
            progress.islandsProgress = new IslandProgressData[0];
            return progress;
        }

        // Метод для обновления прогресса после прохождения уровня
        public void UpdateLevelProgress(string islandId, string levelId,
                                        int stars, float time,
                                        int coinsEarned, int xpEarned)
        {
            foreach (var island in islandsProgress)
            {
                if (island.islandId == islandId)
                {
                    foreach (var level in island.levelProgresses)
                    {
                        if (level.levelId == levelId)
                        {
                            // Обновляем данные уровня
                            level.isCompleted = true;
                            level.starsEarned = Mathf.Max(level.starsEarned, stars);

                            if (time > 0 && (level.bestTime == 0 || time < level.bestTime))
                            {
                                level.bestTime = time;
                            }

                            level.lastPlayedTime = DateTime.Now;
                            level.attemptsCount++;
                            level.totalCoinsEarned += coinsEarned;
                            level.totalXpEarned += xpEarned;

                            // Обновляем данные острова
                            island.totalStars = 0;
                            island.completedLevels = 0;

                            foreach (var lvl in island.levelProgresses)
                            {
                                if (lvl.isCompleted)
                                {
                                    island.completedLevels++;
                                    island.totalStars += lvl.starsEarned;
                                }
                            }

                            // Обновляем общие данные
                            totalCoins += coinsEarned;
                            totalXp += xpEarned;
                            totalLevelsCompleted = Mathf.Max(totalLevelsCompleted, GetTotalCompletedLevels());

                            // Разблокируем следующий уровень на том же острове
                            UnlockNextLevel(islandId, levelId);

                            // Проверяем, нужно ли разблокировать следующий остров
                            if (island.completedLevels == island.levelProgresses.Length)
                            {
                                UnlockNextIsland(islandId);
                            }

                            lastSaveDate = DateTime.Now;
                            return;
                        }
                    }
                }
            }
        }

        // Разблокировка следующего уровня
        private void UnlockNextLevel(string islandId, string currentLevelId)
        {
            foreach (var island in islandsProgress)
            {
                if (island.islandId == islandId)
                {
                    for (int i = 0; i < island.levelProgresses.Length - 1; i++)
                    {
                        if (island.levelProgresses[i].levelId == currentLevelId)
                        {
                            island.levelProgresses[i + 1].isUnlocked = true;
                            return;
                        }
                    }
                }
            }
        }

        // Разблокировка следующего острова
        private void UnlockNextIsland(string currentIslandId)
        {
            for (int i = 0; i < islandsProgress.Length - 1; i++)
            {
                if (islandsProgress[i].islandId == currentIslandId)
                {
                    islandsProgress[i + 1].isUnlocked = true;

                    // Разблокируем первый уровень следующего острова
                    if (islandsProgress[i + 1].levelProgresses.Length > 0)
                    {
                        islandsProgress[i + 1].levelProgresses[0].isUnlocked = true;
                    }
                    return;
                }
            }
        }

        // Получение общего количества пройденных уровней
        public int GetTotalCompletedLevels()
        {
            int total = 0;
            foreach (var island in islandsProgress)
            {
                total += island.completedLevels;
            }
            return total;
        }

        // Получение общего количества звезд
        public int GetTotalStars()
        {
            int total = 0;
            foreach (var island in islandsProgress)
            {
                total += island.totalStars;
            }
            return total;
        }

        // Получение прогресса конкретного уровня
        public LevelProgressData GetLevelProgress(string islandId, string levelId)
        {
            foreach (var island in islandsProgress)
            {
                if (island.islandId == islandId)
                {
                    foreach (var level in island.levelProgresses)
                    {
                        if (level.levelId == levelId)
                        {
                            return level;
                        }
                    }
                }
            }
            return null;
        }

        //Добавление острова
        public void AddNewIsland(IslandConfig islandConfig, bool autoUnlock = false)
        {
            // Проверяем, нет ли уже такого острова
            if (GetIslandProgress(islandConfig.islandId) != null)
            {
                Debug.LogWarning($"Остров {islandConfig.islandId} уже существует!");
                return;
            }

            // Создаем прогресс для нового острова
            IslandProgressData newIslandProgress = new IslandProgressData
            {
                islandId = islandConfig.islandId,
                isUnlocked = islandConfig.isUnlocked,
                totalStars = 0,
                completedLevels = 0,
                levelProgresses = new LevelProgressData[islandConfig.levels.Length]
            };

            // Инициализируем уровни
            for (int i = 0; i < islandConfig.levels.Length; i++)
            {
                newIslandProgress.levelProgresses[i] = new LevelProgressData
                {
                    levelId = islandConfig.levels[i].levelId,
                    starsEarned = 0,
                    isCompleted = false,
                    isUnlocked = islandConfig.levels[i].isUnlocked,
                    bestTime = 0f,
                    lastPlayedTime = DateTime.MinValue,
                    attemptsCount = 0,
                    totalCoinsEarned = 0,
                    totalXpEarned = 0
                };
            }

            // Расширяем массив и добавляем новый остров
            IslandProgressData[] newArray = new IslandProgressData[islandsProgress.Length + 1];
            islandsProgress.CopyTo(newArray, 0);
            newArray[islandsProgress.Length] = newIslandProgress;
            islandsProgress = newArray;

            Debug.Log($"Добавлен новый остров: {islandConfig.displayName}");
            lastSaveDate = DateTime.Now;
        }

        //Метод Разблокировки следующего острова
        private bool CheckAutoUnlock(IslandConfig island)
        {
            // Если это первый остров в игре
            if (islandsProgress.Length == 0)
                return true;

            // Или если игрок прошел все предыдущие острова
            foreach (var existingIsland in islandsProgress)
            {
                if (existingIsland.completedLevels < existingIsland.levelProgresses.Length)
                    return false;
            }

            return true;
        }

        // Получение прогресса острова по ID (уже есть в предыдущем коде, но уточним)
        public IslandProgressData GetIslandProgress(string islandId)
        {
            foreach (var island in islandsProgress)
            {
                if (island.islandId == islandId)
                    return island;
            }
            return null;
        }

        // Получение всех ID островов
        public string[] GetAllIslandIds()
        {
            string[] ids = new string[islandsProgress.Length];
            for (int i = 0; i < islandsProgress.Length; i++)
            {
                ids[i] = islandsProgress[i].islandId;
            }
            return ids;
        }
    }
}