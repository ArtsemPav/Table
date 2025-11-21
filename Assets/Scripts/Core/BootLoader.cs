using UnityEngine;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private float loadSeconds = 2f;
    private void Start()
    {
        // короткая задержка для красоты/логов (можно убрать)
        GameManager.Instance.AddQuestProgress("daily_login", 1);
        StartCoroutine(LoadNext());

    }

    private System.Collections.IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(loadSeconds);

        // Переход в главное меню
        SceneController.Instance.LoadScene(SceneID.MainMenu);
    }
}
