using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustSpawner : MonoBehaviour {

    ParticleSystem particleSys;
    public int particleAmount;
    public int rockAmount;
    public int spawnCount;
    public List<GameObject> rockObjects;
    public float areaSize;
    public float overTime;


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
            Invoke("CreateParticle", Random.Range(0.00f, overTime));
            Invoke("CreateRock", Random.Range(0.00f, overTime));
        }
    }

    void CreateParticle()
    {
        particleSys.Emit(particleAmount);
    }

    void CreateRock()
    {
        GameObject rockObj = rockObjects[Random.Range(0, rockObjects.Count)];
        Vector3 spawnPos = transform.position + (Random.Range(-areaSize, areaSize) * transform.right + (Random.Range(-areaSize, areaSize) * transform.up));
        Destroy(Instantiate(rockObj, spawnPos, Quaternion.identity), 5);
    }
}
