/* old */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableObject : MonoBehaviour
{
    Rigidbody rb;
    Collider coll;
    FixedJoint joint;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    void AttachToHand(Rigidbody attachPoint)
    {
        Debug.Log("attached to hand");
        if (rb)
        {
            //rb.isKinematic = true;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.None;
        }
        if (coll)
            coll.enabled = false;

        rb.MovePosition(attachPoint.transform.position - transform.position);

        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = attachPoint;
    }

    void DetachFromHand()
    {
        Debug.Log("detached from hand");
        Object.DestroyImmediate(joint);
        joint = null;
        if (rb)
        {
            //rb.isKinematic = false;
            rb.useGravity = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        var collider = GetComponent<Collider>();
        if (collider)
            collider.enabled = true;
    }
}
