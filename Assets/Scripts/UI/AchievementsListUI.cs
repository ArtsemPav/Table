using UnityEngine;

public class AchievementsListUI : MonoBehaviour
{
    public Transform content;
    public AchievementItemUI itemPrefab;

    private void Start()
    {
        foreach (var a in GameManager.Instance.achievementDefs)
        {
            var item = Instantiate(itemPrefab, content);
            bool unlocked = GameManager.Instance.Data.unlockedAchievements.Contains(a.id);
            item.Setup(a, unlocked);
        }
    }
}
