using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class IslandButtonUI : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text progressText;
    public TMP_Text TotalStarText;
    public Slider progressBar;
    public GameObject lockOverlay;

    [Header("Animation")]
    public float hoverScale = 1.05f;
    public float hoverSpeed = 8f;
    public float clickScale = 1.15f;
    public float clickDuration = 0.12f;

    private IslandConfig _config;
    private bool _isUnlocked = true;
    private bool _hover;
    private Vector3 _baseScale;

    // ----------------------------
    // INIT
    // ----------------------------

    private void Awake()
    {
        _baseScale = transform.localScale;
    }
    public void Setup(IslandConfig config)
    {
        _config = config;
        if (config == null)
        {
            Debug.LogError("IslandButtonUI: Setup() got NULL config!");
            return;
        }

        nameText.text = config.displayName;
/*
        // Check unlock
        _isUnlocked = GameManager.Instance == null
            ? true
            : GameManager.Instance.IsIslandUnlocked(config.islandId);

        if (_isUnlocked)
        {
            iconImage.sprite = config.icon;
            if (lockOverlay != null) lockOverlay.SetActive(false);
        }
        else
        {
            if (config.lockedIcon != null)
                iconImage.sprite = config.lockedIcon;
            if (lockOverlay != null) lockOverlay.SetActive(true);
        }
/*/
        UpdateProgressUI();
    }

    // ----------------------------
    // UPDATE (hover animation)
    // ----------------------------

    private void Update()
    {
        Vector3 target = _hover && _isUnlocked
            ? _baseScale * hoverScale
            : _baseScale;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            target,
            Time.unscaledDeltaTime * hoverSpeed
        );
    }

    // ----------------------------
    // PROGRESS
    // ----------------------------

    private void UpdateProgressUI()
    {
        if (_config == null || GameManager.Instance == null) return;
        if (progressBar == null || progressText == null) return;

        int total = _config.levels != null ? _config.levels.Length : 0;
        int completed = 0;

        if (_config.levels != null)
        {
            foreach (var lvl in _config.levels)
            {
                if (GameManager.Instance.GetStars(lvl.levelId) > 0)
                    completed++;
            }
        }
        TotalStarText.text = GameManager.Instance.GetTotalStars().ToString();
        progressBar.value = total > 0 ? (float)completed / total : 0f;
        progressText.text = $"{completed}/{total}";
    }

    // ----------------------------
    // EVENTS
    // ----------------------------

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
            Debug.Log("Island locked: " + _config.islandId);
            return;
        }

        StopAllCoroutines();
        StartCoroutine(ClickAnimation());

        // Load scene
        if (!string.IsNullOrEmpty(_config.sceneToLoad))
        {
            SaveManager.Instance.playerData.LastSelectedIsLand = _config;
            SaveManager.Instance.Save();
            SceneController.Instance.LoadScene("LevelSelect");
        }
        else
            Debug.LogWarning("Island has no sceneToLoad: " + _config.islandId);
    }

    // ----------------------------
    // CLICK ANIMATION
    // ----------------------------

    private IEnumerator ClickAnimation()
    {
        Vector3 start = transform.localScale;
        Vector3 peak = _baseScale * clickScale;

        float t = 0f;
        while (t < clickDuration)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(start, peak, t / clickDuration);
            yield return null;
        }

        t = 0f;
        Vector3 target = _hover ? _baseScale * hoverScale : _baseScale;

        while (t < clickDuration)
        {
            t += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(peak, target, t / clickDuration);
            yield return null;
        }

        transform.localScale = target;
    }
}
