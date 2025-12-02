using UnityEngine;

public class SwitchSceneButton : BaseButton
{
    [SerializeField] private SceneID targetScene;

    protected override void OnClick()
    {
        SceneController.Instance.LoadScene(targetScene);
    }

}
