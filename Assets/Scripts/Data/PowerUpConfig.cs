using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpConfig", menuName = "Game/PowerUp Config")]
public class PowerUpConfig : ScriptableObject
{
    public string id;
    public string displayName;
    public PowerUpType type;
    public Sprite icon;

    [Header("Values")]
    public float value;       // например, +5 секунд или длительность заморозки
    public int damageValue;   // для InstantDamage
}
