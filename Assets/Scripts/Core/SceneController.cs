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
    public float minLoadingTime = 2f; // минимальное время показа загрузки (в секундах)

    [Header("Hints")]
    public TMP_Text hintText;          // UI-элемент для подсказки

    private bool _isLoading = false;
    private int _lastHintIndex = -1;
    private string[] _hints;       // массив подсказок из JSON

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

        LoadHintsFromJson();
    }

    private void LoadHintsFromJson()
    {
        // файл по пути Assets/Resources/hints.json => Resources.Load<TextAsset>("hints")
        TextAsset jsonAsset = Resources.Load<TextAsset>("hints");

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

        OnSceneLoadStarted?.Invoke(sceneName);

        // показываем подсказку ДО начала fade-out
        SetRandomHint();

        // FADE OUT
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float elapsed = 0f; // сколько уже показан экран загрузки

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            OnSceneLoadProgress?.Invoke(progress);

            if (progressBar != null)
                progressBar.value = progress;

            elapsed += Time.unscaledDeltaTime;

            // сцена фактически загружена, но ждём, пока пройдёт минимум времени
            if (op.progress >= 0.9f && elapsed >= minLoadingTime)
            {
                // маленькая пауза для красоты (по желанию)
                yield return new WaitForSeconds(0.1f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }

        // подождём один кадр, чтобы сцена активировалась
        yield return null;

        if (loadingScreen != null)
            loadingScreen.SetActive(false);

        // FADE IN
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        OnSceneLoadCompleted?.Invoke(sceneName);

        _isLoading = false;
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
}
