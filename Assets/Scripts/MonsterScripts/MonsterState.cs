using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour {

    public enum States { Follow, Fetch, Sleep, Search} //states for the monster
    States currentState = States.Follow;
    Monster monster;
    SearchFood search;
    MonsterStats stats;
    Fetch fetch;
    float statTimer;
    public float gameSpeed;
    public GameObject fetchObj;
    public float followDistance;

    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        search = gameObject.GetComponent<SearchFood>();
        stats = gameObject.GetComponent<MonsterStats>();
        fetch = gameObject.GetComponent<Fetch>();
        //Update other objects

        EventManager.instance.Fetching += OnFetching;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //FOR TESTING
        {
            OnFetching();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentState = States.Sleep;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentState = States.Search;
        }
      


        switch (currentState)
        {
            case States.Follow:
                monster.FollowPlayer(followDistance);
                break;
            case States.Fetch:
                fetch.FetchObject(fetchObj);
                break;
            case States.Sleep:
                monster.GoSleep();
                break;
            case States.Search:
                search.Search();
                break;
        }

        /*
        statTimer += Time.deltaTime; //Timer for hunger and fatigue
        if (statTimer > gameSpeed)
        {
            stats.UpdateStats();
            statTimer = 0;
        }*/
        
    }

    public void SetState(States _state)
    {
        currentState = _state;
    }

    public void OnFetching()
    {
        StartFetch(fetchObj);
    }

    public void StartFetch(GameObject fObj)
    {
        currentState = States.Fetch;
        fetchObj = fObj;
    }


}
