using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    /*
    [Header("Refs")]
    public BattleManager battleManager;

    [Header("Stats")]
    public int maxHP = 10;
    public float baseAttackInterval = 5f;

    private int _currentHp;
    private BossPhaseController _phaseController;
    private Coroutine _attackRoutine;
    
    private void Start()
    {
        _currentHp = maxHP;
        _phaseController = new BossPhaseController(maxHP);
        _phaseController.OnPhaseChanged += OnPhaseChanged;

        if (battleManager != null)
        {
            battleManager.OnBossHealthChanged += OnBossHpChanged;
        }

        _attackRoutine = StartCoroutine(AttackLoop());
    }

    private void OnDestroy()
    {
        if (battleManager != null)
        {
            battleManager.OnBossHealthChanged -= OnBossHpChanged;
        }

        if (_phaseController != null)
            _phaseController.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnBossHpChanged(int newHp)
    {
        _currentHp = Mathf.Clamp(newHp, 0, maxHP);
        _phaseController.UpdatePhase(_currentHp);
    }

    private void OnPhaseChanged(BossPhase phase)
    {
        Debug.Log($"Boss phase changed to: {phase}");

        switch (phase)
        {
            case BossPhase.Phase1:
                baseAttackInterval = 5f;
                break;
            case BossPhase.Phase2:
                baseAttackInterval = 3.5f;
                break;
            case BossPhase.Rage:
                baseAttackInterval = 2f;
                break;
        }

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        _attackRoutine = StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (_currentHp > 0)
        {
            yield return new WaitForSeconds(baseAttackInterval);
            PerformAttackByPhase();
        }
    }

    private void PerformAttackByPhase()
    {
        if (battleManager == null) return;

        switch (_phaseController.CurrentPhase)
        {
            case BossPhase.Phase1:
                // Лёгкая атака: визуальный эффект, без штрафов или с минимальными
                Debug.Log("Boss Phase1 attack (light)");
                break;

            case BossPhase.Phase2:
                // Средняя атака: например, сокращение времени
                Debug.Log("Boss Phase2 attack (medium)");
                // тут можно дернуть метод уменьшения таймера
                break;

            case BossPhase.Rage:
                // Жёсткая атака: две задачи подряд / сильный дебафф
                Debug.Log("Boss Rage attack (hard)");
                // тут можно включить «двойную задачу» или визуальный хаос
                break;
        }
    }*/
}
