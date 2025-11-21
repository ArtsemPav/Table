using UnityEngine;

public class DailyQuestsScreenUI : MonoBehaviour
{
    public Transform content;
    public DailyQuestItemUI questItemPrefab;

    private void Start()
    {
        Build();
        GameManager.Instance.OnDailyQuestsUpdated += Build;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnDailyQuestsUpdated -= Build;
    }

    private void Build()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        var quests = GameManager.Instance.Data.dailyQuests;

        foreach (var q in quests)
        {
            var item = Instantiate(questItemPrefab, content);
            item.Setup(q);
        }
    }

    public void OnBack()
    {
        SceneController.Instance.LoadScene("MainMenu");
    }
}
