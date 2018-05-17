using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramSpinner : MonoBehaviour {

	[SerializeField] private float spinSpeed = 0.0f;
	[SerializeField] private bool rotateZ = false;

	void Update ()
	{
		if (!rotateZ)
		{
			Vector3 rotation = new Vector3 (0.0f, spinSpeed * Time.deltaTime, 0.0f);
			transform.localEulerAngles += rotation;
		}
		else
		{
			Vector3 rotation = new Vector3 (0.0f, 0.0f, spinSpeed * Time.deltaTime);
			transform.localEulerAngles += rotation;
		}
	}
}
