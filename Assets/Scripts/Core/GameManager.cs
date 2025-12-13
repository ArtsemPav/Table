using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Game.Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerData PlayerData { get; private set; }

    [Header("Islands configs")]
    public IslandConfig[] IslandsList;

    [Header("Gameplay XP")]
    public int xpPerLevel = 100;

    [Header("Daily Quests Templates")]
    public List<DailyQuest> questTemplates;

    [Header("Achievements")]
    public List<AchievementDef> achievementDefs;

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnXPChanged;
    public event Action<int> OnLevelChanged;
    public event Action OnStarsChanged;
    public event Action OnInventoryChanged;
    public event Action OnDailyQuestsUpdated;
    public event Action<AchievementDef> OnAchievementUnlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //        EnsureDefaultUnlocks();
            //        GenerateDailyQuestsIfNeeded();
            //       CheckAchievements();
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager должен быть инициализирован перед GameManager!");
            return;
        }

        Initialize();
    }
    private void Initialize()
    {
        // Теперь SaveManager.Instance уже должен существовать
        if (SaveManager.Instance != null)
        {
            PlayerData = SaveManager.Instance.PlayerData;

            // Инициализируем острова
            if (PlayerData != null && IslandsList != null)
            {
                foreach (var island in IslandsList)
                {
                    PlayerData.AddNewIsland(island, true);
                }
                SaveManager.Instance.Save(PlayerData);
            }
        }
        else
        {
            Debug.LogError("SaveManager not initialized!");
        }
    }

    public IslandConfig GetLastIslandConfig()
    {
        IslandConfig _islandConfig = PlayerData.LastSelectedIsLand;
        return _islandConfig;
    }

    public LevelConfig GetLastLevelConfig()
    {
        LevelConfig _levelConfig = PlayerData.LastSelectedLevel;
        return _levelConfig;
    }

/*
    public int GetBossesDefeatedCount()
    {
        int c = 0;
        foreach (var kvp in PlayerData.LevelStars)
            if (kvp.Key.StartsWith("boss_") && kvp.Value > 0)
                c++;
        return c;
    }*/


    #region IslandAndModes

 /*   public bool IsIslandUnlocked(string id) => Data.unlockedIslands.Contains(id);
    public void UnlockIsland(string id)
    {
        if (Data.unlockedIslands.Add(id))
            SaveManager.Instance.Save();
    }

    public bool IsModeUnlocked(string id) => Data.unlockedModes.Contains(id);
    public void UnlockMode(string id)
    {
        if (Data.unlockedModes.Add(id))
            SaveManager.Instance.Save();
    }
 */
    #endregion

    #region Inventory
    /*
    public int GetItem(string id) =>
        Data.inventory.TryGetValue(id, out int count) ? count : 0;

    public void AddItem(string id, int amount = 1)
    {
        if (!Data.inventory.ContainsKey(id)) Data.inventory[id] = 0;
        Data.inventory[id] += amount;
        OnInventoryChanged?.Invoke();
        SaveManager.Instance.Save();
    }

    public bool UseItem(string id)
    {
        if (!Data.inventory.ContainsKey(id) || Data.inventory[id] <= 0) return false;

        Data.inventory[id]--;
        OnInventoryChanged?.Invoke();
        SaveManager.Instance.Save();
        return true;
    }
    */
    #endregion

    #region DailyQuest
    /*
    void GenerateDailyQuestsIfNeeded()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        if (Data.dailyQuestDate == today) return;

        Data.dailyQuestDate = today;
        Data.dailyQuests.Clear();

        int count = Mathf.Min(3, questTemplates.Count);
        for (int i = 0; i < count; i++)
        {
            var t = questTemplates[UnityEngine.Random.Range(0, questTemplates.Count)];

            Data.dailyQuests.Add(new DailyQuest
            {
                id = t.id,
                desc = t.desc,
                target = t.target,
                progress = 0,
                reward = t.reward,
                completed = false
            });
        }

        Save();
        OnDailyQuestsUpdated?.Invoke();
    }

    public void AddQuestProgress(string questId, int amount)
    {
        foreach (var q in Data.dailyQuests)
        {
            if (q.id == questId && !q.completed)
            {
                q.progress += amount;

                if (q.progress >= q.target)
                {
                    q.completed = true;
                    q.progress = q.target;
                    AddCoins(q.reward);
                    Data.totalDailyQuestsCompleted++;
                }

                Save();
                OnDailyQuestsUpdated?.Invoke();
                CheckAchievements();
                return;
            }
        }
    }
*/
    #endregion    // DAILY  DAILY QUESTS

    #region Achievements
    /*
    public void CheckAchievements()
    {
        foreach (var a in achievementDefs)
        {
            if (Data.unlockedAchievements.Contains(a.id))
                continue;

            bool unlock = false;

            switch (a.type)
            {
                case AchievementType.TotalStars:
                    unlock = GetTotalStars() >= a.value;
                    break;
                case AchievementType.LevelsCompleted:
                    unlock = GetCompletedLevelsCount() >= a.value;
                    break;
                case AchievementType.BossesDefeated:
                    unlock = GetBossesDefeatedCount() >= a.value;
                    break;
                case AchievementType.DailyQuestsDone:
                    unlock = Data.totalDailyQuestsCompleted >= a.value;
                    break;
                case AchievementType.PlayerLevelReach:
                    unlock = Data.level >= a.value;
                    break;
            }

            if (unlock)
            {
                Data.unlockedAchievements.Add(a.id);
                SaveManager.Instance.Save();
                OnAchievementUnlocked?.Invoke(a);
            }
        }
    }
    */
    #endregion
}
