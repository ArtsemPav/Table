using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskHistoryDisplay : MonoBehaviour
{
    [SerializeField] private Transform _tableContent;
    [SerializeField] private GameObject _rowPrefab;

    private void Start()
    {
        DisplayHistory();
    }

    private void DisplayHistory()
    {
        if (BattleResultHolder.TaskHistory == null || BattleResultHolder.TaskHistory.Count == 0)
            return;

        foreach (var record in BattleResultHolder.TaskHistory)
        {
            GameObject row = Instantiate(_rowPrefab, _tableContent);

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            if (texts.Length >= 3)
            {
                texts[0].text = record.taskText;
                texts[1].text = record.playerAnswer.ToString();
                texts[2].text = record.correctAnswer.ToString();

                texts[1].color = record.isCorrect ? Color.green : Color.red;
            }
        }
    }
}
