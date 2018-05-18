using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoArrow : TabletButton
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
            tablet.currentPicIndex--;
        else
            tablet.currentPicIndex++;
        if (tablet.currentPicIndex >= 6)
            tablet.currentPicIndex = 0;
        else if (tablet.currentPicIndex < 0)
            tablet.currentPicIndex = 5;

        tablet.OpenInfoPicture(tablet.currentPicIndex);
    }
}
