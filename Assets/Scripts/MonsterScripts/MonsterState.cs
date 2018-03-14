using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour {

    public enum States { Follow, Fetch, Sleep, Search, Boxing, Dead} //states for the monster
    States currentState = States.Follow;
    Monster monster;
    SearchFood search;
    MonsterStats stats;
    Fetch fetch;
    Boxing boxing;
    float statTimer;
    public float gameSpeed;
    public GameObject fetchObj;
    public float followDistance;


    public Animator anim;

    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        search = gameObject.GetComponent<SearchFood>();
        stats = gameObject.GetComponent<MonsterStats>();
        fetch = gameObject.GetComponent<Fetch>();
        boxing = gameObject.GetComponent<Boxing>();

        fetch.stats = stats;
        fetch.state = this;
        fetch.monster = monster;
        search.monster = monster;
        stats.state = this;
        stats.monster = monster;
        boxing.monster = monster;
        boxing.player = monster.mainPlayer;
        boxing.obstacleMask = monster.ObstacleMask;
        boxing.stats = stats;



        EventManager.instance.Fetching += OnFetching;
        EventManager.instance.Pointing += OnPointing;
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
        else if(Input.GetKey(KeyCode.Alpha4))
        {
            currentState = States.Boxing;
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
            case States.Boxing:
                boxing.DoBoxing();
                break;
            case States.Dead:
                //Do dying stuff
                break;
        }

        
        statTimer += Time.deltaTime; //Timer for hunger and fatigue
        if (statTimer > gameSpeed)
        {
            stats.UpdateStats();
            statTimer = 0;
        }
        
    }

    public void SetState(States _state)
    {
        currentState = _state;
    }

    public void OnFetching()
    {
        StartFetch(EventManager.instance.targetObj);
    }
    public void OnPointing()
    {
        monster.GoSleep();
    }
    public void StartFetch(GameObject fObj)
    {
        currentState = States.Fetch;
        fetchObj = fObj;
    }


}
