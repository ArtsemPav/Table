using UnityEngine;

[CreateAssetMenu(fileName = "IslandConfig", menuName = "Game/Island Config")]
public class IslandConfig : ScriptableObject
{
    [Header("ID & Name")]
    public string islandId;          // "addition_island"
    public string displayName;       // "Остров Сложения"
    [TextArea] public string description;

    [Header("Visual")]
    public Sprite icon;
    public Sprite lockedIcon;

    [Header("Levels")]
    public LevelConfig[] levels;     // ВАЖНО: вот этого поля сейчас не хватает

    [Header("Scene")]
    public string sceneToLoad;       // например, "LevelSelect"
}
