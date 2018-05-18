using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : TabletButton
{
    public TabletSlider slider;
    

    protected override void OnTouch()
    {
        base.OnTouch();
        slider.confirmingType = TabletSlider.ConfirmingType.ConfirmQuit;
        slider.UpdateDescription();
        tablet.screenStatus = Valve.VR.InteractionSystem.Tablet.ScreenStatus.Quit;
    }
}
