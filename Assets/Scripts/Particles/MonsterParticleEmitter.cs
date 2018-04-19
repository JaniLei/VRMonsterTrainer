using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParticleEmitter : MonoBehaviour
{
	[SerializeField] private ParticleSystem particle;
	
	public void EmitParticle (int amount)
	{
		if (particle != null)
		{
			particle.Emit(amount);
		}
		else
		{
			Debug.LogWarning(this + " has no particle system attached!");
		}
	}
}
