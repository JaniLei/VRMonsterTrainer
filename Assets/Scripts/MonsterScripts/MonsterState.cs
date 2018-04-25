﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour {

    public enum States { Hatching, Follow, Fetch, Sleep, Search, Pooping, Whine, Evolve, Exit, Push, Ragdoll, Dead, Petting, EatHand, EatGround} //states for the monster
    public enum animStates {Walking, EatHand, EatGround, Idle, Dead, Sleep, Petting, Poop, Lift, GetHit, Sniff, Hungry, Yawn, Evolve, AdultIdle, Push, AdultWalk }
    public enum Emotions {Neutral, Sad, Happy, Tired, Relaxed, Angry, Furious, Scared, Hungry }
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
    public Animator adultAnim;
    public AudioSource audioSource;
    public AudioClip yawn;
    public AudioClip hungry;
    public AudioClip poop;
    public AudioClip walk;
    public AudioClip eatHand;
    public AudioClip eatGround;
    public AudioClip hit;
    public AudioClip sleep;
    public AudioClip dyingWhimper;
    public AudioClip die;
    public AudioClip sniff;
    public AudioClip pickUp;
    public AudioClip enjoyPetting;
    public AudioClip push;
    public AudioClip evolve;

    void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        search = gameObject.GetComponent<SearchFood>();
        stats = gameObject.GetComponent<MonsterStats>();
        fetch = gameObject.GetComponent<Fetch>();
        boxing = gameObject.GetComponent<Boxing>();
        audioSource = gameObject.GetComponent<AudioSource>();

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
            case States.Evolve:
                monster.WaitFor(5);
                stateInQueue = States.Exit;
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
        stats.childMonster.SetActive(true);
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
        audioSource.loop = false;
        switch (animationState)
        {
            case animStates.Idle:
                anim.SetFloat("Speed", 0);
                adultAnim.SetFloat("Speed", 0);
                break;
            case animStates.Walking:
                anim.SetFloat("Speed", 0.75f + stats.mStats.speed * 0.0025f);
                adultAnim.SetFloat("Speed", 0.75f + stats.mStats.speed * 0.0025f);
                audioSource.Play();
                audioSource.loop = true;
                break;
            case animStates.EatGround:
                anim.SetTrigger("Eat");
                if (!audioSource.isPlaying) { audioSource.PlayOneShot(eatGround); }
                break;
            case animStates.EatHand:
                anim.SetTrigger("EatFromHand");
                if (!audioSource.isPlaying) { audioSource.PlayOneShot(eatHand); }
                break;
            case animStates.Sleep:
                anim.SetBool("Sleep", true);
                audioSource.PlayOneShot(sleep);
                break;
            case animStates.Dead:
                anim.SetFloat("Speed", 0);
                anim.SetBool("Dead", true);
                audioSource.PlayOneShot(die);
                break;
            case animStates.Lift:
                anim.SetTrigger("LiftObject");
                audioSource.PlayOneShot(pickUp);
                break;
            case animStates.Poop:
                audioSource.PlayOneShot(poop);
                anim.SetTrigger("DoTheDoo");
                break;
            case animStates.Sniff:
                anim.SetTrigger("Sniff");
                if (!audioSource.isPlaying) { audioSource.PlayOneShot(sniff); }
                break;
            case animStates.Petting:
                anim.SetTrigger("Petting");
                if (!audioSource.isPlaying) { audioSource.PlayOneShot(enjoyPetting); }
                break;
            case animStates.GetHit:
                anim.SetTrigger("Hit");
                if (!audioSource.isPlaying) { audioSource.PlayOneShot(hit); }
                break;
            case animStates.Hungry:
                anim.SetTrigger("");
                audioSource.PlayOneShot(hungry);
                break;
            case animStates.Yawn:
                audioSource.PlayOneShot(yawn);
                anim.SetTrigger("Yawn");
                break;
            case animStates.Evolve:
                adultAnim.SetTrigger("Evolve");
                audioSource.PlayOneShot(evolve);
                break;
            case animStates.Push:
                adultAnim.SetTrigger("Push");
                audioSource.PlayOneShot(push);
                break;


        }
    }

    public States stateInQueue = States.Follow;

    public void SetState(States _state)
    {
        if (monster.WaitStarted)
        {
            stateInQueue = _state;
            Debug.Log(stateInQueue);
            return;
        }
        monster.WaitStarted = false;
        try
        {
            fetchObj.GetComponent<Rigidbody>().useGravity = true;
        }
        catch { }

        if (currentState == States.Dead || currentState == States.Hatching || ragdolling)
        {
            return;
        }
        currentState = _state;
        stateInQueue = States.Follow;
    }


    public void SetEmotion(Emotions emotion)
    {
        switch (emotion)
        {
            case Emotions.Neutral:
                anim.SetInteger("Emotion", 0);
                break;
            case Emotions.Sad:
                anim.SetInteger("Emotion", 1);
                break;
            case Emotions.Happy:
                anim.SetInteger("Emotion", 2);
                break;
            case Emotions.Tired:
                anim.SetInteger("Emotion", 3);
                break;
            case Emotions.Relaxed:
                anim.SetInteger("Emotion", 4);
                break;
            case Emotions.Angry:
                anim.SetInteger("Emotion", 5);
                break;
            case Emotions.Furious:
                anim.SetInteger("Emotion", 6);
                break;
            case Emotions.Scared:
                anim.SetInteger("Emotion", 7);
                break;
            case Emotions.Hungry:
                anim.SetInteger("Emotion", 8);
                break;
        }
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
        SetEmotion(Emotions.Relaxed);
    }



}
