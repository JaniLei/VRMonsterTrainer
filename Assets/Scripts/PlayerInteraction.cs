//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlayerInteraction : MonoBehaviour
{
    public Rigidbody attachPoint;
    [HideInInspector] public static GameObject objPointed;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;
    GameObject overlappedObject;
    BoxCollider controllerCollider;
    bool handClosed, holdingFetchable;


    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        controllerCollider = gameObject.AddComponent<BoxCollider>();
        controllerCollider.isTrigger = true;
    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (joint == null && overlappedObject)
            {
                var go = overlappedObject;
                //go.transform.position = attachPoint.transform.position;
                Rigidbody rb = go.GetComponent<Rigidbody>();
                if (rb)
                    rb.useGravity = false;
                var collider = go.GetComponent<Collider>();
                if (collider)
                    collider.enabled = false;
                rb.MovePosition(attachPoint.transform.position - go.transform.position);

                joint = go.AddComponent<FixedJoint>();
                joint.connectedBody = attachPoint;

                if (go.tag == "fetchable")
                    holdingFetchable = true;
            }
            handClosed = true;
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (joint != null)
            {
                var go = joint.gameObject;
                var rigidbody = go.GetComponent<Rigidbody>();
                if (rigidbody)
                    rigidbody.useGravity = true;
                var collider = go.GetComponent<Collider>();
                if (collider)
                    collider.enabled = true;
                Object.DestroyImmediate(joint);
                joint = null;

                var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
                if (origin != null)
                {
                    rigidbody.velocity = origin.TransformVector(device.velocity);
                    rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
                }
                else
                {
                    rigidbody.velocity = device.velocity;
                    rigidbody.angularVelocity = device.angularVelocity;
                }

                rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;

                if (holdingFetchable)
                {
                    Invoke("StartFetching", 2);
                    objPointed = go;
                    holdingFetchable = false;
                }
            }
            handClosed = false;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool bHit = Physics.Linecast(transform.position, transform.forward * 100, out hit);
            if (bHit && hit.transform.gameObject.tag == "bed")
            {
                objPointed = hit.transform.gameObject;
                EventManager.instance.OnPointing();
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractableObject>())
            overlappedObject = other.gameObject;

        if (other.tag == "monster")
        {
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;

            Debug.Log("controller velocity: " + origin.TransformVector(device.velocity).magnitude);
            if (handClosed && origin.TransformVector(device.velocity).magnitude > 2)
            {
                Debug.Log("hit monster");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>())
            overlappedObject = null;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "monster")
        {
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;

            Debug.Log("controller velocity: " + origin.TransformVector(device.velocity).magnitude);
            if (!handClosed && origin.TransformVector(device.velocity).magnitude > 2)
            {
                Debug.Log("petting monster");
            }
        }
    }

    void StartFetching()
    {
        EventManager.instance.OnFetching();
    }
}
