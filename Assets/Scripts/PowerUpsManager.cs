using UnityEngine;
using System;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public event Action<PowerUpConfig> OnPowerUpUsed;

    [Header("Refs")]
    public BattleManager battleManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UsePowerUp(PowerUpConfig config)
    {
        if (config == null || battleManager == null)
            return;

        Debug.Log($"Using powerup: {config.displayName} ({config.type})");

        switch (config.type)
        {
            case PowerUpType.ExtraTime:
                // Здесь должен быть доступ к таймеру
                // battleManager.AddTime(config.value);
                break;

            case PowerUpType.FreezeBoss:
                // тут можно временно отключить атаки босса
                // например, через флаг в BossAI
                break;

            case PowerUpType.Shield:
                // Можно повесить флаг «игрок не теряет HP при следующей ошибке»
                // battleManager.EnableShield();
                break;

            case PowerUpType.InstantDamage:
                // прямой урон боссу
                // battleManager.DirectBossDamage(config.damageValue);
                break;
        }

        OnPowerUpUsed?.Invoke(config);
    }
}
