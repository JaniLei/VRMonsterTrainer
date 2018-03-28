using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour {

    public enum States { Hatching, Follow, Fetch, Sleep, Search, Boxing, Pooping, Whine, Exit, Dead} //states for the monster
    public enum animStates {Walking, EatHand, EatGround, Idle, Dead, Sleep, Petting, Poop, Lift, Sniff }
    States currentState = States.Hatching;
    animStates animationState = animStates.Idle;
    Monster monster;
    SearchFood search;
    MonsterStats stats;
    Fetch fetch;
    Boxing boxing;
    float statTimer;
    public float gameSpeed;
    public GameObject fetchObj;
    public float followDistance;
    public float hatchTime = 10;
    public GameObject hatchObject;
    public Vector3 exitPoint;
    float hTimer;


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
        search.fetch = fetch;
        search.state = this;
        stats.state = this;
        stats.monster = monster;
        boxing.monster = monster;
        boxing.player = monster.mainPlayer;
        boxing.obstacleMask = monster.ObstacleMask;
        boxing.stats = stats;

        stats.DisplayStats();

        Vector3 temphPos = hatchObject.transform.position;
        temphPos.y = 0.5f;
        gameObject.transform.position = temphPos;


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
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentState = States.Boxing;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            boxing.DodgeTeleport();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            stats.UpdateStats();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            anim.SetBool("Dead", true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            stats.IncreaseStat("agility", 10);
            stats.IncreaseStat("speed", 10);
        }




        switch (currentState)
        {
            case States.Hatching:
                hTimer += Time.deltaTime;
                if (hTimer > hatchTime)
                {
                    HatchMonster();
                }
                break;
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
            case States.Pooping:
                monster.WaitFor(5.75f);
                anim.SetFloat("Speed", 0);
                break;
            case States.Whine:
                monster.WaitFor(2);
                anim.SetFloat("Speed", 0);
                break;
            case States.Exit:
                monster.MoveTo(exitPoint);
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

    void HatchMonster()
    {
        hatchObject.SetActive(false);
        currentState = States.Follow;
    }

    public void SetAnimationState(animStates stateToSet)
    {
        animationState = stateToSet;
        switch (animationState)
        {
            case animStates.Idle:
                anim.SetFloat("Speed", 0);
                break;
            case animStates.Walking:
                anim.SetFloat("Speed", 0.75f + stats.mStats.speed * 0.0025f);
                break;
            case animStates.EatGround:
                anim.SetTrigger("Eat");
                break;
            case animStates.EatHand:
                
                break;
            case animStates.Sleep:
                anim.SetBool("Sleep", true);
                break;
            case animStates.Dead:
                anim.SetBool("Dead", true);
                break;
            case animStates.Lift:
                anim.SetTrigger("LiftObject");
                break;
            case animStates.Poop:
                anim.SetTrigger("DoTheDoo");
                break;
            case animStates.Sniff:
                anim.SetTrigger("Sniff");
                break;
            case animStates.Petting:
                anim.SetTrigger("Petting");
                break;
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
        currentState = States.Sleep;
        monster.GoSleep();
    }
    public void StartFetch(GameObject fObj)
    {
        currentState = States.Fetch;
        fetchObj = fObj;
    }


}
