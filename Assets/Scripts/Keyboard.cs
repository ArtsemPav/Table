using TMPro;
using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private GameObject errorMsg;
    private TextMeshProUGUI erroeMsgTxt;
    private TextMeshProUGUI answer;
    private float disableTime = 3f;

    private void Start()
    {
        answer = GameObject.FindWithTag("Answer").GetComponent<TextMeshProUGUI>();
        erroeMsgTxt = GetComponentInChildren<TextMeshProUGUI>();
        errorMsg.SetActive(false);
        answer.text = "?";
    }

    public void SetNumber(int number)
    {
        if (answer.text == "?")
        {
            answer.text = number.ToString();
            
        } else
        {
            if (answer.text.Length <= 6)
            {
                answer.text = answer.text + number.ToString();
            }
            else
            {
                errorMsg.SetActive(true);
                StartCoroutine(DisableAfterSeconds(disableTime));
            }
        }
    }

    public void ClearField()
    {
        answer.text = "?";
    }

    public void BackSpace()
    {
        if (answer.text.Length > 1)
        {
            answer.text = answer.text.Remove(answer.text.Length - 1);
        } else if (answer.text.Length == 1)
        {
            answer.text = "?";
        }
    }

    public void Check()
    {

    }

    public void Next()
    {

    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        errorMsg.SetActive(false);
    }


}
