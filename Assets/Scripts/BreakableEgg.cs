using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class BreakableEgg : MonoBehaviour
    {
        public float breakForce = 1.5f;
        public int secondsTillBreak = 180;

        void Start()
        {
            Invoke("Break", secondsTillBreak);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                Break();
        }

        private void OnHandHoverBegin(Hand hand)
        {
            if (hand.GetTrackedObjectVelocity().magnitude > breakForce)
            {
                Break();
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
            }
            FindObjectOfType<MonsterState>().SendMessage("HatchMonster", SendMessageOptions.DontRequireReceiver);
        }
    }
}
