using System.Collections;
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
    public Text txtStats;

    public void UpdateStats()
    {
        if (hasEaten)
        {
            //Poop
            hasEaten = false;
        }
        else if (mStats.hunger>3) //Hungry
        {
            if (mStats.hunger > 5)
            {
                Debug.Log("Died of hunger");
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
            //Yawn
            if (mStats.fatigue > 9)
            {
                Debug.Log("Died of fatigue");
                state.SetState(MonsterState.States.Dead);
            }
            Debug.Log("Monster is sleepy");
        }
        mStats.hunger++;
        mStats.fatigue++;


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
                mStats.meat++;
                mStats.hunger = 0;
                IncreaseStat("health", 2);
                break;
            case "vegetable":
                mStats.vegetables++;
                mStats.hunger = 1;
                IncreaseStat("health", 1);
                break;
            case "item":
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
        txtStats.text = "Monster stats \nHealth:" + health + "\nSpeed:" + speed + "\nAgility" + agility;
    }

}
