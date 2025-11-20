using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public PlayerData Data;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(Data, true);
        File.WriteAllText(GetPath(), json);
    }

    public void Load()
    {
        string path = GetPath();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            Data = new PlayerData();
            Save();
        }
    }

    private string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "save.json");
    }
}
