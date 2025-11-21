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
    public Image[] starImages;
    public Sprite starFilled;
    public Sprite starEmpty;

    [Header("Buttons")]
    public Button retryButton;
    public Button levelsButton;

    private void Start()
    {
        ApplyResult();
        HookButtons();
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
        if (starImages != null && starImages.Length > 0)
        {
            for (int i = 0; i < starImages.Length; i++)
            {
                if (starImages[i] == null) continue;

                if (i < stars)
                    starImages[i].sprite = starFilled;
                else
                    starImages[i].sprite = starEmpty;
            }
        }
    }

    private void HookButtons()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryClicked);

        if (levelsButton != null)
            levelsButton.onClick.AddListener(OnLevelsClicked);
    }

    private void OnRetryClicked()
    {
        // переиграть тот же уровень
        if (BattleResultHolder.Level != null)
        {
            SelectedLevelHolder.SelectedLevel = BattleResultHolder.Level;
            SceneController.Instance.LoadScene("Battle");
        }
        else
        {
            // fallback
            SceneController.Instance.LoadScene("Battle");
        }
    }

    private void OnLevelsClicked()
    {
        // вернуться к выбору уровней текущего острова
        SceneController.Instance.LoadScene("LevelSelect");
    }
}
