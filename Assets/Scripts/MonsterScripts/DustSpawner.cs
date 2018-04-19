using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustSpawner : MonoBehaviour {

    ParticleSystem particleSys;
    public int particleAmount;
    public int spawnCount;
    public int rockAmount;
    public GameObject rockObj;
    public float areaSize;

	void Start ()
    {
        particleSys = gameObject.GetComponent<ParticleSystem>();
	}
	

	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateDust();
        }
	}

    void CreateDust()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Invoke("CreateParticle", Random.Range(0.00f, 1.00f));
            Invoke("CreateRock", Random.Range(0.00f, 1.00f));
        }
    }

    void CreateParticle()
    {
        particleSys.Emit(particleAmount);
    }

    void CreateRock()
    {
        Vector3 spawnPos = transform.position + (Random.Range(-areaSize, areaSize) * transform.right + (Random.Range(-areaSize, areaSize) * transform.up));
        Destroy(Instantiate(rockObj, spawnPos, Quaternion.identity), 5);
    }
}
