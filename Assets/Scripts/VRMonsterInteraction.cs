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

        bool pettingReady = true;
        
        private void OnHandHoverBegin(Hand hand)
        {
            if (hand.GetStandardInteractionButton() &&
                hand.GetTrackedObjectVelocity().magnitude >= hitVelocity)
            {
                if (hand.currentAttachedObject == null)
                {
                    Debug.Log("monster got hit");
                    //Boxing mBoxing = GetComponent<Boxing>();
                    //if (mBoxing)
                    //    mBoxing.GetHit(false);

                    //Rigidbody rb = GetComponent<Rigidbody>();
                    //if (rb)
                    //{
                    //    Vector3 force = hand.GetTrackedObjectVelocity();
                    //    Debug.Log("hit force : " + force.magnitude);
                    //    force *= hitForceMultiplier;
                    //    force.y++;
                    //    rb.AddForceAtPosition(force, hand.transform.position, ForceMode.Impulse);
                    //}
                }
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (!hand.GetStandardInteractionButton() && hand.GetTrackedObjectVelocity().magnitude >= petVelocity)
            {
                if (pettingReady)
                {
                    Debug.Log("petting monster");
                    StartCoroutine("PettingRefresh");
                }
            }
        }

        IEnumerable PettingRefresh()
        {
            MonsterState mState = GetComponent<MonsterState>();
            //if (mState)
            //    mState.Petting();
            pettingReady = false;
            yield return new WaitForSeconds(2.5f);
            pettingReady = true;
        }
        
    }
}
