using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : TabletButton
{
    protected override void OnTouch()
    {
        base.OnTouch();
        tablet.OpenMenu(false);
        tablet.OpenInfo(true);
    }
}
