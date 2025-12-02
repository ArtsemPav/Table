using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaulSettingsButton : BaseButton
{
    protected override void OnClick()
    {
        SaveManager.Instance.ResetEverything();
    }
}
