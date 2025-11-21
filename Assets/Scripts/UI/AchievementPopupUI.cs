using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class AchievementPopupUI : MonoBehaviour
{
    public GameObject popupRoot;
    public TMP_Text titleText;

    public float showTime = 2f;
    public float fadeTime = 0.3f;

    private void Start()
    {
        popupRoot.SetActive(false);
        GameManager.Instance.OnAchievementUnlocked += ShowPopup;
    }

    private void ShowPopup(AchievementDef a)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(a));
    }

    private IEnumerator ShowRoutine(AchievementDef a)
    {
        popupRoot.SetActive(true);
        titleText.text = "Достижение: " + a.name;

        CanvasGroup cg = popupRoot.GetComponent<CanvasGroup>();
        if (cg == null) cg = popupRoot.AddComponent<CanvasGroup>();

        // fade-in
        cg.alpha = 0;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = t / fadeTime;
            yield return null;
        }

        yield return new WaitForSeconds(showTime);

        // fade-out
        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            cg.alpha = 1 - (t / fadeTime);
            yield return null;
        }

        popupRoot.SetActive(false);
    }
}
