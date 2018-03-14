using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterStats : MonoBehaviour {

    [HideInInspector] public int hunger = 0, fatigue = 0;
    [HideInInspector]public int health = 10, speed = 0, agility = 0, vegetables, meat, items;
    public int maxHunger, maxFatique;
    [HideInInspector] public bool hasEaten;
    [HideInInspector] public MonsterState state;
    [HideInInspector] public Monster monster;

    public void UpdateStats()
    {
        if (hasEaten)
        {
            //Poop
            hasEaten = false;
        }
        else if (hunger>3) //Hungry
        {
            if (hunger > 5)
            {
                Debug.Log("Died of hunger");
                state.SetState(MonsterState.States.Dead);
            }
            else
            {
                state.SetState(MonsterState.States.Search);
                hunger++;
            }
        }
        else if (fatigue > 6)
        {
            //Yawn
            if (fatigue > 9)
            {
                Debug.Log("Died of fatigue");
                state.SetState(MonsterState.States.Dead);
            }
            Debug.Log("Monster is sleepy");
        }
        hunger++;
        fatigue++;


    }

    public void IncreaseStat(string stat, int amount) //optional
    {
        switch (stat)
        {
            case "health":
                health += amount;
                break;
            case "speed":
                if (speed < 100)
                {
                    speed += amount;
                    monster.totalSpeed = monster.moveSpeed * (1 + 0.01f * speed);
                }
                break;
            case "agility":
                agility += amount;
                break;
                
        }
        
    }

    public void EatFood(string type)
    {
        switch (type)
        {
            case "meat":
                meat++;
                hunger = 0;
                IncreaseStat("health", 2);
                break;
            case "vegetable":
                vegetables++;
                hunger = 1;
                IncreaseStat("health", 1);
                break;
            case "item":
                items++;
                hunger = 2;
                IncreaseStat("health", -1);
                break;
        }
        hasEaten = true;
        state.SetState(MonsterState.States.Follow);
        Debug.Log("ate object type: " + type);

    }

}
