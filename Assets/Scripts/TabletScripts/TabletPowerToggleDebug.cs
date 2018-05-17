using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Require the tablet power toggle script, since this script calls it.
[RequireComponent(typeof(TabletPowerToggle))]

public class TabletPowerToggleDebug : MonoBehaviour
{
	[SerializeField] private bool toggleState = false;
	private bool toggleStateCheck = false;
	private TabletPowerToggle tablet;

	void Start ()
	{
		tablet = GetComponent<TabletPowerToggle>();
		toggleState = tablet.Power;
	}
	
	void Update ()
	{
		if (toggleState != toggleStateCheck)
		{
			tablet.Power = toggleState;
			toggleStateCheck = toggleState;
		}
	}
}
