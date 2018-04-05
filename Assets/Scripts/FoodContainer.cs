using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : MonoBehaviour
{
    public int objectLimit, containerLimit;
    public float spawnInterval;
    public GameObject objectToSpawn;
    public Vector3 spawnPos;

    int objsInContainer;
    List<GameObject> spawnedObjs = new List<GameObject>();

	void Start()
    {
        if (objectLimit == 0)
            objectLimit = 6;
        if (spawnInterval == 0)
            spawnInterval = 60;
        StartCoroutine("SpawnObject");
    }

    IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(spawnInterval);
        for (int i = 0; i < spawnedObjs.Count; i++)
        {
            if (spawnedObjs[i] == null)
                spawnedObjs.RemoveAt(i);
        }
        var lid = GetComponentInChildren<Valve.VR.InteractionSystem.Door>();
        if (Quaternion.Angle(lid.transform.rotation, lid.startRot) < 1) // lid closed
        {
            if (spawnedObjs.Count < objectLimit && objsInContainer < containerLimit)
            {
                spawnedObjs.Add(Instantiate(objectToSpawn, transform.position + spawnPos, objectToSpawn.transform.rotation));
                //objsInContainer++;
            }
        }
        else
        {
            Debug.Log("Lid is open");
        }
        StartCoroutine("SpawnObject");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Valve.VR.InteractionSystem.Edible>())
        {
            objsInContainer++;
            //Debug.Log("objs in container :" + objsInContainer);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Valve.VR.InteractionSystem.Edible>())
        {
            objsInContainer--;
            //Debug.Log("objs in container :" + objsInContainer);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + spawnPos, 0.05f);
    }
}
