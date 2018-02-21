using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObject : MonoBehaviour
{

    public Monster monster;

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Edible")
        {
            monster.EatObject(col.gameObject);
        }
        else if (col.gameObject.tag == "Stick")
        {

        }
    }
}