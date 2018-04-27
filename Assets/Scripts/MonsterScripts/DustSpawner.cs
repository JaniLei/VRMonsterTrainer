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

    Vector3 defaultGravity;

	void Start ()
    {
        particleSys = gameObject.GetComponent<ParticleSystem>();
        defaultGravity = Physics.gravity;
        Debug.Log(defaultGravity);
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
        Invoke("ShakeObjects", Random.Range(0.00f, overTime));
        Invoke("ShakeObjects", Random.Range(0.00f, overTime));
    }

    void ShakeObjects()
    {
        Physics.gravity = -Vector3.down * 20;
        Invoke("RestoreGravity", 0.05f);
    }

    void RestoreGravity()
    {
        Physics.gravity = defaultGravity;
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
