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
        public Vector3 fullScale;
        public Vector3 startPos;
    }
    Produce[] produce;
    float currentTime;

	void Start()
    {
        produce = new Produce[produceAmount];
        SpawnProduce();
        for (int i = 0; i < produce.Length; i++)
        {
            produce[i].fullScale = producePrefab.transform.localScale;
            produce[i].startPos = produce[i].edible.transform.position;
        }
        //StartCoroutine("GrowFruit");
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        for (int i = 0; i < produce.Length; i++)
        {
            Produce prod = produce[i];
            if (prod.edible)
            {
                if (prod.edible.transform.localScale.x < prod.fullScale.x &&
                    prod.edible.transform.localScale.y < prod.fullScale.y &&
                    prod.edible.transform.localScale.z < prod.fullScale.z)
                {
                    float growth = currentTime / growTime;
                    Vector3 scaleGrowth = new Vector3(growth * prod.fullScale.x, growth * prod.fullScale.y, growth * prod.fullScale.z);
                    prod.edible.transform.localScale = scaleGrowth;
                    prod.edible.transform.position = prod.startPos + new Vector3(0, -(growth / 3), 0);
                }
                else
                {
                    prod.edible.gameObject.layer = LayerMask.NameToLayer("Default");
                    prod.edible = null;
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
            produce[i].edible = Instantiate(producePrefab, transform.position + spawnPos[i], producePrefab.transform.rotation, transform).GetComponent<Valve.VR.InteractionSystem.Edible>();
            produce[i].edible.gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
            produce[i].edible.transform.localScale = Vector3.zero;
            produce[i].edible.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void OnDrawGizmos()
    {
        if (spawnPos.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < spawnPos.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + spawnPos[i], 0.05f);
            }
        }
    }
}
