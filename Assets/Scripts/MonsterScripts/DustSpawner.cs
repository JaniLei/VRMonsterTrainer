using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create dust particles, spawn small rocks and change gravity temporarily

public class DustSpawner : MonoBehaviour {

    ParticleSystem particleSys;
    public int particleAmount;
    public int rockAmount;
    public int spawnCount;
    public float shakeAmount;
    public List<GameObject> rockObjects;
    public float areaSize;
    public float overTime;
    float dustTimer;
    int nextDustSpawn;

    Vector3 defaultGravity;
    AudioSource audioSource;

    public GameObject tree;


	void Start ()
    {
        particleSys = gameObject.GetComponent<ParticleSystem>();
        defaultGravity = Physics.gravity;
        nextDustSpawn = Random.Range(60, 300);
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	

	void Update ()
    {
        dustTimer += Time.deltaTime;
		if (Input.GetKey(KeyCode.AltGr) && (Input.GetKeyDown(KeyCode.Alpha1)))
        {
            CreateDust();
        }
        if (dustTimer > nextDustSpawn)
        {
            dustTimer = 0;
            nextDustSpawn = Random.Range(60, 300);
            CreateDust();
        }
	}

    void CreateDust()
    {
        audioSource.Play();
        tree.GetComponent<Animator>().speed = 5;
        Invoke("RestoreAnimation", overTime);
        for (int i = 0; i < spawnCount; i++)
        {
            Invoke("CreateParticle", Random.Range(0.00f, overTime));
            Invoke("CreateRock", Random.Range(0.00f, overTime));
        }
        for (int i = 0; i < shakeAmount; i++)
        {
            Invoke("ShakeObjects", Random.Range(0.00f, overTime));
        }
    }

    void ShakeObjects()
    {
        Vector3 tempGravity = new Vector3(Random.Range(-20,20), Random.Range(20, 35), Random.Range(-20,20));
        Physics.gravity = tempGravity;
        Invoke("RestoreGravity", 0.05f);
    }

    void RestoreGravity()
    {
        Physics.gravity = defaultGravity;
    }

    void RestoreAnimation()
    {
        tree.GetComponent<Animator>().speed = 1;
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
