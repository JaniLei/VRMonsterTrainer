using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class VRMonsterInteraction : MonoBehaviour
    {
        public double hitVelocity = 2, petVelocity = 1;
        public float hitForceMultiplier = 2;
        
        
        private void OnHandHoverBegin(Hand hand)
        {
            if (hand.controller != null)
            {
                if (hand.GetStandardInteractionButton() &&
                    hand.GetTrackedObjectVelocity().magnitude >= hitVelocity)
                {
                    Debug.Log("monster got hit");
                    Rigidbody rb = GetComponent<Rigidbody>();
                    if (rb)
                    {
                        Vector3 force = hand.GetTrackedObjectVelocity();
                        Debug.Log("hit force : " + force.magnitude);
                        force *= hitForceMultiplier;
                        force.y++;
                        rb.AddForceAtPosition(force, hand.transform.position, ForceMode.Impulse);
                    }
                }
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (!hand.GetStandardInteractionButton() && hand.GetTrackedObjectVelocity().magnitude >= petVelocity)
                Debug.Log("petting monster");
        }

        void Dodge()
        {
            Debug.Log("Monster dodge");
        }
    }
}
