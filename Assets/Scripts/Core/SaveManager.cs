using UnityEngine;
using System.IO;
using Game.Data;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public PlayerData playerData;
    private string savePath;

    private void Awake()
    {
       if  (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "save.json");
            Load();
        }
        else Destroy(gameObject);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void Save()
    {
        if (playerData == null)
        {
            Debug.LogWarning("Data is null, creating new...");
            playerData = new PlayerData();
        }

        playerData.PrepareForSerialization();
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Игра сохранена: " + savePath);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            if (playerData != null)
            {
                playerData.PrepareForSerialization(); // Инициализирует словари
            }
            else
            {
                Debug.LogWarning("Loaded data is null, creating new...");
                playerData = new PlayerData();
            }
            Debug.Log("Игра загружена");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("Созданы новые данные игрока");
        }
    }

    public void ResetSettingsOnly()
    {
        // Сбросить только настройки, но не прогресс
  //      Data.settings = new GameSettings(); // если есть отдельный класс настроек
        Save();
    }

    public void ResetProgressOnly()
    {
        // Сбросить только игровой прогресс
   //     Data.level = 1;
   //     Data.coins = 0;
  //      Data.inventory.Clear();
        // ... другие поля прогресса
        Save();
    }

    public void ResetEverything()
    {
        // Полный сброс
        playerData = new PlayerData();
        Save();

        // Обновить все системы игры
   //     OnResetComplete();
    }

    private void OnResetComplete()
    {
        // Уведомить другие системы о сбросе
        // Например, обновить UI, загрузить первую сцену и т.д.
    }
}
