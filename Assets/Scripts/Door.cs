﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class Door : MonoBehaviour
    {
        public Quaternion startRot;

        void Awake()
        {
            //HingeJoint doorHinge = gameObject.AddComponent<HingeJoint>() as HingeJoint;
            HingeJoint doorHinge = GetComponent<HingeJoint>();
            //doorHinge.anchor = new Vector3(0.55f, 0, 0);
            //doorHinge.axis = new Vector3(0, 1, 0);
            doorHinge.useLimits = true;
            JointLimits hingeLimits = new JointLimits();
            hingeLimits.min = 0;
            hingeLimits.max = -180;
            doorHinge.limits = hingeLimits;
            startRot = transform.rotation;
        }
        
    }
}
