using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Rigidbody connectedTo;

    HingeJoint doorHinge;
    
	void Start ()
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
	
	void Update ()
    {

	}
}
