using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{    
    Rigidbody attachPoint;
    GameObject targetObj;
    FixedJoint joint;
    GameObject go;
    bool hold, holdingFetchable;


    void Start ()
    {
        GameObject temp_obj = new GameObject();
        temp_obj.transform.parent = null;
        temp_obj.transform.localPosition += Camera.main.transform.forward;
        attachPoint = temp_obj.AddComponent<Rigidbody>();
        attachPoint.isKinematic = true;
    }
	
	void Update ()
    {
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
                PlayerInteraction.objPointed = hit.transform.gameObject;
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

            PlayerInteraction.objPointed = go;
            //Invoke("StartFetching", 2);
            StartFetching();

            joint = null;
            go = null;

            rigidbody.velocity = attachPoint.velocity;
            rigidbody.angularVelocity = attachPoint.angularVelocity;

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
            
        }

        attachPoint.MovePosition(Camera.main.transform.position + Camera.main.transform.forward);
    }

    void RayCastInteractable()
    {
        Ray raycast = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, 1);
        if (bHit && hit.transform.gameObject.GetComponent<InteractableObject>())
            go = hit.transform.gameObject;
    }
    
    void StartFetching()
    {
        EventManager.instance.OnFetching();
    }

}
