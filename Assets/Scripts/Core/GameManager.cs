using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int xp = 0;
    public int level = 1;

    public Dictionary<string, int> levelStars = new Dictionary<string, int>();
    public HashSet<string> unlockedIslands = new HashSet<string>();
    public HashSet<string> unlockedModes = new HashSet<string>();
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public List<DailyQuest> dailyQuests = new List<DailyQuest>();
    public string dailyQuestDate = "";

    public HashSet<string> unlockedAchievements = new HashSet<string>();
    public int totalDailyQuestsCompleted = 0;
}

[Serializable]
public class DailyQuest
{
    public string id;
    public string desc;
    public int target;
    public int progress;
    public int reward;
    public bool completed;
}

public enum AchievementType
{
    TotalStars, LevelsCompleted, BossesDefeated, DailyQuestsDone, PlayerLevelReach
}

[Serializable]
public class AchievementDef
{
    public string id;
    public string name;
    public AchievementType type;
    public int value;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData Data { get; private set; } = new PlayerData();

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

    string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
            EnsureDefaultUnlocks();
            GenerateDailyQuestsIfNeeded();
            CheckAchievements();
        }
        else Destroy(gameObject);
    }

    #region Save_Load

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(SavePath, json);
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Data = new PlayerData();
            return;
        }
        string json = File.ReadAllText(SavePath);
        Data = JsonUtility.FromJson<PlayerData>(json);
    }

    void EnsureDefaultUnlocks()
    {
        if (Data.unlockedIslands.Count == 0)
            Data.unlockedIslands.Add("addition_island");

        if (Data.unlockedModes.Count == 0)
            Data.unlockedModes.Add("story_mode");
    }

    #endregion

    #region Economy

    public void AddCoins(int amount)
    {
        Data.coins += amount;
        OnCoinsChanged?.Invoke(Data.coins);
        Save();
    }

    public bool SpendCoins(int amount)
    {
        if (Data.coins < amount) return false;
        Data.coins -= amount;
        OnCoinsChanged?.Invoke(Data.coins);
        Save();
        return true;
    }

    #endregion

    #region XPAndLevel

    public void AddXP(int amount)
    {
        Data.xp += amount;
        OnXPChanged?.Invoke(Data.xp);

        while (Data.xp >= xpPerLevel)
        {
            Data.xp -= xpPerLevel;
            Data.level++;
            OnLevelChanged?.Invoke(Data.level);
        }

        Save();
        CheckAchievements();
    }

    #endregion

    #region Stars

    public int GetStars(string levelId)
    {
        return Data.levelStars.TryGetValue(levelId, out int stars) ? stars : 0;
    }

    public void SetStars(string levelId, int stars)
    {
        stars = Mathf.Clamp(stars, 0, 3);

        if (!Data.levelStars.ContainsKey(levelId) || Data.levelStars[levelId] < stars)
        {
            Data.levelStars[levelId] = stars;
            OnStarsChanged?.Invoke();
            Save();
            CheckAchievements();
        }
    }

    public int GetTotalStars()
    {
        int s = 0;
        foreach (var kvp in Data.levelStars)
            s += kvp.Value;
        return s;
    }

    public int GetCompletedLevelsCount()
    {
        int c = 0;
        foreach (var kvp in Data.levelStars)
            if (kvp.Value > 0) c++;
        return c;
    }

    public int GetBossesDefeatedCount()
    {
        int c = 0;
        foreach (var kvp in Data.levelStars)
            if (kvp.Key.StartsWith("boss_") && kvp.Value > 0)
                c++;
        return c;
    }

    #endregion

    #region IslandAndModes

    public bool IsIslandUnlocked(string id) => Data.unlockedIslands.Contains(id);
    public void UnlockIsland(string id)
    {
        if (Data.unlockedIslands.Add(id))
            Save();
    }

    public bool IsModeUnlocked(string id) => Data.unlockedModes.Contains(id);
    public void UnlockMode(string id)
    {
        if (Data.unlockedModes.Add(id))
            Save();
    }

    #endregion

    #region Inventory

    public int GetItem(string id) =>
        Data.inventory.TryGetValue(id, out int count) ? count : 0;

    public void AddItem(string id, int amount = 1)
    {
        if (!Data.inventory.ContainsKey(id)) Data.inventory[id] = 0;
        Data.inventory[id] += amount;
        OnInventoryChanged?.Invoke();
        Save();
    }

    public bool UseItem(string id)
    {
        if (!Data.inventory.ContainsKey(id) || Data.inventory[id] <= 0) return false;

        Data.inventory[id]--;
        OnInventoryChanged?.Invoke();
        Save();
        return true;
    }

    #endregion

    #region DailyQuest

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
    #endregion    // DAILY  DAILY QUESTS

    #region Achievements
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
                Save();
                OnAchievementUnlocked?.Invoke(a);
            }
        }
    }
    #endregion
}
