using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Data;

public class AchievementItemUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text statusText;

    public void Setup(AchievementDef a, bool unlocked)
    {
        nameText.text = a.name;
        statusText.text = unlocked ? "Получено" : "Не получено";
    }
}
