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
    public TMP_Text nameText;
    public TMP_Text starsText;
    public GameObject lockOverlay;

    [Header("Animation")]
    public float hoverScale = 1.05f;
    public float hoverSpeed = 8f;
    public float clickScale = 1.15f;
    public float clickDuration = 0.12f;

    private LevelConfig _config;
    private bool _isUnlocked;
    private bool _hover;
    private Vector3 _baseScale;

    private LevelProgressData _levelProgressData;
    private IslandConfig _islandConfig;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    public void Setup(LevelConfig config)
    {
        _islandConfig = SaveManager.Instance.playerData.LastSelectedIsLand;
        _config = config;
        _levelProgressData = SaveManager.Instance.playerData.GetLevelProgress(_islandConfig.islandId, _config.levelId);
        nameText.text = config.displayName;

        int stars = _levelProgressData.starsEarned;

        starsText.text = new string('*', stars) + new string('-', 3 - stars);

        // Check unlock
        _isUnlocked = _levelProgressData.isUnlocked;

        if (_isUnlocked)
        {
     //       iconImage.sprite = config.icon;
            if (lockOverlay != null) lockOverlay.SetActive(false);
        }
        else
        {
     //       if (config.lockedIcon != null)
     //           iconImage.sprite = config.lockedIcon;
            if (lockOverlay != null) lockOverlay.SetActive(true);
        }
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
            return;
        StopAllCoroutines();
        StartCoroutine(ClickAnimation());

        // Load scene
        SaveManager.Instance.playerData.LastSelectedLevel = _config;
        SaveManager.Instance.Save();
        SceneController.Instance.LoadScene("Battle");
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
