﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletButton : MonoBehaviour
{
    protected Valve.VR.InteractionSystem.HologramHandle tablet;

    void Start()
    {
        tablet = GetComponentInParent<Valve.VR.InteractionSystem.HologramHandle>();
    }

    protected virtual void OnTouch() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finger")
        {
            OnTouch();
        }
    }
}
