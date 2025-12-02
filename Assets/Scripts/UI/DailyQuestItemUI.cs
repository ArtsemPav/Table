using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Data;

public class DailyQuestItemUI : MonoBehaviour
{
    public TMP_Text descriptionText;
    public TMP_Text progressText;
    public Slider progressBar;
    public Button claimButton;

    private DailyQuest _quest;
/*
    public void Setup(DailyQuest quest)
    {
        _quest = quest;

        descriptionText.text = quest.desc;
        progressText.text = $"{quest.progress}/{quest.target}";
        progressBar.value = (float)quest.progress / quest.target;

        claimButton.gameObject.SetActive(quest.completed);

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClaim);
    }

    private void OnClaim()
    {
        if (!_quest.completed) return;

        // Награда уже выдана в GameManager.AddQuestProgress
        claimButton.gameObject.SetActive(false);

        // popup можно вызывать отсюда или через событие
        DailyQuestPopupUI.Show($"Получено: {_quest.reward} монет!");
    }*/
}
