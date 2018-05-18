using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsArrow : TabletButton
{
    public enum ArrowDirection
    {
        Left,
        Right
    }
    public ArrowDirection arrowDirection;


    protected override void OnTouch()
    {
        base.OnTouch();

        if (arrowDirection == ArrowDirection.Left)
        {
            if (AudioListener.volume > 0)
                AudioListener.volume -= 0.1f;
        }
        else
        {
            if (AudioListener.volume < 1)
                AudioListener.volume += 0.1f;
        }
    }
}
