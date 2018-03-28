using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    public float growInterval;
    public float growPercent;
    public GameObject fruitPrefab;
    public Vector3 spawnPos;

    bool grown;

    Valve.VR.InteractionSystem.Edible fruit;
    Vector3 fullScale;

	void Start()
    {
        fullScale = fruitPrefab.transform.localScale;
        SpawnFruit();
        StartCoroutine("GrowFruit");
	}
	
    IEnumerator GrowFruit()
    {
        yield return new WaitForSeconds(growInterval);
        if (!fruit.picked && !grown)
        {
            float growth = growPercent / 100;
            fruit.transform.localScale += new Vector3(growth * fullScale.x, growth * fullScale.y, growth * fullScale.z);
            if (fruit.transform.localScale.x >= 1 &&
                fruit.transform.localScale.y >= 1 &&
                fruit.transform.localScale.z >= 1)
            {
                grown = true;
                fruit.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
        else
        {
            SpawnFruit();
        }
        StartCoroutine("GrowFruit");
    }

    void SpawnFruit()
    {
        fruit = Instantiate(fruitPrefab, transform.position + spawnPos, Quaternion.identity/*, transform*/).GetComponent<Valve.VR.InteractionSystem.Edible>();
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
