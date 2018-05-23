using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class VRMonsterInteraction : MonoBehaviour
    {
        public double hitVelocity = 1, petVelocity = 0.3f;
        public float hitForceMultiplier = 2;

        bool canBePet = true;
        bool canBeHit = true;
        int _buttonPresses;
        int buttonPresses
        {
            set { _buttonPresses = value;  if (_buttonPresses < 0) _buttonPresses = 0; }
            get { return _buttonPresses; }
        }
        
        private void OnHandHoverBegin(Hand hand)
        {
            if (canBeHit)
            {
                if (hand.GetStandardInteractionButton() &&
                    hand.GetTrackedObjectVelocity().magnitude >= hitVelocity)
                {
                    if (hand.currentAttachedObject == null)
                    {
                        Debug.Log("monster got hit");
                        Boxing mBoxing = GetComponent<Boxing>();
                        if (mBoxing)
                        {
                            mBoxing.GetHit(false);
                            StartCoroutine("HitRefresh");
                        }
                    }
                }
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetTrackedObjectVelocity().magnitude >= petVelocity && !canBeHit)
            {
                if (canBePet)
                {
                    Debug.Log("petting monster");
                    StartCoroutine("PettingRefresh");
                }
            }
            if (hand.GetStandardInteractionButtonDown())
            {
                StartCoroutine("AddButtonPress");
                if (buttonPresses > 2 && canBePet)
                {
                    StartCoroutine("PettingRefresh");
                    buttonPresses = 0;
                }
            }
        }

        IEnumerator AddButtonPress()
        {
            buttonPresses++;
            yield return new WaitForSeconds(2.5f);
            buttonPresses--;
        }

        IEnumerator PettingRefresh()
        {
            MonsterState mState = GetComponent<MonsterState>();
            if (mState)
                mState.Petting();
            canBePet = false;
            yield return new WaitForSeconds(2);
            canBePet = true;
        }

        IEnumerator HitRefresh()
        {
            canBeHit = false;
            yield return new WaitForSeconds(1);
            canBeHit = true;
        }

    }
}
