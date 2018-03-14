using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPawsRenderer : MonoBehaviour
{
	private LineRenderer line;
	[SerializeField] private Transform[] bones;

	void Start ()
	{
		line = GetComponent<LineRenderer> ();

		//Testing a bunch of edge case scenarios...

		if (line == null)
		{
			Debug.LogWarning (this + " has a missing LineRenderer! Disabling script...");
			this.enabled = false;
		}

		if (line.positionCount != bones.Length)
		{
			Debug.LogWarning ("Values set in " + this + " and LineRenderer are not equal! Disabling script...");
			this.enabled = false;
		}

		foreach (Transform bone in bones)
		{
			if (bone == null)
			{
				Debug.LogWarning ("Some bones in " + this + " are null! Disabling script...");
				this.enabled = false;
			}
		}

		//If script is still going, everything is good!
		//Enable line renderer if it's disabled.

		if (!line.enabled)
		{
			line.enabled = true;
		}
	}
	
	void Update ()
	{
		for(int i = 0; i < bones.Length; i++)
		{
			line.SetPosition (i, bones[i].position);
		}
	}
}
