using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class BreakableEgg : MonoBehaviour
    {
        Player player;
        bool broken;

        public float breakForce = 1.2f;
        public int secondsTillBreak = 180;

        void Start()
        {
            Invoke("Break", secondsTillBreak);
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            if (!broken)
            {
                if (Input.GetKeyDown(KeyCode.N))
                    Break();

                foreach (var hand in player.hands)
                {
                    if (hand.currentAttachedObject != null)
                    {
                        if (hand.currentAttachedObject.GetComponent<BoxingGloves>() || hand.currentAttachedObject.GetComponent<Fetchable>() || hand.currentAttachedObject.tag == "Mallet")
                        {
                            Collider otherColl = hand.currentAttachedObject.GetComponent<Collider>();
                            if (!otherColl)
                                otherColl = hand.currentAttachedObject.GetComponentInChildren<Collider>();
                            if (GetComponent<Collider>().bounds.Intersects(otherColl.bounds))
                            {
                                if (hand.GetTrackedObjectVelocity().magnitude > breakForce)
                                {
                                    Break();
                                }
                                //Break();
                            }
                        }
                    }
                }
            }
        }

        private void OnHandHoverBegin(Hand hand)
        {
            if (!broken)
            {
                if (hand.GetTrackedObjectVelocity().magnitude > breakForce)
                {
                    Break();
                }
            }
        }

        void OnCollisionEnter(Collision coll)
        {
            if (!broken)
            {
                if (coll.relativeVelocity.magnitude > breakForce * 3)
                {
                    Break();
                }
            }
        }

        void Break()
        {
            GetComponent<MeshCollider>().enabled = false;
            Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].useGravity = true;
                rbs[i].isKinematic = false;
                rbs[i].GetComponent<MeshCollider>().enabled = true;
            }
            FindObjectOfType<MonsterState>().SendMessage("HatchMonster", SendMessageOptions.DontRequireReceiver);
            broken = true;
        }
    }
}
