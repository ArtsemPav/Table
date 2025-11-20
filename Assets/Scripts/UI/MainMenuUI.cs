using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneController.Instance.LoadScene("IslandSelect");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
