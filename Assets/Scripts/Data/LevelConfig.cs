using UnityEngine;

public enum TaskType
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    MixedAddSub,
    MixedMulDiv,
    MixedAll
}

public enum LevelKind
{
    Normal,
    Boss
}

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("ID")]
    public string levelId;          // "add_01", "boss_golem_01"
    public string displayName;      // "Сложение: до 10"
    public LevelKind levelKind;

    [Header("Math")]
    public TaskType taskType;
    public int difficulty;          // 1–10
    public int tasksCount = 10;
    public int minValue = 1;
    public int maxValue = 10;

    [Header("Timer")]
    public float baseTime = 60f;

    [Header("Boss")]
    public int bossHp = 10;

    [Header("Rewards")]
    public int baseCoinsReward = 20;
    public int baseXpReward = 50;
}
