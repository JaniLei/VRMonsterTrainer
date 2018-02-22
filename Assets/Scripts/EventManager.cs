using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;
    public delegate void StateEventHandler();
    public event StateEventHandler Pointing, Fetching, BoxingStart, BoxingEnd;
    public GameObject targetObj;


    void Awake ()
    {
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // if instance already exists and it's not this
        else if (instance != this)
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

    public void OnBoxingStart()
    {
        BoxingStart();
    }

    public void OnBoxingEnd()
    {
        BoxingEnd();
    }
}
