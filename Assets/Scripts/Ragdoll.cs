using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Ragdoll : MonoBehaviour
    {
        bool isKinematic = true;
        Vector3 pos;
        float startY;

        void Start()
        {
            //anim = GetComponentInParent<Animator>();
            //GetComponent<RagdollHelper>().ragdolled = false;
            //ToggleRagdoll();
            startY = transform.position.y;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleRagdoll();
            }
        }

        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown() /*&& hand.otherHand.GetStandardInteractionButtonDown()*/)
            {
                //SetKinematic(false);
                //GetComponentInParent<Animator>().enabled = false;
                //isKinematic = false;
                ToggleRagdoll();
            }
        }

        private void OnAttachedToHand(Hand hand)
        {
            hand.canTeleport = false;
            if (hand.otherHand)
                hand.otherHand.canTeleport = false;
        }

        private void OnDetachedFromHand(Hand hand)
        {
            hand.canTeleport = true;
            if (hand.otherHand)
                hand.otherHand.canTeleport = true;

            Invoke("ToggleRagdoll", 2);
        }

        void SetKinematic(bool newValue)
        {
            Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in bodies)
            {
                rb.isKinematic = newValue;
                rb.useGravity = !newValue;
            }
            isKinematic = !isKinematic;
        }

        public void ToggleRagdoll()
        {
            //anim.enabled = !isKinematic;
            
            GetComponentInChildren<RagdollHelper>().ragdolled = isKinematic;// SetKinematic(!isKinematic);
            if (!isKinematic)
            {
                Vector3 fixedPos = GetComponentInChildren<RagdollHelper>().gameObject.transform.position;
                fixedPos.y = startY;
                transform.position = fixedPos;
            }
            isKinematic = !isKinematic;

            //if (isKinematic)
            //{
            //    GetComponent<MonsterState>().SetState(MonsterState.States.Follow);
            //}
            //else
            //{
            //    GetComponent<MonsterState>().SetState(MonsterState.States.Ragdoll);
            //}
        }
    }
}
