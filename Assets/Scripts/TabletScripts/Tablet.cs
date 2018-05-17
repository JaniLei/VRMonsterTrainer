using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Tablet : MonoBehaviour
    {
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.SnapOnAttach;
        public GameObject screen;
        public GameObject followHead;
        public GameObject[] menuItems;
        public GameObject statText;
        public float shakeThreshold = 3;
        
        Vector3 screenPos;
        float startY;
        bool attached;
        bool menuOpen;
        bool lerp;


        void Start()
        {
            startY = transform.position.y;
            screenPos = screen.transform.localPosition;
            
            //StartCoroutine("StartAttach");
        }

        void Update()
        {
            if (followHead)
            {
                if (!attached)
                {
                    Vector3 nextPos = followHead.transform.position;// + followHead.transform.right;

                    nextPos.x += 0.2f;
                    nextPos.y = startY;
                    if (lerp)
                        transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime * 5);
                    else
                        transform.position = nextPos;
                }
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown())
            {
                if (hand.currentAttachedObject != gameObject)
                {
                    hand.HoverLock(GetComponent<Interactable>());
                    
                    hand.AttachObject(gameObject, attachmentFlags);
                }
            }
            else if (hand.GetStandardInteractionButtonUp())
            {
                hand.DetachObject(gameObject);
                
                hand.HoverUnlock(GetComponent<Interactable>());
            }
        }

        private void HandAttachedUpdate(Hand hand)
        {
            //if ((rb.velocity + rb.angularVelocity).magnitude >= shakeThreshold)
            //{

            //}
            if ((hand.GetTrackedObjectVelocity() + hand.GetTrackedObjectAngularVelocity()).magnitude >= shakeThreshold)
            {
                if (!menuOpen)
                {
                    statText.SetActive(false);
                    OpenMenu(true);
                }
            }
        }

        private void OnAttachedToHand(Hand hand)
        {
            GetComponent<TabletPowerToggle>().Power = true;
            attached = true;

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
            attached = false;
            StartCoroutine("LerpMovement");
        }

        public void OpenMenu(bool open)
        {
            foreach (var item in menuItems)
            {
                item.SetActive(open);
            }
            menuOpen = open;
        }

        IEnumerator StartAttach()
        {
            yield return new WaitForEndOfFrame();
            Hand hand = FindObjectOfType<Player>().rightHand;
            hand.HoverLock(GetComponent<Interactable>());
            hand.AttachObject(gameObject, attachmentFlags);
            attached = true;
        }

        IEnumerator LerpMovement()
        {
            lerp = true;
            yield return new WaitForSeconds(0.5f);
            lerp = false;
        }
    }
}
