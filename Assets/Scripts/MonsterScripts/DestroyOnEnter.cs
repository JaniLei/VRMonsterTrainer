using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnter : MonoBehaviour {
    


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Edible")
        {
            Destroy(col);
        }
    }

}
