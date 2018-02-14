using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterForTesting : MonoBehaviour
{
    GameObject objFollowing;
    Rigidbody rb;


	void Start ()
    {
        EventManager.instance.Pointing += OnPointing;
        EventManager.instance.Fetching += OnFetching;
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
		if (objFollowing)
        {
            Vector3 targetPos = objFollowing.transform.position;
            targetPos.y += 0.5f;
            Vector3 dir = Vector3.Normalize(targetPos - transform.position);
            rb.MovePosition(transform.position + (dir * 0.1f));

            if (Vector3.Magnitude(objFollowing.transform.position - transform.position) <= 0.6f)
                objFollowing = null;
        }
	}

    void OnPointing()
    {
        if (PlayerInteraction.objPointed != this)
            objFollowing = PlayerInteraction.objPointed;
    }

    void OnFetching()
    {
        if (PlayerInteraction.objPointed != this)
            objFollowing = PlayerInteraction.objPointed;
    }

}
