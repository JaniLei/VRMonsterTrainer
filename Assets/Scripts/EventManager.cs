using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;
    public delegate void StateEventHandler();
    public event StateEventHandler Pointing, Fetching;


    void Awake ()
    {
        if (instance == null)
        {
            //if not,set instance to this
            instance = this;
        }
        //if instance already exists and it`s not this
        else if (instance != this)
        {
            //destroy this, there can only be one instance
            Destroy(gameObject);
        }
    }
	
	public void OnPointing()
    {
        Pointing();
        Debug.Log("eventmanager point");
    }

    public void OnFetching()
    {
        Fetching();
    }
}
