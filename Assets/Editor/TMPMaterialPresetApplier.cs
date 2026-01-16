using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TMPMaterialPresetApplier : EditorWindow
{
    private TMP_FontAsset targetFontAsset;
    private Material targetMaterialPreset;
    private SearchScope searchScope = SearchScope.Selected;
    private bool includeInactive = true;
    private bool recursiveSearch = false;
    private Vector2 scrollPos;
    private int processedCount;
    private int skippedCount;

    // Новые поля для поиска материалов
    private string materialSearchFilter = "";
    private List<Material> allFoundMaterials = new List<Material>(); // Все найденные материалы
    private List<Material> filteredMaterials = new List<Material>(); // Отфильтрованные материалы
    private Vector2 materialsScrollPos;
    private bool materialsFound = false;

    private enum SearchScope
    {
        Selected,
        Scene,
        Prefab
    }

    [MenuItem("Tools/TextMeshPro/TMP Material Preset Applier")]
    public static void ShowWindow()
    {
        GetWindow<TMPMaterialPresetApplier>("TMP Preset Applier");
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.Space(5);

        // Основные настройки
        EditorGUILayout.LabelField("Target Settings", EditorStyles.boldLabel);

        // Поле выбора шрифта с кнопкой поиска материалов
        EditorGUILayout.BeginHorizontal();
        targetFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Target Font Asset",
            targetFontAsset, typeof(TMP_FontAsset), false);

        GUI.enabled = targetFontAsset != null;
        if (GUILayout.Button("Find Presets", GUILayout.Width(120)))
        {
            FindPresetMaterials();
        }
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        targetMaterialPreset = (Material)EditorGUILayout.ObjectField("Material Preset",
            targetMaterialPreset, typeof(Material), false);

        EditorGUILayout.Space(10);

        // Кнопки действий
        EditorGUILayout.BeginHorizontal();
        {
            GUI.enabled = targetMaterialPreset != null;
            if (GUILayout.Button(new GUIContent("Apply Preset",
            "Apply the selected material preset to found TMP components"), GUILayout.Height(30)))
            {
                ApplyMaterialPreset();
            }
            GUI.enabled = true;

            if (GUILayout.Button(new GUIContent("Select With Font",
            "Select all GameObjects in scene with the target font asset"),
            GUILayout.Height(30)))
            {
                SelectObjectsWithFontAsset();
            }

            if (GUILayout.Button(new GUIContent("Refresh Materials",
            "Force update all TMP materials in the scene"),
            GUILayout.Height(30)))
            {
                RefreshAllTMPMaterials();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        // Секция поиска пресет-материалов
        if (targetFontAsset != null && materialsFound)
        {
            EditorGUILayout.LabelField("Found Preset Materials", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search:", GUILayout.Width(50));

            // Поле поиска с обработкой изменений
            EditorGUI.BeginChangeCheck();
            materialSearchFilter = EditorGUILayout.TextField(materialSearchFilter);
            if (EditorGUI.EndChangeCheck())
            {
                FilterMaterials();
            }

            if (GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                materialSearchFilter = "";
                FilterMaterials();
            }

            if (GUILayout.Button("Refresh", GUILayout.Width(60)))
            {
                FindPresetMaterials();
            }
            EditorGUILayout.EndHorizontal();

            // Информация о результатах поиска
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Showing {filteredMaterials.Count} of {allFoundMaterials.Count} materials",
                EditorStyles.miniLabel);

            if (!string.IsNullOrEmpty(materialSearchFilter) && filteredMaterials.Count == 0)
            {
                EditorGUILayout.LabelField("No materials found for this filter",
                    EditorStyles.miniLabel);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            // Отображение найденных материалов
            DisplayFoundMaterials();

            EditorGUILayout.Space(15);
        }
        else if (targetFontAsset != null && !materialsFound)
        {
            EditorGUILayout.HelpBox("Click 'Find Presets' to search for materials related to this font",
                MessageType.Info);
        }

        EditorGUILayout.Space(10);

        // Настройки поиска
        /*
        EditorGUILayout.LabelField("Search Settings", EditorStyles.boldLabel);
        searchScope = (SearchScope)EditorGUILayout.EnumPopup("Search Scope", searchScope);
        includeInactive = EditorGUILayout.Toggle(
        new GUIContent("Include Inactive",
        "Include inactive GameObjects in the search"),
        includeInactive);

        if (searchScope == SearchScope.Selected || searchScope == SearchScope.Scene)
        {
            recursiveSearch = EditorGUILayout.Toggle(
            new GUIContent("Recursive Search",
            "When enabled, searches through all child objects.\n" +
            "When disabled, only checks the selected objects themselves.\n\n" +
            "Example:\n" +
            "✓ ON: Panel → Text1, Panel/Child → Text2, Panel/Child/Grandchild → Text3\n" +
            "✗ OFF: Panel → Text1 (only if Panel has TMP component)"),
            recursiveSearch);
        }*/
        EditorGUILayout.EndScrollView();
    }

    void DisplayFoundMaterials()
    {
        if (filteredMaterials.Count == 0)
        {
            if (!string.IsNullOrEmpty(materialSearchFilter))
            {
                EditorGUILayout.HelpBox($"No materials found matching '{materialSearchFilter}'",
                    MessageType.Warning);
            }
            return;
        }

        materialsScrollPos = EditorGUILayout.BeginScrollView(materialsScrollPos,
            GUILayout.Height(Mathf.Min(filteredMaterials.Count * 50 + 20, 400)));

        foreach (var material in filteredMaterials)
        {
            bool isSelected = material == targetMaterialPreset;

            // Стиль фона для выбранного материала
            if (isSelected)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            }

            EditorGUILayout.BeginHorizontal();

            // Информация о материале
            EditorGUILayout.BeginVertical();

            // Кнопка-ссылка на имя материала
            if (GUILayout.Button(material.name, EditorStyles.boldLabel))
            {
                EditorGUIUtility.PingObject(material);
            }

            // Дополнительная информация
            if (isSelected)
            {
                EditorGUILayout.LabelField($"Path: {AssetDatabase.GetAssetPath(material)}",
                    EditorStyles.wordWrappedMiniLabel);
            }

            EditorGUILayout.EndVertical();

            // Кнопки действий
            EditorGUILayout.BeginVertical(GUILayout.Width(60));

            // Кнопка выбора
            if (GUILayout.Button("Select", GUILayout.Height(20)))
            {
                targetMaterialPreset = material;
                EditorGUIUtility.PingObject(material);
                Repaint();
            }

            EditorGUILayout.EndVertical();

/*
            EditorGUILayout.BeginVertical(GUILayout.Width(60));
            // Кнопка применения
            if (GUILayout.Button("Apply", GUILayout.Height(20)))
            {
                targetMaterialPreset = material;
                ApplyMaterialPreset();
            }

            EditorGUILayout.EndVertical();
*/
            EditorGUILayout.EndHorizontal();

            if (isSelected)
            {
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space(2);
        }

        EditorGUILayout.EndScrollView();
    }

    void FindPresetMaterials()
    {
        allFoundMaterials.Clear();
        filteredMaterials.Clear();

        if (targetFontAsset == null)
        {
            Debug.LogWarning("No font asset selected for material search.");
            materialsFound = false;
            return;
        }

        string fontName = targetFontAsset.name;
        Debug.Log($"Searching for materials related to font: {fontName}");

        // Поиск материалов по всему проекту
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && IsMaterialRelatedToFont(material, fontName))
            {
                allFoundMaterials.Add(material);
            }
        }

        // Сортируем материалы по имени
        allFoundMaterials = allFoundMaterials
            .OrderBy(m => m.name)
            .ToList();

        // Копируем все материалы в отфильтрованные
        filteredMaterials = new List<Material>(allFoundMaterials);

        materialsFound = allFoundMaterials.Count > 0;

        Debug.Log($"Found {allFoundMaterials.Count} preset materials for font '{fontName}'");

        // Автоматически выбираем первый материал, если ничего не выбрано
        if (targetMaterialPreset == null && filteredMaterials.Count > 0)
        {
            targetMaterialPreset = filteredMaterials[0];
        }

        // Сбрасываем фильтр поиска
        materialSearchFilter = "";
        Repaint();
    }

    void FilterMaterials()
    {
        if (string.IsNullOrEmpty(materialSearchFilter))
        {
            // Если фильтр пустой, показываем все материалы
            filteredMaterials = new List<Material>(allFoundMaterials);
        }
        else
        {
            // Применяем фильтр
            string filter = materialSearchFilter.ToLower();
            filteredMaterials = allFoundMaterials
                .Where(m => m.name.ToLower().Contains(filter))
                .OrderBy(m => m.name)
                .ToList();
        }

        // Обновляем выбранный материал, если текущий не проходит фильтр
        if (targetMaterialPreset != null && !filteredMaterials.Contains(targetMaterialPreset))
        {
            // Пытаемся найти похожий материал
            var similar = allFoundMaterials.FirstOrDefault(m =>
                m.name.ToLower().Contains(materialSearchFilter.ToLower()));
            targetMaterialPreset = similar;
        }

        Repaint();
    }

    bool IsMaterialRelatedToFont(Material material, string fontName)
    {
        // Упрощенный алгоритм поиска - только по имени
        string materialName = material.name.ToLower();
        string fontNameLower = fontName.ToLower();

        // Проверяем, содержит ли имя материала имя шрифта
        if (materialName.Contains(fontNameLower))
            return true;

        // Проверяем стандартные паттерны имен TMP материалов
        if (materialName.Contains("tmp") || materialName.Contains("textmeshpro"))
        {
            // Также проверяем шейдер
            string shaderName = material.shader.name.ToLower();
            if (shaderName.Contains("textmeshpro") || shaderName.Contains("tmp"))
            {
                return true;
            }
        }

        return false;
    }

    void AnalyzeFontAsset()
    {
        if (targetFontAsset == null)
            return;

        // Автоматически ищем материалы при анализе
        FindPresetMaterials();

        string analysis = $"Font Asset Analysis:\n" +
                         $"Name: {targetFontAsset.name}\n" +
                         $"Face Info: {targetFontAsset.faceInfo.familyName} {targetFontAsset.faceInfo.styleName}\n" +
                         $"Atlas Size: {targetFontAsset.atlasWidth}x{targetFontAsset.atlasHeight}\n" +
                         $"Primary Material: {(targetFontAsset.material != null ? targetFontAsset.material.name : "None")}\n" +
                         $"Found Presets: {allFoundMaterials.Count}\n" +
                         $"Fallback Materials: {targetFontAsset.fallbackFontAssetTable?.Count ?? 0}";

        EditorUtility.DisplayDialog("Font Asset Analysis", analysis, "OK");
    }

    void ApplyMaterialPreset()
    {
        if (targetMaterialPreset == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a Material Preset first.", "OK");
            return;
        }

        List<TextMeshProUGUI> tmpComponents = CollectTMPComponents();

        if (tmpComponents.Count == 0)
        {
            EditorUtility.DisplayDialog("Info", "No TMP components found.", "OK");
            return;
        }

        processedCount = 0;
        skippedCount = 0;
        List<Object> undoObjects = new List<Object>();

        foreach (var tmp in tmpComponents)
        {
            undoObjects.Add(tmp);
        }

        Undo.RecordObjects(undoObjects.ToArray(), "Apply TMP Material Preset");

        foreach (var tmp in tmpComponents)
        {
            if (ShouldProcessComponent(tmp))
            {
                tmp.fontSharedMaterial = targetMaterialPreset;

                var renderer = tmp.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = targetMaterialPreset;
                }

                EditorUtility.SetDirty(tmp);
                processedCount++;
            }
            else
            {
                skippedCount++;
            }
        }

        Debug.Log($"TMP Material Preset applied: {processedCount} processed, {skippedCount} skipped");
        EditorUtility.DisplayDialog("Success",
            $"Material preset applied to {processedCount} TMP components.\nSkipped: {skippedCount}", "OK");
    }

    List<TextMeshProUGUI> CollectTMPComponents()
    {
        List<TextMeshProUGUI> components = new List<TextMeshProUGUI>();

        switch (searchScope)
        {
            case SearchScope.Selected:
                GameObject[] selectedObjects = Selection.gameObjects;
                foreach (var obj in selectedObjects)
                {
                    if (!includeInactive && !obj.activeInHierarchy)
                        continue;

                    if (recursiveSearch)
                        components.AddRange(obj.GetComponentsInChildren<TextMeshProUGUI>(includeInactive));
                    else
                    {
                        var tmp = obj.GetComponent<TextMeshProUGUI>();
                        if (tmp != null) components.Add(tmp);
                    }
                }
                break;

            case SearchScope.Scene:
                components.AddRange(FindObjectsOfType<TextMeshProUGUI>(includeInactive));
                break;

            case SearchScope.Prefab:
                string[] guids = AssetDatabase.FindAssets("t:Prefab");
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    if (prefab != null)
                    {
                        TextMeshProUGUI[] prefabComponents = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
                        components.AddRange(prefabComponents);
                    }
                }
                break;
        }

        return components;
    }

    bool ShouldProcessComponent(TextMeshProUGUI tmp)
    {
        if (targetFontAsset != null && tmp.font != targetFontAsset)
            return false;

        return true;
    }

    void RefreshAllTMPMaterials()
    {
        List<TextMeshProUGUI> tmpComponents = new List<TextMeshProUGUI>();
        tmpComponents.AddRange(FindObjectsOfType<TextMeshProUGUI>(true));

        int refreshed = 0;
        foreach (var tmp in tmpComponents)
        {
            if (tmp.fontSharedMaterial != null)
            {
                tmp.ForceMeshUpdate();
                refreshed++;
            }
        }

        Debug.Log($"Refreshed {refreshed} TMP materials");
        EditorUtility.DisplayDialog("Info", $"Refreshed {refreshed} TMP materials", "OK");
    }

    void SelectObjectsWithFontAsset()
    {
        if (targetFontAsset == null)
        {
            EditorUtility.DisplayDialog("Error", "Please select a Font Asset first.", "OK");
            return;
        }

        List<GameObject> objectsWithFont = new List<GameObject>();
        TextMeshProUGUI[] allTMP = FindObjectsOfType<TextMeshProUGUI>(includeInactive);

        foreach (var tmp in allTMP)
        {
            if (tmp.font == targetFontAsset)
            {
                objectsWithFont.Add(tmp.gameObject);
            }
        }

        if (objectsWithFont.Count > 0)
        {
            Selection.objects = objectsWithFont.ToArray();
            Debug.Log($"Selected {objectsWithFont.Count} objects with font '{targetFontAsset.name}'");
        }
        else
        {
            EditorUtility.DisplayDialog("Info", $"No objects found with font '{targetFontAsset.name}'", "OK");
        }
    }

    // Контекстное меню для быстрого применения
    [MenuItem("Assets/Find TMP Material Presets", false, 100)]
    static void FindPresetsFromContext()
    {
        var selected = Selection.activeObject;
        if (selected is TMP_FontAsset fontAsset)
        {
            var window = GetWindow<TMPMaterialPresetApplier>();
            window.targetFontAsset = fontAsset;
            window.FindPresetMaterials();
            window.Show();
        }
    }

    [MenuItem("Assets/Find TMP Material Presets", true)]
    static bool ValidateFindPresetsFromContext()
    {
        return Selection.activeObject is TMP_FontAsset;
    }
}