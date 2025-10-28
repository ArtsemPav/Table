using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private float disableTimePopup = 3f;
    [SerializeField] private GameObject rightPopup;
    [SerializeField] private GameObject wrongPopup;
    [SerializeField] private GameObject errorPopup;
    [SerializeField] private TextMeshProUGUI rightAnswerTxt;

    private TextMeshProUGUI errorMsg;

    void Start()
    {
        errorMsg = errorPopup.GetComponentInChildren<TextMeshProUGUI>();
        ResetPoups();
    }
    public void ResetPoups()
    {
        rightPopup.SetActive(false);
        wrongPopup.SetActive(false);
        errorPopup.SetActive(false);
    }
    public void ShowRightPopup()
    {
        rightPopup.SetActive(true);
    }

    public void ShowWrongtPopup(string msg)
    {
        wrongPopup.SetActive(true);
        rightAnswerTxt.text = msg;
    }

    public void ErrorMsg(string msg)
    {
        errorMsg.text = msg;
        errorPopup.SetActive(true);
        StartCoroutine(DisableAfterSeconds(disableTimePopup));
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        errorPopup.SetActive(false);
    }

}
