using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class BoxingGloves : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.SnapOnAttach;
        
        [Tooltip("Name of the attachment transform under in the hand's hierarchy which the object should should snap to.")]
        public string attachmentPoint;
        public int hitForceMultiplier = 2;
        
        Hand gloveHand;


        void Start()
        {
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
            {
                if (hand.currentAttachedObject != gameObject)
                {
                    hand.HoverLock(GetComponent<Interactable>());
                    hand.AttachObject(gameObject, attachmentFlags);
                }
            }
            else if (hand.currentAttachedObject == gameObject && hand.GetStandardInteractionButtonUp() || 
                     hand.currentAttachedObject == gameObject && ((hand.controller != null) && hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_Grip)))
            {
                hand.DetachObject(gameObject);
                hand.HoverUnlock(GetComponent<Interactable>());
            }
        }
        
        private void HandAttachedUpdate(Hand hand)
        {
            // send signal for monster to dodge
            if (hand.GetTrackedObjectVelocity().magnitude > 2)
            {
                RaycastHit hit;
        
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
                    hit.transform.gameObject.SendMessage("Dodge", SendMessageOptions.DontRequireReceiver);
            }
        }

        private void OnAttachedToHand(Hand hand)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gloveHand = hand;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            gloveHand = null;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (gloveHand /*&& collision.relativeVelocity.magnitude > 0.5f*/)
            {
                if (collision.gameObject.GetComponent<VRMonsterInteraction>())
                {
                    Vector3 force = gloveHand.GetTrackedObjectVelocity();
                    Debug.Log("hit force : " + force.magnitude);
                    force *= hitForceMultiplier;
                    force.y++;
                    collision.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(force, collision.contacts[0].point, ForceMode.Impulse);
                }
            }
        }
    }
}
