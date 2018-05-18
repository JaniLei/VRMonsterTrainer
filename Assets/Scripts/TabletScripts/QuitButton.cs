﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : TabletButton
{
    public TabletSlider slider;
    

    protected override void OnTouch()
    {
        base.OnTouch();
        slider.confirmingType = TabletSlider.ConfirmingType.ConfirmQuit;
        tablet.OpenMenu(false);
        tablet.ActivateSlider(true);
    }
}
