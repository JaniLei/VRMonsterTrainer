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
    }
    Produce[] produce;
    float currentTime;

	void Start()
    {
        produce = new Produce[produceAmount];
        SpawnProduce();
        for (int i = 0; i < produce.Length; i++)
        {
            //produce[i].fullScale = producePrefab.GetComponentInChildren<MeshRenderer>().transform.localScale;
            produce[i].startPos = produce[i].edible.transform.position;
        }
        Invoke("RespawnCheck", growTime);
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        for (int i = 0; i < produce.Length; i++)
        {
            Produce prod = produce[i];
            if (!prod.grown)
            {
                if (prod.mesh.transform.localScale.x < prod.fullScale.x &&
                    prod.mesh.transform.localScale.y < prod.fullScale.y &&
                    prod.mesh.transform.localScale.z < prod.fullScale.z)
                {
                    float growth = currentTime / growTime;
                    Vector3 scaleGrowth = new Vector3(growth * prod.fullScale.x, growth * prod.fullScale.y, growth * prod.fullScale.z);
                    prod.mesh.transform.localScale = scaleGrowth;
                    //prod.edible.transform.position = prod.startPos + new Vector3(0, -(growth / 3), 0);
                }
                else
                {
                    prod.edible.gameObject.layer = LayerMask.NameToLayer("Default");
                    prod.grown = true;
                }
            }
        }
    }
	
    //IEnumerator GrowFruit()
    //{
    //    yield return new WaitForSeconds(growInterval);
    //    if (!fruit.picked && !grown)
    //    {
    //        float growth = growPercent / 100;
    //        fruit.transform.localScale += new Vector3(growth * fullScale.x, growth * fullScale.y, growth * fullScale.z);
    //        if (fruit.transform.localScale.x >= fullScale.x &&
    //            fruit.transform.localScale.y >= fullScale.y &&
    //            fruit.transform.localScale.z >= fullScale.z)
    //        {
    //            grown = true;
    //            fruit.gameObject.layer = LayerMask.NameToLayer("Default");
    //        }
    //        StartCoroutine("GrowFruit");
    //    }
    //    //else
    //    //{
    //    //    SpawnFruit();
    //    //}
    //}
    
    void SpawnProduce()
    {
        for (int i = 0; i < produce.Length; i++)
        {
            produce[i].edible = Instantiate(producePrefab, transform.position /*+ spawnPos[i]*/, producePrefab.transform.rotation, transform).GetComponent<Valve.VR.InteractionSystem.Edible>();
            produce[i].edible.gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
            produce[i].mesh = produce[i].edible.GetComponentInChildren<MeshRenderer>().gameObject;
            produce[i].mesh.transform.position = transform.position + spawnPos[i];
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
            if (produce[i].edible && produce[i].edible.picked && !produce[i].spawning)
                ReSpawnProduce(i);
        }
        StartCoroutine("RespawnCheck");
    }

    IEnumerator ReSpawnProduce(int index)
    {
        produce[index].spawning = true;
        yield return new WaitForSeconds(60);
        produce[index].edible = Instantiate(producePrefab, transform.position /*+ spawnPos[index]*/, producePrefab.transform.rotation, transform).GetComponent<Valve.VR.InteractionSystem.Edible>();
        produce[index].edible.gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
        produce[index].mesh = produce[index].edible.GetComponentInChildren<MeshRenderer>().gameObject;
        produce[index].mesh.transform.position = transform.position + spawnPos[index];
        produce[index].fullScale = produce[index].mesh.transform.localScale;
        produce[index].mesh.transform.localScale = Vector3.zero;
        produce[index].edible.GetComponent<Rigidbody>().isKinematic = true;
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
