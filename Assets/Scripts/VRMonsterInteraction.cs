using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class VRMonsterInteraction : MonoBehaviour
    {
        
        void Start()
        {

        }

        private void OnHandHoverBegin(Hand hand)
        {
            if (hand.controller.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip) && 
                hand.controller.velocity.magnitude > 2)
                Debug.Log("monster got hit");
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (!hand.GetStandardInteractionButtonDown()
                || ((hand.controller != null) && !hand.controller.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip) && hand.GetTrackedObjectVelocity().magnitude > 1))
                Debug.Log("petting monster");
        }
    }
}
