using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

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

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

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
        SelectedLevelHolder.SelectedLevel = _config;
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
