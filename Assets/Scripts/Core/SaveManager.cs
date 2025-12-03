using UnityEngine;
using System;
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
            Addislands();
        }
        else Destroy(gameObject);
    }

    private void Addislands()
    {
        foreach (var island in GameManager.Instance.islands)
        {
            playerData.AddNewIsland(island, true);
        }
        Save();
    }

    public void Save()
    {
        if (playerData == null)
        {
            Debug.LogWarning("Data is null, creating new...");
            playerData = new PlayerData();
        }
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
            Debug.Log("Игра загружена");
        }
        else
        {
            playerData = PlayerData.CreateNew();
            Debug.Log("Созданы новые данные игрока");
            string json = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Файл создан: " + savePath);

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
