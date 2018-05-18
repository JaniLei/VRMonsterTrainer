using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerState : MonoBehaviour
{
    public float pointingRange = 4.5f;

    Valve.VR.InteractionSystem.Hand hand;
    Animator anim;
    enum HandState
    {
        Relaxed = 1,
        Point = 2,
        Grab = 3,
        Punch = 4
    }
    HandState currentState;

    float axis;
    float pointingTime;

	void Start()
    {
        hand = GetComponentInParent<Valve.VR.InteractionSystem.Hand>();
        //if (hand.startingHandType == Valve.VR.InteractionSystem.Hand.HandType.Left)
        //    transform.localScale = new Vector3(-1, 1, 1);

        anim = GetComponent<Animator>();
	}
	
	void Update()
    {
        if (hand.controller != null)
        {
            axis = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                axis = 0.85f;
            else if (Input.GetMouseButtonUp(0))
                axis = 0;
        }
        

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    axis = 0.1f;
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //    axis = 0.6f;
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //    axis = 0.85f;
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //    axis = 1;
        
        if (axis < 0.1f)
            SetHandState(HandState.Relaxed);
        else if (axis < 0.81f)
            SetHandState(HandState.Point);
        else if (axis < 0.9f)
            SetHandState(HandState.Grab);
        else
            SetHandState(HandState.Punch);


        if (currentState == HandState.Point)
        {
            //pointingTime += Time.deltaTime;
            //if (pointingTime >= 0.2f)
            //{
            //    PointToBed();
            //    pointingTime = 0;
            //}
            PointToBed();
        }
    }

    void SetHandState(HandState newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            SetAnimation((int)currentState);
        }
    }

    void SetAnimation(int i)
    {
        anim.SetInteger("State", i);
    }

    void PointToBed()
    {
        RaycastHit hit;
        bool bHit = Physics.Linecast(transform.position, transform.forward * pointingRange, out hit);
        if (bHit && hit.transform.gameObject.tag == "Bed")
        {
            EventManager.instance.targetObj = hit.transform.gameObject;
            EventManager.instance.OnPointing();
        }
    }
}
