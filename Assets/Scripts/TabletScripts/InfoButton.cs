using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : TabletButton
{
    public GameObject[] infoObjs;

    protected override void OnTouch()
    {
        base.OnTouch();
        tablet.OpenMenu(false);
        foreach (var item in infoObjs)
        {
            item.SetActive(true);
        }
    }
}
