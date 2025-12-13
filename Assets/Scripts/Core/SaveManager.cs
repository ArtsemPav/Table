using UnityEngine;
using System;
using System.IO;
using Game.Data;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public PlayerData PlayerData { get; private set; }
    
    private string _savePath;

    private void Awake()
    {
       if  (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _savePath = Path.Combine(Application.persistentDataPath, "save.json");
            Load();
            // Подписываемся на события жизненного цикла приложения
            Application.focusChanged += OnApplicationFocusChanged;
        }
        else Destroy(gameObject);
    }

    private void Addislands()
    {
        foreach (var island in GameManager.Instance.IslandsList)
        {
            PlayerData.AddNewIsland(island, true);
        }
    }

    public void Save(PlayerData _playerData)
    {
        if (_playerData == null)
        {
            _playerData = PlayerData;
        }
        string json = JsonUtility.ToJson(_playerData, true);
        File.WriteAllText(_savePath, json);
        Debug.Log("Игра сохранена: " + _savePath);
    }

    public void Load()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            PlayerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("Игра загружена");
        }
        else
        {
            PlayerData = PlayerData.CreateNew();
            Debug.LogWarning("Data is null, creating new...");
        }
    }

    public void ResetSettingsOnly()
    {
        // Сбросить только настройки, но не прогресс
  //      Data.settings = new GameSettings(); // если есть отдельный класс настроек
        Save(PlayerData);
    }

    public void ResetProgressOnly()
    {
        // Сбросить только игровой прогресс
   //     Data.level = 1;
   //     Data.coins = 0;
  //      Data.inventory.Clear();
        // ... другие поля прогресса
        Save(PlayerData);
    }

    public void ResetEverything()
    {
        // Полный сброс
        PlayerData = new PlayerData();
        Save(PlayerData);

        // Обновить все системы игры
   //     OnResetComplete();
    }

    private void OnResetComplete()
    {
        // Уведомить другие системы о сбросе
        // Например, обновить UI, загрузить первую сцену и т.д.
    }

    #region События жизненного цикла приложения

    // Вызывается при изменении фокуса приложения
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            // При потере фокуса (переход в фон, минимизация) - сохраняем
            Debug.Log("Application lost focus - auto saving...");
            Save(PlayerData);
        }
    }

    // Вызывается при изменении фокуса (альтернативный подход)
    private void OnApplicationFocusChanged(bool focused)
    {
        if (!focused)
        {
            Debug.Log("Application focus changed - auto saving...");
            Save(PlayerData);
        }
    }

    // Вызывается при паузе приложения (мобильные устройства)
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("Application paused - auto saving...");
            Save(PlayerData);
        }
        else
        {
            // При возобновлении можно проверить целостность данных
            Debug.Log("Application resumed");
    //        CheckDataIntegrity();
        }
    }

    // Вызывается при выходе из игры
    private void OnApplicationQuit()
    {
        Debug.Log("Application quitting - final save...");
        Save(PlayerData);

        // Отписываемся от событий
        Application.focusChanged -= OnApplicationFocusChanged;
    }

    #endregion
}
