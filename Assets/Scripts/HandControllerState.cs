using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerState : MonoBehaviour
{
    Valve.VR.InteractionSystem.Hand hand;
    Animator anim;

    float axis = 0;

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
        
        if (axis < 0.3f)
        {
            anim.SetInteger("State", 1);
        }
        else if (axis < 0.8f)
        {
            anim.SetInteger("State", 2);
        }
        else if (axis < 0.9f)
        {
            anim.SetInteger("State", 3);
        }
        else
        {
            anim.SetInteger("State", 4);
        }
	}
}
