using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(Rigidbody))]
    public class Door : MonoBehaviour
    {
        public Rigidbody connectedTo;
        
        HingeJoint doorHinge;

        void Start()
        {
            doorHinge = gameObject.AddComponent<HingeJoint>() as HingeJoint;
            doorHinge.connectedBody = connectedTo;
            doorHinge.anchor = new Vector3(0.5f, 0, 0);
            doorHinge.axis = new Vector3(0, 1, 0);
            doorHinge = GetComponent<HingeJoint>();
            doorHinge.useLimits = true;
            JointLimits hingeLimits = new JointLimits();
            hingeLimits.min = -90;
            hingeLimits.max = 90;
            doorHinge.limits = hingeLimits;
        }

        //private void HandHoverUpdate(Hand hand)
        //{
        //    if (hand.GetStandardInteractionButton())
        //    {
        //        //float step = 10 * Time.deltaTime;
        //        //Vector3 newDir = Vector3.RotateTowards(transform.forward, hand.transform.position, step, 0.0F);
        //        //Quaternion newRot = Quaternion.LookRotation(newDir);
        //        //newRot.x = 0; newRot.z = 0; newRot.y *= -1;
        //        //rb.MoveRotation(newRot);
        //        //Debug.Log("here");
        //    }
        //}
    }
}
