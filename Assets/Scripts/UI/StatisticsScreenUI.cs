using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticsScreenUI : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text playerLevelText;
    public TMP_Text xpText;
    public TMP_Text coinsText;
    public TMP_Text starsText;
    public TMP_Text levelsCompletedText;
    public TMP_Text bossesText;
    public TMP_Text dailyQuestsText;
    public TMP_Text islandsText;
    public TMP_Text achievementsText;

    // Опционально: чтобы показать "из скольки" островов
    [Header("Config")]
    public int totalIslandsPlanned = 7;          // сколько всего островов в игре
    public int totalAchievementsPlanned = 10;    // сколько всего ачивок (или берем из GameManager)

    private void Start()
    {
        ApplyStats();
    }

    private void ApplyStats()
    {
        var gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogWarning("StatisticsScreenUI: GameManager.Instance is null");
            return;
        }

        var data = gm.Data;

        // Уровень и XP
        if (playerLevelText != null)
            playerLevelText.text = $"Уровень: {data.level}";

        if (xpText != null)
            xpText.text = $"Опыт: {data.xp} / {gm.xpPerLevel}";

        // Монеты
        if (coinsText != null)
            coinsText.text = $"Монеты: {data.coins}";

        // Звезды
        int totalStars = gm.GetTotalStars();
        if (starsText != null)
            starsText.text = $"Звёзды: {totalStars}";

        // Пройденные уровни
        int completedLevels = gm.GetCompletedLevelsCount();
        if (levelsCompletedText != null)
            levelsCompletedText.text = $"Пройдено уровней: {completedLevels}";

        // Побежденные боссы
        int bosses = gm.GetBossesDefeatedCount();
        if (bossesText != null)
            bossesText.text = $"Побеждено боссов: {bosses}";

        // Дейлики
        int totalDailies = data.totalDailyQuestsCompleted;
        if (dailyQuestsText != null)
            dailyQuestsText.text = $"Выполнено ежедневных заданий: {totalDailies}";

        // Острова
        int unlockedIslands = data.unlockedIslands != null ? data.unlockedIslands.Count : 0;
        if (islandsText != null)
        {
            if (totalIslandsPlanned > 0)
                islandsText.text = $"Острова: {unlockedIslands} / {totalIslandsPlanned}";
            else
                islandsText.text = $"Острова: {unlockedIslands}";
        }

        // Достижения
        int unlockedAch = data.unlockedAchievements != null ? data.unlockedAchievements.Count : 0;
        int totalAchDefs = gm.achievementDefs != null ? gm.achievementDefs.Count : totalAchievementsPlanned;

        if (achievementsText != null)
        {
            if (totalAchDefs > 0)
                achievementsText.text = $"Достижения: {unlockedAch} / {totalAchDefs}";
            else
                achievementsText.text = $"Достижения: {unlockedAch}";
        }
    }

    // Можно повесить на кнопку "Обновить", если захочешь
    public void Refresh()
    {
        ApplyStats();
    }

    public void OnBackButton()
    {
        SceneController.Instance.LoadScene("MainMenu");
    }
}
