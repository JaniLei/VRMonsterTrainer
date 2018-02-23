﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("Failed to get event manager instance. Add an event manager to the scene.");
            }
            return _instance;
        }
    }

    public delegate void StateEventHandler();
    public event StateEventHandler Pointing, Fetching;
    public GameObject targetObj;


    void Awake ()
    {
        if (_instance == null)
        {
            // if not, set instance to this
            _instance = this;
        }
        // if instance already exists and it's not this
        else if (_instance != this)
        {
            // destroy this, there can only be one instance
            Destroy(gameObject);
        }
    }
	
	public void OnPointing()
    {
        Pointing();
    }

    public void OnFetching()
    {
        Fetching();
    }
}
