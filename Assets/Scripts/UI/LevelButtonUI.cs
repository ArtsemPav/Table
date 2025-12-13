using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using Game.Data;

public class LevelButtonUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _starsText;
    [SerializeField] private GameObject _lockOverlay;

    [Header("Animation")]
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _hoverSpeed = 8f;
    [SerializeField] private float _clickScale = 1.15f;
    [SerializeField] private float _clickDuration = 0.12f;


    private bool _isUnlocked;
    private bool _hover;
    private Vector3 _baseScale;
    private LevelProgressData _levelProgressData;
    private IslandConfig _islandConfig;
    private LevelConfig _levelConfig;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    public void Setup(LevelConfig config)
    {
        _islandConfig = GameManager.Instance.PlayerData.LastSelectedIsLand;
        _levelConfig = config;
        _levelProgressData = GameManager.Instance.PlayerData.GetLevelProgress(_islandConfig.islandId, _levelConfig.levelId);
        _nameText.text = config.displayName;

        int stars = _levelProgressData.starsEarned;

        _starsText.text = new string('*', stars) + new string('-', 3 - stars);

        // Check unlock
        _isUnlocked = _levelProgressData.isUnlocked;

        if (_isUnlocked)
        {
            if (_lockOverlay != null) _lockOverlay.SetActive(false);
        }
        else
        {
            if (_lockOverlay != null) _lockOverlay.SetActive(true);
        }
    }

    // UPDATE (hover animation)
    private void Update()
    {
        Vector3 target = _hover && _isUnlocked
            ? _baseScale * _hoverScale
            : _baseScale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            target,
            Time.unscaledDeltaTime * _hoverSpeed
        );
    }

    // EVENTS
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isUnlocked)
            _hover = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _hover = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isUnlocked)
            return;
        StopAllCoroutines();
        StartCoroutine(ClickAnimation());

        // Load scene
        GameManager.Instance.PlayerData.LastSelectedLevel = _levelConfig;
        SaveManager.Instance.Save(GameManager.Instance.PlayerData);
        SceneController.Instance.LoadScene("Battle");
    }

    // CLICK ANIMATION
    private IEnumerator ClickAnimation()
    {
        Vector3 start = transform.localScale;
        Vector3 peak = _baseScale * _clickScale;

        float t = 0f;
        while (t < _clickDuration)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(start, peak, t / _clickDuration);
            yield return null;
        }

        t = 0f;
        Vector3 target = _hover ? _baseScale * _hoverScale : _baseScale;

        while (t < _clickDuration)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(peak, target, t / _clickDuration);
            yield return null;
        }

        transform.localScale = target;
    }
}
