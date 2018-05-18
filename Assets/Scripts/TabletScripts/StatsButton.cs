using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsButton : TabletButton
{
    protected override void OnTouch()
    {
        base.OnTouch();
        tablet.screenStatus = Valve.VR.InteractionSystem.Tablet.ScreenStatus.Stats;
    }
}
