using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletPowerToggle : MonoBehaviour
{
	//Setting up variables.
	[SerializeField] private GameObject tablet;
	[SerializeField] private GameObject screen;
	private Renderer tabletRenderer;
	private bool powerState = false;
	private Color emissionEnabled;
	private Color emissionDisabled = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	void Start ()
	{
		//If script has null references, disable script and stop execution.
		if (tablet == null || screen == null)
		{
			Debug.LogWarning (this + " is missing references!");
			this.enabled = false;
			return;
		}

		//Get tablet renderer to toggle its emission color on and off.
		tabletRenderer = tablet.GetComponent<Renderer>();
		emissionEnabled = tabletRenderer.material.GetColor("_EmissionColor");

		//Turn the tablet off at the start of the game.
		if (!powerState)
		{
			PowerSwitch(false);
		}
	}
	
	public bool Power //Call this to get the current power state or set the tablet on/off.
	{
		get {return powerState;}
		set
		{
			if (value && !powerState) //If tablet is off and a call is telling to turn it on...
			{
				PowerSwitch(true); //Turn the tablet on.
			}
			else if (!value && powerState) //If tablet is on and a call is telling to turn it off...
			{
				PowerSwitch(false); //Turn the tablet off.
			}
		}
	}

	private void PowerSwitch (bool b) //This does the actual switching. True is on, false is off.
	{
		if (b)
		{
			tabletRenderer.material.SetColor("_EmissionColor", emissionEnabled);
			screen.SetActive(true);
			powerState = true;
		}
		else
		{
			tabletRenderer.material.SetColor("_EmissionColor", emissionDisabled);
			screen.SetActive(false);
			powerState = false;
		}
	}
}
