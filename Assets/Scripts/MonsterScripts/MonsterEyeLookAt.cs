using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEyeLookAt : MonoBehaviour {

	public Transform eyeLeft;
	public Transform eyeRight;
	public Transform lookAtTarget;

	private Vector3 rotationFix = new Vector3 (-90.0f, 90.0f, 0.0f);

	void Start ()
	{
		if (eyeLeft == null || eyeRight == null || lookAtTarget == null)
		{
			Debug.LogWarning (this + " has null transforms attached to script!");
			this.enabled = false;
		}
	}
	
	void Update ()
	{
		eyeLeft.LookAt (lookAtTarget);
		eyeRight.LookAt (lookAtTarget);

		eyeLeft.Rotate (rotationFix);
		eyeRight.Rotate (rotationFix);
	}
}
