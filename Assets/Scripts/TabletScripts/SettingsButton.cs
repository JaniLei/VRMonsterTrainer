using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : TabletButton
{
    protected override void OnTouch()
    {
        base.OnTouch();
        tablet.OpenMenu(false);
        tablet.OpenSettings(true);
    }
}
