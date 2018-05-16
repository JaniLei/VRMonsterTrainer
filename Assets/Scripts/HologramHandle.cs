using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class HologramHandle : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.SnapOnAttach;
        public GameObject screen;

        Transform parent;
        Vector3 oldPosition;
        Quaternion oldRotation;
        Vector3 screenPos;


        void Start()
        {
            screenPos = screen.transform.localPosition;
            //parent = transform.parent;
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown())
            {
                if (hand.currentAttachedObject != gameObject)
                {
                    //oldPosition = transform.localPosition;
                    //oldRotation = transform.localRotation;
                    
                    hand.HoverLock(GetComponent<Interactable>());
                    
                    hand.AttachObject(gameObject, attachmentFlags);
                }
            }
            else if (hand.GetStandardInteractionButtonUp())
            {
                hand.DetachObject(gameObject);
                
                hand.HoverUnlock(GetComponent<Interactable>());
                
                //transform.parent = parent;
                //transform.localPosition = oldPosition;
                //transform.localRotation = oldRotation;
            }
        }

        private void OnAttachedToHand(Hand hand)
        {
            GetComponent<TabletPowerToggle>().Power = true;

            if (hand.startingHandType == Hand.HandType.Left)
            {
                if (screenPos.x < 0)
                    screenPos.x = -screenPos.x;
                screen.transform.localPosition = screenPos;
            }
            else if (hand.startingHandType == Hand.HandType.Right)
            {
                if (screenPos.x > 0)
                    screenPos.x = -screenPos.x;
                screen.transform.localPosition = screenPos;
            }
        }

        private void OnDetachedFromHand(Hand hand)
        {
            GetComponent<TabletPowerToggle>().Power = false;
        }
    }
}
