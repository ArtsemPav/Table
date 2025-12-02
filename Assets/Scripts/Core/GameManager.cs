using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Game.Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerData Data
    {
        get
        {
            if (SaveManager.Instance != null)
                return SaveManager.Instance.playerData;

            Debug.LogError("SaveManager не доступен!");
            return null;
        }
    }

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
    //        EnsureDefaultUnlocks();
    //        GenerateDailyQuestsIfNeeded();
     //       CheckAchievements();
        }
        else Destroy(gameObject);
    }

    #region Save_Load

 /*   void EnsureDefaultUnlocks()
    {
        if (Data.unlockedIslands.Count == 0)
            Data.unlockedIslands.Add("addition_island");

        if (Data.unlockedModes.Count == 0)
            Data.unlockedModes.Add("story_mode");
    }*/

    #endregion

    #region Economy
    /*
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
    */
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

        SaveManager.Instance.Save();
  //      CheckAchievements();
    }

    #endregion

    #region Stars

    public int GetStars(string levelId)
    {
        if (SaveManager.Instance.playerData.LevelStars.TryGetValue(levelId, out int stars))
        {
            return stars; // Уровень найден - возвращаем звёзды
        }
        else
        {
            return 0; // Уровень не найден - возвращаем 0
        }
    }
 
    public void SetStars(string levelId, int stars)
    {
        stars = Mathf.Clamp(stars, 0, 3);

        if (!SaveManager.Instance.playerData.LevelStars.ContainsKey(levelId) || Data.LevelStars[levelId] < stars)
        {
            SaveManager.Instance.playerData.LevelStars[levelId] = stars;
            OnStarsChanged?.Invoke();
            SaveManager.Instance.Save();
            //    CheckAchievements();
        }
    }

    public int GetTotalStars()
    {
        int s = 0;
        foreach (var kvp in Data.LevelStars)
            s += kvp.Value;
        return s;
    }

    public int GetCompletedLevelsCount()
    {
        int c = 0;
        foreach (var kvp in Data.LevelStars)
            if (kvp.Value > 0) c++;
        return c;
    }

    public int GetBossesDefeatedCount()
    {
        int c = 0;
        foreach (var kvp in Data.LevelStars)
            if (kvp.Key.StartsWith("boss_") && kvp.Value > 0)
                c++;
        return c;
    }

    #endregion

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
