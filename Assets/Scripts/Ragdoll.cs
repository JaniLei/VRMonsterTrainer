using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Ragdoll : MonoBehaviour
    {
        bool isKinematic = true;
        Vector3 pos;
        float startY;
        Collider[] colls;

        void Start()
        {
            startY = transform.position.y;
            colls = GetComponentsInChildren<Collider>(true);
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
                if (hand.otherHand)
                {
                    if (hand.otherHand.GetStandardInteractionButton())
                        ToggleRagdoll();
                }
                else
                    ToggleRagdoll();
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

        // moved to RagdollHelper script
        //void SetKinematic(bool newValue)
        //{
        //    Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        //    foreach (Rigidbody rb in bodies)
        //    {
        //        rb.isKinematic = newValue;
        //        rb.useGravity = !newValue;
        //    }
        //    isKinematic = !isKinematic;
        //}

        public void ToggleRagdoll()
        {
            for (int i = 0; i < colls.Length; i++)
            {
                colls[i].isTrigger = !isKinematic;
            }
            GetComponentInChildren<RagdollHelper>().ragdolled = isKinematic;
            if (!isKinematic)
            {
                Vector3 fixedPos = GetComponentInChildren<RagdollHelper>().gameObject.transform.position;
                fixedPos.y = startY;
                transform.position = fixedPos;
            }
            isKinematic = !isKinematic;
        }
    }
}
