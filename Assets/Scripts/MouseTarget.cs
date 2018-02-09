using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{    
    Rigidbody attachPoint;
    GameObject targetObj;
    FixedJoint joint;
    GameObject go;
    bool hold;


    void Start ()
    {
        GameObject temp_obj = new GameObject();
        temp_obj.transform.parent = null;
        temp_obj.transform.localPosition += Camera.main.transform.forward;
        attachPoint = temp_obj.AddComponent<Rigidbody>();
        attachPoint.isKinematic = true;

        EventManager.instance.Pointing += OnPointing;
    }
	
	void Update ()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 pos = Input.mousePosition;
            //pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 pos = Camera.main.transform.position;

            Ray raycast = new Ray(pos, Camera.main.transform.forward);
            RaycastHit hit;
            bool bHit = Physics.Raycast(raycast, out hit, 1);

            if(bHit && hit.transform.gameObject.tag == "interactable")
            {
                targetObj = hit.transform.gameObject;

                Debug.Log("hit something");
            }
        }
        if (Input.GetMouseButtonUp(0) && targetObj)
        {
            Rigidbody rb = targetObj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            targetObj = null;
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            RayCastInteractable();
            hold = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            hold = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            bool bHit = Physics.Linecast(transform.position, transform.forward * 100, out hit);
            if (bHit && hit.transform.gameObject.tag == "bed")
            {
                Debug.Log("Pointed the bed");
                EventManager.instance.OnPointing();
            }
        }
    }

    void FixedUpdate()
    {
        if (joint == null && hold)
        {
            if (go)
            {
                Vector3 pos = attachPoint.transform.position;
                go.transform.position = pos;

                joint = go.AddComponent<FixedJoint>();
                joint.connectedBody = attachPoint;
                
            }
        }
        if (joint != null && !hold)
        {
            go = joint.gameObject;
            var rigidbody = go.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;
            go = null;

            rigidbody.velocity = attachPoint.velocity;
            rigidbody.angularVelocity = attachPoint.angularVelocity;

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }
        /*
        if (targetObj)
        {
            Vector3 pos = transform.position + Camera.main.transform.forward;

            //targetObj.transform.position = pos;
            Rigidbody rb = targetObj.GetComponent<Rigidbody>();
            rb.MovePosition(pos);
            rb.useGravity = false;
        }*/

        attachPoint.MovePosition(Camera.main.transform.position + Camera.main.transform.forward);
    }

    void RayCastInteractable()
    {
        Ray raycast = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, 1);
        if (bHit && hit.transform.gameObject.GetComponent<InteractableObject>())
        {
            go = hit.transform.gameObject;
        }
    }

    public void OnPointing()
    {
        Debug.Log("player pointing");
    }

    public void OnFetching()
    {
        Debug.Log("player fetching");
    }

}
