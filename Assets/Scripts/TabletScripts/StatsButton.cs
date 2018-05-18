using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsButton : TabletButton
{
    protected override void OnTouch()
    {
        base.OnTouch();
        tablet.OpenMenu(false);
        tablet.OpenStats(true);
    }
}
