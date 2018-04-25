using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Ragdoll : MonoBehaviour
    {
        public bool activeOnStart;

        bool isKinematic = false;

        void Start()
        {
            SetKinematic(true);
            if (activeOnStart)
                SetKinematic(!isKinematic);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleRagdoll();
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown())
            {
                SetKinematic(false);
                GetComponentInParent<Animator>().enabled = false;
            }
        }

        private void OnAttachedToHand(Hand hand)
        {
            hand.canTeleport = false;
            if (hand.otherHand)
                hand.otherHand.canTeleport = false;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            hand.canTeleport = true;
            if (hand.otherHand)
                hand.otherHand.canTeleport = true;

            Invoke("ToggleRagdoll", 2);
        }

        void SetKinematic(bool newValue)
        {
            Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in bodies)
            {
                rb.isKinematic = newValue;
            }
            isKinematic = !isKinematic;
        }

        public void ToggleRagdoll()
        {
            SetKinematic(!isKinematic);
            GetComponentInParent<Animator>().enabled = isKinematic;
        }
    }
}
