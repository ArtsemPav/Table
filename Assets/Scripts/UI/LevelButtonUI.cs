using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class LevelButtonUI : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text nameText;
    public TMP_Text starsText;
    public GameObject lockOverlay;

    private LevelConfig _config;
    private bool _isUnlocked;

    public void Setup(LevelConfig config)
    {
        _config = config;
        nameText.text = config.displayName;

        int stars = GameManager.Instance != null
            ? GameManager.Instance.GetStars(config.levelId)
            : 0;

        starsText.text = new string('*', stars) + new string('-', 3 - stars);

        // логику unlock можно сделать простой: все первые уровни открыты
        _isUnlocked = true; // либо через GameManager, если хочешь условия

        lockOverlay.SetActive(!_isUnlocked);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isUnlocked)
            return;

        // Передать выбранный LevelConfig в Battle сцену
        // simplest: запомнить id в GameManager или static LevelConfigHolder

        SelectedLevelHolder.SelectedLevel = _config;
        SceneController.Instance.LoadScene("Battle");
    }
}
