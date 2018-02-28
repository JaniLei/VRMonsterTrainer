using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEyeLookAt : MonoBehaviour {

	public Transform eyeLeft;
	public Transform eyeRight;
	public Transform lookAtTarget;

	void Start ()
	{
		if (eyeLeft == null || eyeRight == null || lookAtTarget == null)
		{
			this.enabled = false;
		}
	}
	
	void Update ()
	{
		eyeLeft.LookAt (lookAtTarget);
		eyeRight.LookAt (lookAtTarget);
	}
}
