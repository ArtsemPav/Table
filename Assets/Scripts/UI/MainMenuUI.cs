using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlayClicked(string sceneName)
    {
        SceneController.Instance.LoadScene(sceneName);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
