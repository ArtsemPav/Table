using UnityEngine;

public class BattleAnimations : MonoBehaviour
{
    /*
    public BattleManager battleManager;

    [Header("Animators")]
    public Animator taskAnimator;     // анимация текста/панели задачи
    public Animator playerAnimator;   // анимация игрока (опционально)
    public Animator bossAnimator;     // анимация босса

    private void OnEnable()
    {
        if (battleManager == null) return;

        battleManager.OnCorrectAnswer += OnCorrect;
        battleManager.OnWrongAnswer += OnWrong;
        battleManager.OnBattleWin += OnWin;
        battleManager.OnBattleLose += OnLose;
    }

    private void OnDisable()
    {
        if (battleManager == null) return;

        battleManager.OnCorrectAnswer -= OnCorrect;
        battleManager.OnWrongAnswer -= OnWrong;
        battleManager.OnBattleWin -= OnWin;
        battleManager.OnBattleLose -= OnLose;
    }

    private void OnCorrect()
    {
        taskAnimator?.SetTrigger("Correct");
        playerAnimator?.SetTrigger("Attack");
        bossAnimator?.SetTrigger("Hit");
    }

    private void OnWrong()
    {
        taskAnimator?.SetTrigger("Wrong");
        playerAnimator?.SetTrigger("Hurt");
    }

    private void OnWin()
    {
        bossAnimator?.SetTrigger("Defeated");
    }

    private void OnLose()
    {
        playerAnimator?.SetTrigger("Defeated");
    }
    */
}
