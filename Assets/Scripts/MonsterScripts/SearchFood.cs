using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchFood : MonoBehaviour {

    public Monster monster;
    public List<Vector3> nodes;
    public Vector3 movePoint;
    public LayerMask foodMask;
    int rnd;
    bool foodOnSight = false;
    GameObject foodObj;
    public void Search()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, 3, foodMask);


        if (collisions.Length > 0)
        {
            float tempDist = 100;
            foreach (Collider col in collisions)
            {
                if (col.gameObject.tag == "Edible" && Vector3.Distance(col.transform.position, transform.position) < tempDist && !Physics.Raycast(transform.position, col.gameObject.transform.position, monster.ObstacleMask))
                {
                    Debug.Log("löyty ruokaa");
                    tempDist = Vector3.Distance(col.transform.position, transform.position);
                    foodObj = col.gameObject;
                    foodOnSight = true;
                    movePoint = foodObj.transform.position;
                }
            }
        }

        if (foodOnSight)
        {
            monster.MoveTo(foodObj.transform.position);
        }
        else if (Vector3.Distance(transform.position, movePoint) < 0.2f)
        {

            rnd = Random.Range(0, nodes.Count);
            movePoint = (nodes[rnd]);
            {
                /*float tempDist = 100;
                foreach (Collider col in collisions)
                {
                    if (Vector3.Distance(col.transform.position, transform.position) < tempDist && !Physics.Linecast(transform.position, col.gameObject.transform.position, monster.ObstacleMask))
                    {
                        tempDist = Vector3.Distance(col.transform.position, transform.position);
                        foodObj = col.gameObject;
                        foodOnSight = true;
                    }
                }
                if (!foodOnSight)
                {
                    rnd = Random.Range(0, nodes.Count);
                    movePoint = (nodes[rnd]);
                }*/
            }

        }
        else
        {
            monster.MoveTo(movePoint);
        }

    }
}
