using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public float growInterval;
    public float growPercent;
    public float growTime = 60;
    public GameObject fruitPrefab;
    public Vector3 spawnPos;

    bool grown;

    Valve.VR.InteractionSystem.Edible fruit;
    Vector3 fullScale;
    float currentTime;

	void Start()
    {
        fullScale = fruitPrefab.transform.localScale;
        SpawnFruit();
        //StartCoroutine("GrowFruit");
	}

    void Update()
    {
        currentTime += Time.deltaTime;
        if (fruit.transform.localScale.x < fullScale.x &&
            fruit.transform.localScale.y < fullScale.y &&
            fruit.transform.localScale.z < fullScale.z)
        {
            float growth = currentTime / growTime;
            Vector3 scaleGrowth = new Vector3(growth * fullScale.x, growth * fullScale.y, growth * fullScale.z);
            fruit.transform.localScale = scaleGrowth;
        }
    }
	
    IEnumerator GrowFruit()
    {
        yield return new WaitForSeconds(growInterval);
        if (!fruit.picked && !grown)
        {
            float growth = growPercent / 100;
            fruit.transform.localScale += new Vector3(growth * fullScale.x, growth * fullScale.y, growth * fullScale.z);
            if (fruit.transform.localScale.x >= fullScale.x &&
                fruit.transform.localScale.y >= fullScale.y &&
                fruit.transform.localScale.z >= fullScale.z)
            {
                grown = true;
                fruit.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            StartCoroutine("GrowFruit");
        }
        //else
        //{
        //    SpawnFruit();
        //}
    }

    void SpawnFruit()
    {
        fruit = Instantiate(fruitPrefab, transform.position + spawnPos, fruitPrefab.transform.rotation, transform).GetComponent<Valve.VR.InteractionSystem.Edible>();
        fruit.gameObject.layer = LayerMask.NameToLayer("IgnoreHand");
        fruit.transform.localScale = Vector3.zero;
        fruit.GetComponent<Rigidbody>().isKinematic = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + spawnPos, 0.05f);
    }
}
