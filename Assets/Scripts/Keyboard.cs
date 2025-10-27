using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private Score score;
    [SerializeField] private ExampleGeneration exampleGeneration;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button checkButton;
    [SerializeField] private TextMeshProUGUI answer;
    [SerializeField] private TextMeshProUGUI playerName;

    private void Start()
    {
        playerName.text = PlayerPrefs.GetString("PlayerName", "Ghost");
        exampleGeneration.StartGenaration();
        nextButton.interactable = false;
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
                popupManager.ErrorMsg("Слишком много цифр");
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
        if (answer.text == "?")
        {
            popupManager.ErrorMsg("Введите значение");
        }
        else
        {
            int.TryParse(answer.text, out int answerResult);
            if (answerResult == exampleGeneration.ResultOperation)
            {
                popupManager.ShowRightPopup();
                score.RightAnswer();
                nextButton.interactable = true;
                checkButton.interactable = false;
            }else if (answerResult != exampleGeneration.ResultOperation)
            {
                popupManager.ShowWrongtPopup(exampleGeneration.ResultOperation.ToString());
                score.WrongAnswer();
                nextButton.interactable = true;
                checkButton.interactable = false;
            }
        }
    }

    public void Next()
    {
        popupManager.ResetPoups();
        ClearField();
        exampleGeneration.StartGenaration();
        nextButton.interactable = false;
        checkButton.interactable = true;
    }
}
