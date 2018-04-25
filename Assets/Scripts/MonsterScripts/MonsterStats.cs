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
    int foodCount;
    [HideInInspector] public Stats mStats;
    [HideInInspector] public int health = 10;
    public int maxHunger, maxFatigue;
    [HideInInspector] public bool hasEaten;
    [HideInInspector] public MonsterState state;
    [HideInInspector] public Monster monster;
    public GameObject poopObject;
    public GameObject childMonster;
    public GameObject adultMonster;
    public Text txtStats;
    int lastEaten; //1 = meat, 2 = vege, 3 = item

    public void UpdateStats()
    {
        if (hasEaten)
        {
            state.SetState(MonsterState.States.Pooping);
            state.SetAnimationState(MonsterState.animStates.Poop);
            state.SetEmotion(MonsterState.Emotions.Neutral);
            Invoke("SpawnPoop", 3.75f);
            /*for (int i = 0; i < 50; i++)
            {
                Invoke("SpawnPoop", i*0.1f);
            }*/

            hasEaten = false;
        }
        else if (mStats.hunger>3) //Hungry
        {
            state.SetAnimationState(MonsterState.animStates.Hungry);
            state.SetEmotion(MonsterState.Emotions.Hungry);
            if (mStats.hunger > maxHunger)
            {
                Debug.Log("Died of hunger");
                state.SetAnimationState(MonsterState.animStates.Dead);
                state.SetState(MonsterState.States.Dead);
            }
            else
            {
                state.SetState(MonsterState.States.Search);
                state.SetEmotion(MonsterState.Emotions.Sad);
                mStats.hunger++;
            }
        }
        else if (mStats.fatigue > maxFatigue)
        {
            state.SetAnimationState(MonsterState.animStates.Yawn);
            state.SetEmotion(MonsterState.Emotions.Tired);
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
        spawnPos += transform.forward * 0.3f;
        spawnPos -= transform.up * 0.3f;
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
                if (mStats.speed + amount < 100)
                {
                    mStats.speed += amount;
                    monster.totalSpeed = monster.moveSpeed * (1 + 0.01f * mStats.speed);
                }
                else
                {
                    mStats.speed = 100;
                }
                break;
            case "agility":
                if (mStats.agility + amount < 100)
                {
                    mStats.agility += amount;
                }
                else
                {
                    mStats.agility = 100;
                }
                break;
                
        }
        DisplayStats();
        CheckEvolve();
    }

    void CheckEvolve()
    {
        int totalCount = 1 + mStats.meat + mStats.vegetables + mStats.items;
        if (mStats.speed > 50 && mStats.agility > 30)
        {
            if (health < 1)
            {
                //Bad evolution
                Debug.Log("BAD EVOLUTION -> GAME OVER");
            }
            else if (mStats.meat/ totalCount > 0.75f)
            {
                //evolve red
                childMonster.SetActive(false);
                adultMonster.SetActive(true);
            }
            else if (mStats.vegetables / totalCount > 0.75f)
            {
                //evolve green
                childMonster.SetActive(false);
                adultMonster.SetActive(true);
            }
            else
            {
                //evolve brown
                childMonster.SetActive(false);
                adultMonster.SetActive(true);
            }
            state.SetState(MonsterState.States.Evolve);
        }
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
        foodCount++;
        Debug.Log("ate object type: " + type);

    }

    public void DisplayStats()
    {
        txtStats.text = "Monster stats \nHealth:" + health + "\nSpeed:" + mStats.speed + "\nAgility" + mStats.agility;
    }

}
