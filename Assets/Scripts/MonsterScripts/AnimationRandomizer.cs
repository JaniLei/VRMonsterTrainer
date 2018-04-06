using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomizer : StateMachineBehaviour
{
	[SerializeField] private string parameterName;
	[SerializeField] private Vector2 range;

	override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
	{
		if (parameterName != null)
		{
			animator.SetInteger(parameterName, Mathf.RoundToInt(Random.Range(range.x, range.y)));
		}
	}
}
