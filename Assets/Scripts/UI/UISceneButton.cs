using UnityEngine;

public class UISceneButton : MonoBehaviour
{
    public SceneID targetScene;

    public void OnClick()
    {
        SceneController.Instance.LoadScene(targetScene);
    }
}
