//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VR_ObjectInteraction : MonoBehaviour
{
    public Rigidbody attachPoint;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;
    GameObject overlappedObject;
    BoxCollider controllerCollider;


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
        if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (overlappedObject)
            {
                var go = overlappedObject;
                go.transform.position = attachPoint.transform.position;

                joint = go.AddComponent<FixedJoint>();
                joint.connectedBody = attachPoint;
            }
        }
        else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            var go = joint.gameObject;
            var rigidbody = go.GetComponent<Rigidbody>();
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
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "interactable")
            overlappedObject = other.gameObject;
        else if (other.tag == "monster")
        {
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;

            Debug.Log("controller velocity: " + origin.TransformVector(device.velocity).magnitude);
            if (origin.TransformVector(device.velocity).magnitude > 1)
            {
                Debug.Log("hit monster");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "interactable")
            overlappedObject = null;
    }
}
