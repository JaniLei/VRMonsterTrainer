using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public float growTime = 60;
    public GameObject producePrefab;
    public int produceAmount;
    public Vector3[] spawnPos;

    struct Produce
    {
        public Valve.VR.InteractionSystem.Edible edible;
        public GameObject mesh;
        public Vector3 fullScale;
        public Vector3 startPos;
        public bool grown;
        public bool spawning;
        public float currentTime;
    }
    Produce[] produce;

    List<Vector3> freeSpots;

	void Start()
    {
        produce = new Produce[produceAmount];
        freeSpots = new List<Vector3>();
        freeSpots.AddRange(spawnPos);
        SpawnProduce();
        StartCoroutine("RespawnCheck");
    }

    void Update()
    {
        for (int i = 0; i < produce.Length; i++)
        {
            produce[i].currentTime += Time.deltaTime;

            if (!produce[i].grown)
            {
                if (produce[i].mesh.transform.localScale.x < produce[i].fullScale.x &&
                    produce[i].mesh.transform.localScale.y < produce[i].fullScale.y &&
                    produce[i].mesh.transform.localScale.z < produce[i].fullScale.z)
                {
                    float growth = produce[i].currentTime / growTime;
                    Vector3 scaleGrowth = new Vector3(growth * produce[i].fullScale.x, growth * produce[i].fullScale.y, growth * produce[i].fullScale.z);
                    produce[i].mesh.transform.localScale = scaleGrowth;
                }
                else
                {
                    Collider[] colls = produce[i].mesh.GetComponentsInChildren<Collider>();
                    for (int c = 0; c < colls.Length; c++)
                    {
                        colls[c].gameObject.layer = LayerMask.NameToLayer("Default");
                    }
                    //produce[i].edible.gameObject.layer = LayerMask.NameToLayer("Default");
                    produce[i].grown = true;
                }
            }
        }
    }

    Vector3 FindRandomSpawnSpot()
    {
        int randIndex = Random.Range(0, freeSpots.Count);
        Vector3 randomPos = freeSpots[randIndex];
        freeSpots.RemoveAt(randIndex);
        return randomPos;
    }

    void SpawnProduce()
    {
        for (int i = 0; i < produce.Length; i++)
        {
            produce[i].edible = Instantiate(producePrefab, transform.position, producePrefab.transform.rotation, transform).GetComponent<Valve.VR.InteractionSystem.Edible>();
            
            //produce[i].edible.gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
            produce[i].mesh = produce[i].edible.transform.Find("Mesh").gameObject;
            Collider[] colls = produce[i].mesh.GetComponentsInChildren<Collider>();
            for (int c = 0; c < colls.Length; c++)
            {
                colls[c].gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
            }
            produce[i].startPos = FindRandomSpawnSpot();
            produce[i].mesh.transform.position = transform.position + produce[i].startPos;//spawnPos[i];
            produce[i].fullScale = produce[i].mesh.transform.localScale;
            produce[i].mesh.transform.localScale = Vector3.zero;
            produce[i].edible.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    IEnumerator RespawnCheck()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < produce.Length; i++)
        {
            if (produce[i].edible.picked && !produce[i].spawning)
            {
                StartCoroutine("RespawnProduce", i);
                freeSpots.Add(produce[i].startPos);
                Debug.Log("Respawning produce....");
            }
        }
        StartCoroutine("RespawnCheck");
    }

    IEnumerator RespawnProduce(int index)
    {
        produce[index].spawning = true;
        yield return new WaitForSeconds(5);
        Debug.Log("Respawn produce");
        produce[index].edible = Instantiate(producePrefab, transform.position, producePrefab.transform.rotation, transform).GetComponent<Valve.VR.InteractionSystem.Edible>();
        //produce[index].edible.gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
        produce[index].mesh = produce[index].edible.transform.Find("Mesh").gameObject;
        Collider[] colls = produce[index].mesh.GetComponentsInChildren<Collider>();
        for (int c = 0; c < colls.Length; c++)
        {
            colls[c].gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
        }
        produce[index].startPos = FindRandomSpawnSpot();
        produce[index].mesh.transform.position = transform.position + produce[index].startPos;
        produce[index].fullScale = produce[index].mesh.transform.localScale;
        produce[index].mesh.transform.localScale = Vector3.zero;
        produce[index].edible.GetComponent<Rigidbody>().isKinematic = true;
        produce[index].currentTime = 0;
        produce[index].grown = false;
        produce[index].spawning = false;
    }

    void OnDrawGizmos()
    {
        if (spawnPos.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < spawnPos.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + spawnPos[i], 0.01f);
            }
        }
    }
}
