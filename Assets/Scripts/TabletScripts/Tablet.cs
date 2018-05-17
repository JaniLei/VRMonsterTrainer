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
        public GameObject[] statsObjs;
        public GameObject[] infoObjs;
        public GameObject[] settingsObjs;
        public GameObject[] infoPictures;
        public GameObject[] sliderObjs;
        public float shakeThreshold = 3;
        [HideInInspector]public int currentPicIndex;
        
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
            if (Input.GetKeyDown(KeyCode.M))
            {
                OpenMenu(true);
            }

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
            OpenStats(false);
            OpenInfo(false);
            OpenSettings(false);
            ActivateSlider(false);
            foreach (var item in menuItems)
            {
                item.SetActive(open);
            }
            menuOpen = open;
        }

        public void OpenStats(bool open)
        {
            foreach (var item in statsObjs)
            {
                item.SetActive(open);
            }
        }

        public void OpenInfo(bool open)
        {
            foreach (var item in infoObjs)
            {
                item.SetActive(open);
            }
        }

        public void OpenSettings(bool open)
        {
            foreach (var item in settingsObjs)
            {
                item.SetActive(open);
            }
        }

        public void ActivateSlider(bool active)
        {
            foreach (var item in sliderObjs)
            {
                item.SetActive(active);
            }
        }

        public void OpenInfoPicture(int index)
        {
            for (int i = 0; i < infoPictures.Length; i++)
            {
                if (i == index)
                    infoPictures[i].SetActive(true);
                else
                    infoPictures[i].SetActive(false);
            }
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
