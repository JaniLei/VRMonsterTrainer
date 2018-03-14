using UnityEngine;
using System.Collections;



[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlayerInteraction : MonoBehaviour
{
    [HideInInspector] public static GameObject objPointed;
    public Rigidbody attachPoint;
    public double interactionVelocityMin = 2;

    SteamVR_TrackedObject trackedObj;
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
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (overlappedObject)
            {
                overlappedObject.SendMessage("AttachToHand", attachPoint, SendMessageOptions.DontRequireReceiver);
        
                
        
                if (overlappedObject.tag == "fetchable")
                    holdingFetchable = true;
            }
            handClosed = true;
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Grip))
        {
            if (overlappedObject)
            {
                var rigidbody = overlappedObject.GetComponent<Rigidbody>();
        
                overlappedObject.SendMessage("DetachFromHand", SendMessageOptions.DontRequireReceiver);
                
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
                    objPointed = overlappedObject;
                    holdingFetchable = false;
                }
            }
            handClosed = false;
        }
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) || Input.GetKeyDown(KeyCode.E))
        {
            //Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            bool bHit = Physics.Linecast(transform.position, transform.forward * 100, out hit);
            if (bHit && hit.transform.gameObject.tag == "bed")
            {
                EventManager.instance.targetObj = hit.transform.gameObject;
                EventManager.instance.OnPointing();
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractableObject>())
            overlappedObject = other.gameObject;
    
        if (other.tag == "Monster")
        {
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
    
            Debug.Log("controller velocity: " + origin.TransformVector(device.velocity).magnitude);
            if (handClosed && origin.TransformVector(device.velocity).magnitude > interactionVelocityMin)
            {
                //if (boxing)
                //{
                //
                //}
                //else
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
        if (other.tag == "Monster")
        {
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
    
            Debug.Log("controller velocity: " + origin.TransformVector(device.velocity).magnitude);
            if (!handClosed && origin.TransformVector(device.velocity).magnitude > interactionVelocityMin)
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
