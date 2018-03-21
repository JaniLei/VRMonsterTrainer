﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    bool isKinematic = false;

	void Start()
    {
        SetKinematic(true);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SetKinematic(!isKinematic);
    }

    void SetKinematic(bool newValue)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = newValue;
        }
        isKinematic = !isKinematic;
    }
}
