using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPawsRenderer : MonoBehaviour
{
	private LineRenderer line;
	private Material mat;

	[SerializeField] private Transform[] bones;
	[SerializeField] private bool wiggle = false;
	[SerializeField][Range(0.0f,0.1f)] private float wiggleStrength = 0.0f;
	[SerializeField] private bool offsetTexture = false;
	[SerializeField][Range(0.0f,1.0f)] private float offsetStrength = 0.0f;

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

		//Get the line renderer's material.
		mat = line.material;
	}
	
	void LateUpdate ()
	{
		//Move line renderer's points to bone positions.
		for(int i = 0; i < bones.Length; i++)
		{
			Vector3 wigglePosition = Vector3.zero;
			if (wiggle)
			{
				if (i != 0 && i != bones.Length - 1)
				{
					wigglePosition = new Vector3 (Random.Range (-wiggleStrength, wiggleStrength), Random.Range (-wiggleStrength, wiggleStrength), Random.Range (-wiggleStrength, wiggleStrength));
				}
			}
			line.SetPosition (i, bones[i].position + wigglePosition);
		}

		//Apply material offset.
		if (offsetTexture)
		{
			mat.mainTextureOffset = new Vector2 (Random.Range (-offsetStrength, offsetStrength), 0.0f);
		}
	}
}
