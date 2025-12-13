using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;
using Game.Data;

public class IslandButtonUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private TMP_Text _TotalStarText;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private GameObject _lockOverlay;

    [Header("Animation")]
    [SerializeField] private float _hoverScale = 1.05f;
    [SerializeField] private float _hoverSpeed = 8f;
    [SerializeField] private float _clickScale = 1.15f;
    [SerializeField] private float _clickDuration = 0.12f;

    private IslandConfig _islandConfig;
    private bool _isUnlocked = true;
    private bool _hover;
    private Vector3 _baseScale;
    private IslandProgressData _islandProgressData;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }
    public void Setup(IslandConfig config)
    {
        _islandConfig = config;
        _islandProgressData = GameManager.Instance.PlayerData.GetIslandProgress(_islandConfig.islandId);
        if (config == null)
        {
            Debug.LogError("IslandButtonUI: Setup() got NULL config!");
            return;
        }
        _nameText.text = config.displayName;

        // Check unlock
        _isUnlocked = _islandProgressData.isUnlocked;

        if (_isUnlocked)
        {
            if (_lockOverlay != null) _lockOverlay.SetActive(false);
        }
        else
        {
            if (config.lockedIcon != null)
            if (_lockOverlay != null) _lockOverlay.SetActive(true);
        }
        UpdateProgressUI();
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

    // PROGRESS
    private void UpdateProgressUI()
    {
        if (_islandConfig == null || GameManager.Instance == null) return;
        if (_progressBar == null || _progressText == null) return;

        int total;
        if (_islandConfig.levels != null)
        {
            total = _islandConfig.levels.Length;
        }
        else
        {
            total = 0;
        }
        int completed = _islandProgressData.completedLevels;

        _TotalStarText.text = _islandProgressData.totalStars.ToString();
        _progressBar.value = total > 0 ? (float)completed / total : 0f;
        _progressText.text = $"{completed}/{total}";
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
        {
            Debug.Log("Island locked: " + _islandConfig.islandId);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(ClickAnimation());

        // Load scene
        if (!string.IsNullOrEmpty(_islandConfig.sceneToLoad))
        {
            GameManager.Instance.PlayerData.LastSelectedIsLand = _islandConfig;
            SaveManager.Instance.Save(GameManager.Instance.PlayerData);
            SceneController.Instance.LoadScene("LevelSelect");
        }
        else
            Debug.LogWarning("Island has no sceneToLoad: " + _islandConfig.islandId);
    }

    // ----------------------------
    // CLICK ANIMATION
    // ----------------------------

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
