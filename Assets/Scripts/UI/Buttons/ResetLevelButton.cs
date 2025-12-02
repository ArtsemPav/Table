
using UnityEngine;

public class ResetLevelButton : BaseButton
{
    protected override void OnClick()
    {
        SaveManager.Instance.ResetEverything();
    }
}
