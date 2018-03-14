using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchFood : MonoBehaviour {

    public LayerMask foodMask;
    int rnd;
    bool foodOnSight = false;
    GameObject foodObj;

    [HideInInspector] public Monster monster;
    [HideInInspector] public List<Vector3> nodes;
    [HideInInspector] public Vector3 movePoint;

    public void Search()
    {

        if (foodOnSight)
        {
            foodOnSight = !monster.EatObject(foodObj);
        }
        else if (Vector3.Distance(transform.position, movePoint) < 0.2f)
        {

            rnd = Random.Range(0, nodes.Count);
            movePoint = (nodes[rnd]);
            Debug.Log("uuspoint");

        }
        else
        {
            monster.MoveTo(movePoint);

            Collider[] collisions = Physics.OverlapSphere(transform.position, 3, foodMask);
            float tempDist = 100;
            foreach (Collider col in collisions)
            {
                if (col.gameObject.tag == "Edible" && !Physics.Linecast(transform.position, col.gameObject.transform.position, monster.ObstacleMask))
                {
                    Debug.Log("ruokaa");
                    tempDist = Vector3.Distance(col.transform.position, transform.position);
                    foodObj = col.gameObject;
                    foodOnSight = true;
                }
            }
        }

    }
}
