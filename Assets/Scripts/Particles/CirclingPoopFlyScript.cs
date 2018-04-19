using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclingPoopFlyScript : MonoBehaviour
{
	private ParticleSystem particle;
	[SerializeField] private float amplitude = 1.0f;
	[SerializeField] private float speed = 1.0f;
	private float progress = 0.0f;

	void Start ()
	{
		particle = GetComponent<ParticleSystem>();
	}
	
	void Update ()
	{
		float tempProgress = progress * Mathf.Deg2Rad;
		float sin = Mathf.Sin(tempProgress);
		float cos = Mathf.Cos(tempProgress);

		particle.transform.localPosition = new Vector3(sin * amplitude, cos * amplitude, 0.0f);

		progress += speed * Time.deltaTime;
		if (progress > 360.0f)
		{
			progress -= 360.0f;
		}
	}
}
