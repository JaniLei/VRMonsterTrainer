﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MonsterStats : MonoBehaviour {

    public struct Stats
    {
        public int hunger, fatigue;
        public int speed, agility, vegetables, meat, items;
    }
    public Stats mStats;
    [HideInInspector]public int health = 10;
    public int maxHunger, maxFatique;
    [HideInInspector] public bool hasEaten;
    [HideInInspector] public MonsterState state;
    [HideInInspector] public Monster monster;
    public GameObject poopObject;
    public Text txtStats;
    int lastEaten; //1 = meat, 2 = vege, 3 = item

    public void UpdateStats()
    {
        if (hasEaten)
        {
            state.SetState(MonsterState.States.Pooping);
            state.SetAnimationState(MonsterState.animStates.Poop);
            Invoke("SpawnPoop", 3.75f);

            hasEaten = false;
        }
        else if (mStats.hunger>3) //Hungry
        {
            //state.SetAnimationState(MonsterState.animStates.Hungry);
            if (mStats.hunger > 5)
            {
                Debug.Log("Died of hunger");
                state.SetAnimationState(MonsterState.animStates.Dead);
                state.SetState(MonsterState.States.Dead);
            }
            else
            {
                state.SetState(MonsterState.States.Search);
                mStats.hunger++;
            }
        }
        else if (mStats.fatigue > 6)
        {
            //state.SetAnimationState(MonsterState.animStates.Yawn);
            Debug.Log("Monster is sleepy");
            if (mStats.fatigue > 9)
            {
                Debug.Log("Died of fatigue");
                state.SetAnimationState(MonsterState.animStates.Dead);
                state.SetState(MonsterState.States.Dead);
            }
        }
        mStats.hunger++;
        mStats.fatigue++;


    }

    void SpawnPoop()
    {
        Vector3 spawnPos = transform.position;
        spawnPos -= transform.forward;
        switch (lastEaten)
        {
            case 1:
                Instantiate(poopObject, spawnPos, transform.rotation).GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 2:
                Instantiate(poopObject, spawnPos, transform.rotation).GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case 3:
                Instantiate(poopObject, spawnPos, transform.rotation).GetComponent<MeshRenderer>().material.color = Color.grey;
                break;
        }
    }


    public void IncreaseStat(string stat, int amount) //optional
    {
        switch (stat)
        {
            case "health":
                health += amount;
                break;
            case "speed":
                if (mStats.speed < 100)
                {
                    mStats.speed += amount;
                    monster.totalSpeed = monster.moveSpeed * (1 + 0.01f * mStats.speed);
                }
                break;
            case "agility":
                mStats.agility += amount;
                break;
                
        }
        DisplayStats();
        
    }

    public void EatFood(string type)
    {
        switch (type)
        {
            case "meat":
                lastEaten = 1;
                mStats.meat++;
                mStats.hunger = 0;
                IncreaseStat("health", 2);
                break;
            case "vegetable":
                lastEaten = 2;
                mStats.vegetables++;
                mStats.hunger = 1;
                IncreaseStat("health", 1);
                break;
            case "item":
                lastEaten = 3;
                mStats.items++;
                mStats.hunger = 2;
                IncreaseStat("health", -1);
                break;
        }
        hasEaten = true;
        state.SetState(MonsterState.States.Follow);
        Debug.Log("ate object type: " + type);

    }

    public void DisplayStats()
    {
        txtStats.text = "Monster stats \nHealth:" + health + "\nSpeed:" + mStats.speed + "\nAgility" + mStats.agility;
    }

}
