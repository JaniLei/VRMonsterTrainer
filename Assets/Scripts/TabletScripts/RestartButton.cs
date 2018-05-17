using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : TabletButton
{
    public TabletSlider slider;

    bool confirmed;

	
	void Update()
    {
		if (slider.gameObject.activeInHierarchy)
        {
            if (slider.swiped && !confirmed)
            {
                EventManager.instance.RestartGame();
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
