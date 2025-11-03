using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Keyboard : MonoBehaviour
{
    private float disableTimePopup = 5f;
    private bool isCoroutineRunning;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private Score score;
    [SerializeField] private ExampleGeneration exampleGeneration;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button checkButton;
    [SerializeField] private TextMeshProUGUI answer;
    [SerializeField] private AudioManagerMix audioManagerMix;
    [SerializeField] private AudioClip right;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private AnimationController animationController;

    private void Start()
    {
        audioManagerMix.Play("BGSound");
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
                audioManagerMix.PlayOneShot("Right");
                animationController.HappyAnimation();
                StartCoroutine(DisableAfterSeconds(disableTimePopup));
            }
            else if (answerResult != exampleGeneration.ResultOperation)
            {
                popupManager.ShowWrongtPopup(exampleGeneration.ResultOperation.ToString());
                score.WrongAnswer();
                nextButton.interactable = true;
                checkButton.interactable = false;
                audioManagerMix.PlayOneShot("Wrong");
                animationController.SadAnimation();
                StartCoroutine(DisableAfterSeconds(disableTimePopup));
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
        isCoroutineRunning = false;
    }

    IEnumerator DisableAfterSeconds(float seconds)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(seconds);
        if (isCoroutineRunning)
        {
            popupManager.ResetPoups();
            ClearField();
            exampleGeneration.StartGenaration();
            nextButton.interactable = false;
            checkButton.interactable = true;
        }
    }
}
