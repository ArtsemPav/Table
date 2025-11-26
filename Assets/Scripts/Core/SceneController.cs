using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public event Action<string> OnSceneLoadStarted;
    public event Action<string> OnSceneLoadCompleted;
    public event Action<float> OnSceneLoadProgress;

    [Header("Loading UI")]
    public GameObject loadingScreen;
    public Slider progressBar;
    public Image fadeImage;

    [Header("Fade Settings")]
    public float fadeDuration = 0.4f;

    [Header("Loading Time")]
    public float minLoadingTime = 2f;  // минимальное время показа экрана загрузки

    [Header("Hints")]
    public TMP_Text hintText;

    [Header("Continue Prompt")]
    public GameObject continueHint;    // текст/кнопка «Нажмите, чтобы продолжить»

    private bool _isLoading = false;
    private int _lastHintIndex = -1;
    private string[] _hints;

    private bool _waitingForUserContinue = false;
    private bool _userPressedContinue = false;

    [Serializable]
    private class HintData
    {
        public string[] hints;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        if (continueHint != null)
            continueHint.SetActive(false);

        LoadHintsFromJson();
    }

    private void LoadHintsFromJson()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("hints"); // Assets/Resources/hints.json

        if (jsonAsset == null)
        {
            Debug.LogWarning("SceneController: hints.json not found in Resources.");
            return;
        }

        try
        {
            HintData data = JsonUtility.FromJson<HintData>(jsonAsset.text);
            if (data != null && data.hints != null && data.hints.Length > 0)
            {
                _hints = data.hints;
            }
            else
            {
                Debug.LogWarning("SceneController: hints.json loaded, but no hints found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("SceneController: error parsing hints.json: " + e.Message);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (_isLoading)
        {
            Debug.LogWarning("Scene is already loading!");
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void LoadScene(SceneID sceneId)
    {
        LoadScene(sceneId.ToString());
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        _isLoading = true;
        _waitingForUserContinue = false;
        _userPressedContinue = false;

        OnSceneLoadStarted?.Invoke(sceneName);

        // Показать подсказку до начала эффекта
        SetRandomHint();

        // FADE OUT
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        if (continueHint != null)
            continueHint.SetActive(false);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float elapsed = 0f;
        bool pauseCompleted = false;

        // Цикл, пока сцена грузится И пока не пришло время спрашивать игрока
        while (!op.isDone)
        {
            elapsed += Time.unscaledDeltaTime;

            // Рассчитываем визуальный прогресс
            float visualProgress = CalculateVisualProgress(elapsed);
            UpdateProgressUI(visualProgress);

            // Проверяем готовность сцены и время
            bool sceneReady = op.progress >= 0.9f;
            bool minTimeReached = elapsed >= minLoadingTime;

            if (sceneReady && minTimeReached && visualProgress >= 1f)
            {
                yield return new WaitForSeconds(0.1f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }

        // Сцена готова к активации, но ждём подтверждение игрока
        _waitingForUserContinue = true;

        if (continueHint != null)
            continueHint.SetActive(true); // тут может быть текст "Нажми, чтобы продолжить"

        // Ждём, пока игрок нажмёт кнопку / клик / тап
        while (!_userPressedContinue)
        {
            // Если хочешь — можно позволить любое нажатие:
             if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
             _userPressedContinue = true;

            yield return null;
        }

        // Прячем подсказку
        if (continueHint != null)
            continueHint.SetActive(false);

        // Разрешаем активацию сцены
        op.allowSceneActivation = true;

        // Ждём, пока сцена реально активируется
        while (!op.isDone)
            yield return null;

        // один кадр, чтобы всё инициализировалось
        yield return null;

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        // FADE IN
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        OnSceneLoadCompleted?.Invoke(sceneName);

        _isLoading = false;
        _waitingForUserContinue = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeImage == null || duration <= 0f)
            yield break;

        float t = 0f;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / duration);
            float a = Mathf.Lerp(from, to, k);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }

    private void SetRandomHint()
    {
        if (hintText == null || _hints == null || _hints.Length == 0)
            return;

        int index;

        if (_hints.Length == 1)
        {
            index = 0;
        }
        else
        {
            do
            {
                index = UnityEngine.Random.Range(0, _hints.Length);
            }
            while (index == _lastHintIndex);
        }

        _lastHintIndex = index;
        hintText.text = _hints[index];
    }

    /// <summary>
    /// Вызывается из кнопки «Продолжить» на экране загрузки.
    /// </summary>
    public void OnUserContinue()
    {
        if (_waitingForUserContinue)
        {
            _userPressedContinue = true;
        }
    }

    private float CalculateVisualProgress(float elapsed)
    {
        float totalPhases = minLoadingTime;
        float firstPhaseEnd = totalPhases * 0.7f;    // 70% времени - первая фаза
        float pauseEnd = firstPhaseEnd + 0.5f;       // +0.5 секунд паузы
        float secondPhaseStart = pauseEnd;
        float secondPhaseDuration = totalPhases - firstPhaseEnd;

        if (elapsed <= firstPhaseEnd)
        {
            // Фаза 1: 0% → 70%
            return Mathf.Lerp(0f, 0.7f, elapsed / firstPhaseEnd);
        }
        else if (elapsed <= pauseEnd)
        {
            // Пауза: остаемся на 70%
            return 0.7f;
        }
        else
        {
            // Фаза 2: 70% → 100%
            float secondPhaseElapsed = elapsed - secondPhaseStart;
            return Mathf.Lerp(0.7f, 1f, secondPhaseElapsed / secondPhaseDuration);
        }
    }

    private void UpdateProgressUI(float progress)
    {
        OnSceneLoadProgress?.Invoke(progress);
        if (progressBar != null)
            progressBar.value = progress;
    }
}
