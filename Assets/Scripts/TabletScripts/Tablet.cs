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

        public enum ScreenStatus
        {
            Stats,
            MainMenu,
            Info,
            Settings,
            Quit,
            Restart
        }
        public GameObject screen;
        public GameObject followHead;
        public GameObject[] menuItems;
        public GameObject[] statsObjs;
        public GameObject[] infoObjs;
        public GameObject[] settingsObjs;
        public GameObject[] infoPictures;
        public GameObject[] sliderObjs;
        public AudioClip clickSound, warningSound;
        public TabletSlider slider;
        public float shakeThreshold = 9;
        [HideInInspector]public int currentPicIndex;
        public ScreenStatus screenStatus
        {
            get { return _screenStatus; }
            set { _screenStatus = value; UpdateScreenStatus(); }
        }

        ScreenStatus _screenStatus;
        Vector3 screenPos;
        float startY;
        bool attached;
        bool lerp;
        bool once;
        float shakeVel;
        AudioSource audioSource;


        void Start()
        {
            startY = transform.position.y;
            screenPos = screen.transform.localPosition;
            audioSource = GetComponent<AudioSource>();
            //StartCoroutine("StartAttach");
            EventManager.instance.MonsterDeath += OnMonsterDeath;
            EventManager.instance.Victory += OnVictory;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                screenStatus = ScreenStatus.MainMenu;
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
            shakeVel = (hand.GetTrackedObjectVelocity() + hand.GetTrackedObjectAngularVelocity()).magnitude;
            if (shakeVel >= shakeThreshold)
            {
                if (screenStatus != ScreenStatus.MainMenu)
                {
                    screenStatus = ScreenStatus.MainMenu;
                }
                shakeVel = 0;
            }
        }

        private void OnAttachedToHand(Hand hand)
        {
            screenStatus = ScreenStatus.Stats;

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
            //OpenStats(false);
            //OpenInfo(false);
            //OpenSettings(false);
            //ActivateSlider(false);
            foreach (var item in menuItems)
            {
                item.SetActive(open);
            }
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

        void UpdateScreenStatus()
        {
            OpenMenu(false);
            OpenStats(false);
            OpenInfo(false);
            OpenSettings(false);
            ActivateSlider(false);
            switch (screenStatus)
            {
                case ScreenStatus.Stats:
                    OpenStats(true);
                    break;
                case ScreenStatus.MainMenu:
                    OpenMenu(true);
                    break;
                case ScreenStatus.Info:
                    OpenInfo(true);
                    break;
                case ScreenStatus.Settings:
                    OpenSettings(true);
                    break;
                case ScreenStatus.Quit:
                    ActivateSlider(true);
                    break;
                case ScreenStatus.Restart:
                    ActivateSlider(true);
                    break;
                default:
                    break;
            }
        }

        public void PlayClickSound()
        {
            audioSource.PlayOneShot(clickSound);
        }

        public void PlayWarningSound()
        {

            if (screen.activeInHierarchy)
                audioSource.PlayOneShot(warningSound);
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

        public void OnMonsterDeath()
        {
            slider.confirmingType = TabletSlider.ConfirmingType.ConfirmDeathRestart;
            slider.UpdateDescription();
            screenStatus = ScreenStatus.Restart;
        }

        public void OnVictory()
        {
            slider.confirmingType = TabletSlider.ConfirmingType.ConfirmVictoryRestart;
            slider.UpdateDescription();
            screenStatus = ScreenStatus.Restart;
        }
    }
}
