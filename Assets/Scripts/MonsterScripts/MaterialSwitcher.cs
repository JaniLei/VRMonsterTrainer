using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
	//Debug stuff
	[SerializeField] private bool debugMode;
	private float timer = 0.0f;
	private int materialScroll = 0;

	//Model and material references
	[SerializeField] private SkinnedMeshRenderer[] models;
	[SerializeField] private Material[] materials;

	/* ----------------------------------------------------------------------- */
	//Check for null references, and
	//disable script when null reference found

	void Start ()
	{
		foreach (SkinnedMeshRenderer model in models)
		{
			if (model == null)
			{
				Debug.LogWarning("Null model reference found in " + this + "!");
				this.enabled = false;
				return;
			}
		}
		foreach (Material mat in materials)
		{
			if (mat == null)
			{
				Debug.LogWarning("Null material reference found in " + this + "!");
				this.enabled = false;
				return;
			}
		}
	}

	/* ----------------------------------------------------------------------- */
	//Public function for changing materials

	public void ChangeMaterial (int i)
	{
		if (i >= 0 && i < materials.Length)
		{
			foreach (SkinnedMeshRenderer model in models)
			{
				model.material = materials [i];
			}
		}
		else
		{
			Debug.Log("Input in " + this + " is higher that material array length! (Input: " + i + " Array length: " + materials.Length + ")");
		}
	}

	/* ----------------------------------------------------------------------- */
	//Debug stuff

	void Update ()
	{
		if (debugMode)
		{
			if (timer > 1.0f)
			{
				materialScroll++;
				if (materialScroll >= materials.Length)
				{
					materialScroll = 0;
				}
				ChangeMaterial(materialScroll);
				timer = 0.0f;
			}
			timer += Time.deltaTime;
		}
	}
}
