using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class VRMonsterInteraction : MonoBehaviour
    {
        public double hitVelocity = 2, petVelocity = 1;

        bool boxing;

        void Start()
        {
            EventManager.instance.BoxingStart += OnBoxingStart;
            EventManager.instance.BoxingEnd += OnBoxingEnd;
        }

        private void OnHandHoverBegin(Hand hand)
        {
            if (hand.controller != null)
            {
                if (hand.controller.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip) &&
                    hand.controller.velocity.magnitude >= hitVelocity)
                {
                        Debug.Log("monster got hit");
                }
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (!hand.GetStandardInteractionButton() && hand.GetTrackedObjectVelocity().magnitude >= petVelocity ||
                ((hand.controller != null) && !hand.controller.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip) && hand.GetTrackedObjectVelocity().magnitude >= petVelocity))
                Debug.Log("petting monster");
        }

        public void OnBoxingStart()
        {
            boxing = true;
        }

        public void OnBoxingEnd()
        {
            boxing = false;
        }
    }
}
