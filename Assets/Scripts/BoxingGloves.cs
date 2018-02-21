using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class BoxingGloves : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.SnapOnAttach;

        [Tooltip("Name of the attachment transform under in the hand's hierarchy which the object should should snap to.")]
        public string attachmentPoint;

        Rigidbody rb;
        Hand gloveHand;


        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        //private void OnHandHoverBegin(Hand hand)
        //{
        //
        //}

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
        
        private void OnAttachedToHand(Hand hand)
        {
            rb.isKinematic = true;
            EventManager.instance.OnBoxingStart();
            gloveHand = hand;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            rb.isKinematic = false;
            EventManager.instance.OnBoxingEnd();
            gloveHand = null;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (gloveHand)
            {
                if (collision.gameObject.GetComponent<VRMonsterInteraction>())
                {
                    Vector3 force = gloveHand.GetTrackedObjectVelocity();
                    force *= 100;
                    force.y+=3;
                    collision.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(force, collision.contacts[0].point, ForceMode.Impulse);
                }
            }
            //foreach (ContactPoint contact in collision.contacts)
            //{
            //    Debug.DrawRay(contact.point, contact.normal, Color.white);
            //}
            //if (collision.relativeVelocity.magnitude > hitVelocity)
            //    audioSource.Play();
        }
    }
}
