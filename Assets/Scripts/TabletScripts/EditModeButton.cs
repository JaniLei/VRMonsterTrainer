using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EditModeButton : TabletButton
{
    public RoomEditMode editMode;
    public Text screenText;
    
    bool on;
    
    void Start()
    {
        screenText.text = ("Edit mode off.");
    }

    protected override void OnTouch()
    {
        base.OnTouch();

        editMode.enabled = !editMode.enabled;
        on = !on;

        if (on)
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
            screenText.text = ("Edit mode on.");
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            screenText.text = ("Edit mode off.");
        }
    }
}
