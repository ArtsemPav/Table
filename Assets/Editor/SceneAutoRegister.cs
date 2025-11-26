#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

public static class SceneAutoRegister
{
    private const string ScenesRootFolder = "Assets/Scenes";
    private const string GeneratedFolder = "Assets/Scripts/Generated";
    private const string EnumFileName = "SceneID.cs";
    private const string BootSceneName = "Boot";  // <-- ВАЖНО: имя сцены Boot без .unity

    [MenuItem("Tools/Scenes/Auto Fill Build Settings")]
    public static void AutoFillBuildSettings()
    {
        GenerateBuildSettings();
        GenerateSceneEnum();
    }

    // ---------------------------------------------------
    // 1) РЕГИСТРАЦИЯ СЦЕН В BUILD SETTINGS (BOOT = index 0)
    // ---------------------------------------------------
    private static void GenerateBuildSettings()
    {
        if (!Directory.Exists(ScenesRootFolder))
        {
            Debug.LogWarning($"SceneAutoRegister: папка {ScenesRootFolder} не найдена.");
            return;
        }

        var scenePaths = Directory
            .GetFiles(ScenesRootFolder, "*.unity", SearchOption.AllDirectories)
            .Select(path => path.Replace("\\", "/"))
            .ToList();

        if (scenePaths.Count == 0)
        {
            Debug.LogWarning("SceneAutoRegister: сцен не найдено.");
            return;
        }

        // --- Ищем Boot сцену ---
        string bootPath = scenePaths
            .FirstOrDefault(p => Path.GetFileNameWithoutExtension(p).Equals(BootSceneName));

        if (bootPath == null)
        {
            Debug.LogError("SceneAutoRegister: не найдена сцена Boot.unity!");
            return;
        }

        // Убираем Boot из списка, чтобы потом вставить в начало
        scenePaths.Remove(bootPath);

        // Сортируем остальные сцены по алфавиту (можно отключить)
        scenePaths = scenePaths.OrderBy(p => p).ToList();

        // Чтобы Boot был 0 индексом
        var orderedScenes = new[] { bootPath }.Concat(scenePaths).ToList();

        // Создаём Build Settings
        EditorBuildSettingsScene[] buildScenes = orderedScenes
            .Select(path => new EditorBuildSettingsScene(path, true))
            .ToArray();

        EditorBuildSettings.scenes = buildScenes;

        Debug.Log($"SceneAutoRegister: Boot назначен как сцена №0. Всего сцен: {buildScenes.Length}");
        Debug.Log("SceneAutoRegister: Build Settings обновлены.");
    }

    // ---------------------------------------------------
    // 2) ГЕНЕРАЦИЯ ENUM SceneID
    // ---------------------------------------------------
    private static void GenerateSceneEnum()
    {
        var scenes = EditorBuildSettings.scenes;

        if (scenes == null || scenes.Length == 0)
        {
            Debug.LogWarning("SceneAutoRegister: нечего генерировать — Build Settings пусты.");
            return;
        }

        var names = scenes
            .Select(s => Path.GetFileNameWithoutExtension(s.path))
            .Distinct()
            .ToList();

        if (!Directory.Exists(GeneratedFolder))
            Directory.CreateDirectory(GeneratedFolder);

        string enumPath = Path.Combine(GeneratedFolder, EnumFileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("// Автоматически сгенерировано SceneAutoRegister");
        sb.AppendLine("public enum SceneID");
        sb.AppendLine("{");

        foreach (var n in names)
            sb.AppendLine($"    {SanitizeEnumName(n)},");

        sb.AppendLine("}");

        File.WriteAllText(enumPath, sb.ToString());
        AssetDatabase.Refresh();

        Debug.Log($"SceneAutoRegister: enum SceneID создан. ({names.Count} элементов)");
    }

    private static string SanitizeEnumName(string name)
    {
        string clean = new string(name.Where(char.IsLetterOrDigit).ToArray());
        if (char.IsDigit(clean[0])) clean = "_" + clean;
        return clean;
    }

    [InitializeOnLoadMethod]
    private static void AutoRegisterOnLoad()
    {
        AutoFillBuildSettings();
    }
}
#endif
