﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour {

    public enum States { Hatching, Follow, Fetch, Sleep, Search, Pooping, Whine, Exit, Ragdoll, Dead, Petting, EatHand, EatGround} //states for the monster
    public enum animStates {Walking, EatHand, EatGround, Idle, Dead, Sleep, Petting, Poop, Lift, GetHit, Sniff, Hungry, Yawn }
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
    public bool trustPlayer;

    bool ragdolling = false;

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
        boxing.state = this;

        stats.DisplayStats();

        Vector3 temphPos = hatchObject.transform.position;
        temphPos.y = monster.GroundLevel;
        gameObject.transform.position = temphPos;


        EventManager.instance.Fetching += OnFetching;
        EventManager.instance.Pointing += OnPointing;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentState = States.Sleep;
            EventManager.instance.targetObj = monster.bedObj;
            EventManager.instance.OnPointing();
            OnPointing();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentState = States.Search;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            boxing.GetHit(false);
            stats.IncreaseStat("agility", 2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            stats.UpdateStats();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentState = States.Dead;
            anim.SetBool("Dead", true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            stats.IncreaseStat("agility", 10);
            stats.IncreaseStat("speed", 10);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ragdolling = !ragdolling;
            if (ragdolling)
            {
                currentState = States.Ragdoll;
            }
            else
            {
                Vector3 tempVec = monster.mHead.transform.position;
                tempVec.y = monster.GroundLevel;
                transform.position = tempVec;
                
                currentState = States.Follow;
            }
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
                search.Search(trustPlayer);
                break;
            case States.Pooping:
                monster.WaitFor(5.75f);
                break;
            case States.Whine:
                monster.WaitFor(2);
                break;
            case States.EatHand:
                monster.WaitFor(5);
                break;
            case States.EatGround:
                monster.WaitFor(2.5f);
                break;
            case States.Petting:
                monster.WaitFor(2.5f);
                break;
            case States.Ragdoll:

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
        if (stateToSet == animationState)
        {
            return;
        }
        anim.SetFloat("Speed", 0);
        anim.SetBool("Sleep", false);
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
                anim.SetTrigger("EatFromHand");
                break;
            case animStates.Sleep:
                anim.SetBool("Sleep", true);
                break;
            case animStates.Dead:
                anim.SetFloat("Speed", 0);
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
            case animStates.GetHit:
                anim.SetTrigger("Hit");
                break;
            case animStates.Hungry:
                anim.SetTrigger("");
                break;
            case animStates.Yawn:
                anim.SetTrigger("Yawn");
                break;
            
        }
    }

    public void SetState(States _state)
    {
        monster.WaitStarted = false;
        if (currentState == States.Dead || currentState == States.Hatching || ragdolling)
        {
            return;
        }
        currentState = _state;
    }

    public void OnFetching()
    {
        StartFetch(EventManager.instance.targetObj);
    }
    public void OnPointing()
    {
        SetState(States.Sleep);
        monster.GoSleep();

    }
    public void StartFetch(GameObject fObj)
    {
        SetState(States.Fetch);
        fetchObj = fObj;
    }

    public void Petting()
    {
        SetState(States.Petting);
        SetAnimationState(animStates.Petting);
    }



}
