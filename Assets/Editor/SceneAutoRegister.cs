#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public static class SceneAutoRegister
{
    // Папка, где лежат все игровые сцены
    private const string ScenesRootFolder = "Assets/Scenes";

    [MenuItem("Tools/Scenes/Auto Fill Build Settings")]
    public static void AutoFillBuildSettings()
    {
        // Получаем все .unity файлы под указанной папкой
        if (!Directory.Exists(ScenesRootFolder))
        {
            Debug.LogWarning($"SceneAutoRegister: папка {ScenesRootFolder} не найдена.");
            return;
        }

        string[] scenePaths = Directory
            .GetFiles(ScenesRootFolder, "*.unity", SearchOption.AllDirectories)
            .Select(path => path.Replace("\\", "/")) // на всякий случай
            .ToArray();

        if (scenePaths.Length == 0)
        {
            Debug.LogWarning($"SceneAutoRegister: в {ScenesRootFolder} не найдено ни одной сцены.");
            return;
        }

        // Создаём массив EditorBuildSettingsScene
        EditorBuildSettingsScene[] newScenes = scenePaths
            .Select(path => new EditorBuildSettingsScene(path, true)) // enabled = true
            .ToArray();

        // Записываем в Build Settings
        EditorBuildSettings.scenes = newScenes;

        Debug.Log($"SceneAutoRegister: записано {newScenes.Length} сцен(ы) в Build Settings.");
    }

    // Авто-вызов при загрузке проекта (по желанию)
    [InitializeOnLoadMethod]
    private static void AutoRegisterOnLoad()
    {
        // Если не хочешь, чтобы оно делалось каждый раз, закомментируй строку ниже
        AutoFillBuildSettings();
    }
}
#endif
