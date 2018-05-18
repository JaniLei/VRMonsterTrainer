using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class Door : MonoBehaviour
    {
        public Quaternion startRot;
        public int limitsMin = 0;
        public int limitsMax = -180;

        void Awake()
        {
            //HingeJoint doorHinge = gameObject.AddComponent<HingeJoint>() as HingeJoint;
            HingeJoint doorHinge = GetComponent<HingeJoint>();
            //doorHinge.anchor = new Vector3(0.55f, 0, 0);
            //doorHinge.axis = new Vector3(0, 1, 0);
            doorHinge.useLimits = true;
            JointLimits hingeLimits = new JointLimits();
            hingeLimits.min = limitsMin;
            hingeLimits.max = limitsMax;
            doorHinge.limits = hingeLimits;
            startRot = transform.rotation;
        }
        
    }
}
