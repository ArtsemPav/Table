using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rightAnswersTMP;
    [SerializeField] private TextMeshProUGUI totalAnswersTMP;
    [SerializeField] private TextMeshProUGUI strikesTMP;
    private int rightAnswers = 0;
    private int totalAnswers = 0;
    private int strikes = 0;

    void Start()
    {
        UpdateTMP();
    }

    public void RightAnswer()
    {
        rightAnswers++;
        totalAnswers++;
        strikes++;
        UpdateTMP();
    }

    public void WrongAnswer()
    {
        totalAnswers++;
        strikes = 0;
        UpdateTMP();
    }

    private void UpdateTMP()
    {
        rightAnswersTMP.text = rightAnswers.ToString();
        totalAnswersTMP.text = totalAnswers.ToString();
        strikesTMP.text = strikes.ToString();
    }
}
