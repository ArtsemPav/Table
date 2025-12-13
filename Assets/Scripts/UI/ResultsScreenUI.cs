using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Data;

public class ResultsScreenUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TMP_Text _resultTitleText;
    [SerializeField] private TMP_Text _levelNameText;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _xpText;

    [Header("Stars")]
    [SerializeField] private TMP_Text _starsText;

    private LevelProgressData _levelProgressData;
    private LevelConfig _levelConfig;
    private IslandConfig _islandConfig;

    private void Start()
    {
        ApplyResult();
    }

    private void ApplyResult()
    {

        _islandConfig = GameManager.Instance.GetLastIslandConfig();
        _levelConfig = GameManager.Instance.GetLastLevelConfig();
        _levelProgressData = GameManager.Instance.PlayerData.GetLevelProgress(_islandConfig.islandId, _levelConfig.levelId);
        // Если по какой-то причине результат не задан — fallback
        var level = _levelConfig;
        bool isWin = BattleResultHolder.IsWin;
        int stars = _levelProgressData.starsEarned;
        int coins = _levelConfig.baseCoinsReward;
        int xp = _levelConfig.baseXpReward;

        if (_resultTitleText != null)
            _resultTitleText.text = isWin ? "Победа!" : "Поражение";

        if (_levelNameText != null)
        {
            if (level != null)
                _levelNameText.text = level.displayName;
            else
                _levelNameText.text = "Уровень";
        }

        if (_coinsText != null)
            _coinsText.text = isWin ? $"Монеты: +{coins}" : "Монеты: 0";

        if (_xpText != null)
            _xpText.text = isWin ? $"Опыт: +{xp}" : "Опыт: 0";

        // Звёзды
        
        if (_starsText != null)
        {
            _starsText.text = new string('*', stars) + new string('-', 3 - stars);
        }
    }
}
