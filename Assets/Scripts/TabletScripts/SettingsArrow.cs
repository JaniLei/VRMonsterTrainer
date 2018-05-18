using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsArrow : TabletButton
{
    public Text volumeText;
    public enum ArrowDirection
    {
        Left,
        Right
    }
    public ArrowDirection arrowDirection;


    void Start()
    {
        volumeText.text = (Mathf.RoundToInt(AudioListener.volume * 100)).ToString();
    }

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

        volumeText.text = (Mathf.RoundToInt(AudioListener.volume * 100)).ToString();
    }
}
