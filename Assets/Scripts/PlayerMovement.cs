using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.02f;
    //Rigidbody rb;

    void Start ()
    {
        //rb = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        Quaternion newRot = Camera.main.transform.rotation;
        newRot.z = 0;
        newRot.x = 0;
        transform.rotation = newRot;

        float hSpeed = Input.GetAxis("Horizontal") * speed;
        float vSpeed = Input.GetAxis("Vertical") * speed;
        if (hSpeed != 0 || vSpeed != 0)
        {
            Vector3 newPos = new Vector3(hSpeed, 0, vSpeed);
            transform.Translate(newPos);
            //rb.MovePosition(transform.position + newPos);
        }

        Camera.main.transform.position = transform.position;
    }
}
