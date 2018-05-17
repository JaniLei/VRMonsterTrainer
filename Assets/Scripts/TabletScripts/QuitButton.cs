using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : TabletButton
{
    public TabletSlider slider;

    bool confirmed;


    void Update()
    {
        if (slider.gameObject.activeInHierarchy)
        {
            if (slider.swiped && !confirmed)
            {
                Application.Quit();
                confirmed = true;
            }
        }
    }

    protected override void OnTouch()
    {
        base.OnTouch();
        slider.gameObject.SetActive(true);
    }
}
