using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsScreenUI : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text resultTitleText;
    public TMP_Text levelNameText;
    public TMP_Text coinsText;
    public TMP_Text xpText;

    [Header("Stars")]
    public TMP_Text starsText;

    private void Start()
    {
        ApplyResult();
    }

    private void ApplyResult()
    {
        // Если по какой-то причине результат не задан — fallback
        var level = BattleResultHolder.Level;

        bool isWin = BattleResultHolder.IsWin;
        int stars = BattleResultHolder.StarsEarned;
        int coins = BattleResultHolder.CoinsReward;
        int xp = BattleResultHolder.XpReward;

        if (resultTitleText != null)
            resultTitleText.text = isWin ? "Победа!" : "Поражение";

        if (levelNameText != null)
        {
            if (level != null)
                levelNameText.text = level.displayName;
            else
                levelNameText.text = "Уровень";
        }

        if (coinsText != null)
            coinsText.text = isWin ? $"Монеты: +{coins}" : "Монеты: 0";

        if (xpText != null)
            xpText.text = isWin ? $"Опыт: +{xp}" : "Опыт: 0";

        // Звёзды
        
        if (starsText != null)
        {
            starsText.text = new string('*', stars) + new string('-', 3 - stars);
        }
    }
}
