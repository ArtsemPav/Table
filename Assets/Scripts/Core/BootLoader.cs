using UnityEngine;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        SceneController.Instance.LoadScene(SceneID.MainMenu);
    }
}
