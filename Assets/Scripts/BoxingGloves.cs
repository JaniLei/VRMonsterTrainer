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
        public Collider otherColl;
        
        Collider coll;
        GameObject monster;
        Collider[] monsterColls;
        bool canBeHit = true;

        void Start()
        {
            coll = GetComponentInChildren<Collider>();
            monster = FindObjectOfType<Monster>().gameObject;
            otherColl = monster.GetComponent<Collider>();
            monsterColls = monster.GetComponentsInChildren<Collider>();
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
            {
                if (hand.currentAttachedObject != gameObject)
                {
                    hand.HoverLock(GetComponent<Interactable>());
                    hand.AttachObject(gameObject, attachmentFlags);
                    GetComponentInChildren<Collider>().gameObject.layer = LayerMask.NameToLayer("IgnoreTeleport");
                    hand.GetComponentInChildren<HandControllerState>().gameObject.SetActive(false);
                }
                else
                {
                    hand.DetachObject(gameObject);
                    hand.HoverUnlock(GetComponent<Interactable>());
                    GetComponentInChildren<Collider>().gameObject.layer = LayerMask.NameToLayer("Default");
                    hand.GetComponentInChildren<HandControllerState>(true).gameObject.SetActive(true);
                }
            }
        }

        private void HandAttachedUpdate(Hand hand)
        {
            // send signal for monster to dodge
            if (canBeHit)
            {
                if (hand.GetTrackedObjectVelocity().magnitude > 2)
                {
                    RaycastHit hit;

                    if (Physics.Raycast(transform.position, transform.forward * 0.8f, out hit, 1))
                    {
                        hit.transform.gameObject.SendMessage("Dodge", SendMessageOptions.DontRequireReceiver);
                        StartCoroutine("HitRefresh");
                    }
                }
            }

            //if (canBeHit)
            //{
            //    for (int i = 0; i < monsterColls.Length; i++)
            //    {
            //        if (coll.bounds.Intersects(monsterColls[i].bounds))
            //        {
            //            Boxing mBoxing = monster.GetComponent<Boxing>();
            //            if (mBoxing)
            //            {
            //                mBoxing.GetHit(true);
            //                StartCoroutine("HitRefresh");
            //            }
            //        }
            //    }
            //    //if (otherColl)
            //    //{
            //    //    if (coll.bounds.Intersects(otherColl.bounds))
            //    //    {
            //    //        Collider[] colls = otherColl.GetComponentsInChildren<Collider>();
            //    //        for (int i = 0; i < colls.Length; i++)
            //    //        {
            //    //            if (coll.bounds.Intersects(colls[i].bounds))
            //    //            {
            //    //                Boxing mBoxing = colls[i].GetComponentInParent<Boxing>();
            //    //                if (mBoxing)
            //    //                {
            //    //                    mBoxing.GetHit(true);
            //    //                    StartCoroutine("HitRefresh");
            //    //                }
            //    //            }
            //    //        }
            //    //    }
            //    //}
            //}
        }

        private void OnAttachedToHand(Hand hand)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }

        IEnumerator HitRefresh()
        {
            canBeHit = false;
            yield return new WaitForSeconds(0.5f);
            canBeHit = true;
        }
    }
}
