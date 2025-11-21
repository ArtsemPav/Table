using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DailyQuestPopupUI : MonoBehaviour
{
    public static DailyQuestPopupUI Instance;

    public GameObject root;
    public TMP_Text messageText;
    public float duration = 2f;

    private void Awake()
    {
        Instance = this;
        root.SetActive(false);
    }

    public static void Show(string msg)
    {
        if (Instance == null) return;
        Instance.ShowMessage(msg);
    }

    private void ShowMessage(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(msg));
    }

    private IEnumerator ShowRoutine(string msg)
    {
        root.SetActive(true);
        messageText.text = msg;

        CanvasGroup cg = root.GetComponent<CanvasGroup>();
        if (cg == null) cg = root.AddComponent<CanvasGroup>();

        cg.alpha = 1;

        yield return new WaitForSeconds(duration);

        cg.alpha = 1;
        float t = 0;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            cg.alpha = 1 - (t / 0.3f);
            yield return null;
        }

        root.SetActive(false);
    }
}
