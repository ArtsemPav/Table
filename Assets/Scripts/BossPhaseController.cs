public enum BossPhase
{
    Phase1,
    Phase2,
    Rage
}

public class BossPhaseController
{
    public BossPhase CurrentPhase { get; private set; }

    public event System.Action<BossPhase> OnPhaseChanged;

    private int _maxHp;

    public BossPhaseController(int maxHp)
    {
        _maxHp = maxHp;
        CurrentPhase = BossPhase.Phase1;
    }

    public void UpdatePhase(int currentHp)
    {
        BossPhase newPhase = CurrentPhase;

        float hpPercent = (float)currentHp / _maxHp;

        if (hpPercent <= 0.2f)
            newPhase = BossPhase.Rage;
        else if (hpPercent <= 0.5f)
            newPhase = BossPhase.Phase2;
        else
            newPhase = BossPhase.Phase1;

        if (newPhase != CurrentPhase)
        {
            CurrentPhase = newPhase;
            OnPhaseChanged?.Invoke(CurrentPhase);
        }
    }
}
