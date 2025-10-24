using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private GameObject work;
    [SerializeField] private GameObject menu;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private Score score;
    [SerializeField] private ExampleGeneration exampleGeneration;
    [SerializeField] private Button nextButton;
    [SerializeField] private Toggle add;
    [SerializeField] private Toggle sub;
    [SerializeField] private Toggle multi;
    [SerializeField] private Toggle div;
    private TextMeshProUGUI answer;

    private void Start()
    {
        nextButton.interactable = false;
        answer = GameObject.FindWithTag("Answer").GetComponent<TextMeshProUGUI>();
        answer.text = "?";
        work.SetActive(false);
        menu.SetActive(true);
        exampleGeneration.StartGenaration(1);
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
            }else if (answerResult != exampleGeneration.ResultOperation)
            {
                popupManager.ShowWrongtPopup(exampleGeneration.ResultOperation.ToString());
                score.WrongAnswer();
                nextButton.interactable = true;
            }
        }
    }

    public void Next()
    {
        popupManager.ResetPoups();
        ClearField();
        exampleGeneration.StartGenaration(1);
        nextButton.interactable = false;
    }
    public void Menu()
    {
        work.SetActive(false);
        menu.SetActive(true);
    }
    public void StartGame()
    {
        work.SetActive(true);
        menu.SetActive(false);
    }

    public void Difficult()
    {

    }
}
