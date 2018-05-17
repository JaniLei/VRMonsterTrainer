using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletButton : MonoBehaviour
{
    protected Valve.VR.InteractionSystem.Tablet tablet;

    void Start()
    {
        tablet = GetComponentInParent<Valve.VR.InteractionSystem.Tablet>();
    }

    protected virtual void OnTouch() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finger")
        {
            OnTouch();
            var hand = other.gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Hand>();
            if (hand.controller != null)
                hand.controller.TriggerHapticPulse(100);
        }
    }
}
