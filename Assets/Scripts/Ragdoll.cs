using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Ragdoll : MonoBehaviour
    {
        public Collider mainColl;

        bool isKinematic = true;
        Vector3 pos;
        float startY;
        Collider[] colls;
        MonsterThrowable throwable;
        bool attached;

        void Start()
        {
            startY = transform.position.y;
            colls = GetComponentsInChildren<Collider>(true);
            throwable = GetComponent<MonsterThrowable>();
        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        ToggleRagdoll();
        //    }
        //}

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown())
            {
                if (isKinematic)
                    ToggleRagdoll();
                //StartCoroutine("LateAttach", hand);
                throwable.PhysicsAttach(hand);
            }
        }

        IEnumerator LateAttach(Hand hand)
        {
            yield return new WaitForEndOfFrame();
            throwable.PhysicsAttach(hand);
        }

        private void OnAttachedToHand(Hand hand)
        {
            attached = true;
            hand.canTeleport = false;
            if (hand.otherHand)
                hand.otherHand.canTeleport = false;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            attached = false;
            hand.canTeleport = true;
            if (hand.otherHand)
                hand.otherHand.canTeleport = true;

            //if (hand.otherHand.currentAttachedObject != gameObject)
            if (!EventManager.instance.monsterDead)
            {
                StopAllCoroutines();
                StartCoroutine("RagdollOff");
            }
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
            mainColl.enabled = !isKinematic;
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i] != mainColl)
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

        IEnumerator RagdollOff()
        {
            yield return new WaitForSeconds(2.0f);
            if (!attached)
                ToggleRagdoll();
        }
    }
}
