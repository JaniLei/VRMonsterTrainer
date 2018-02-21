using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStats : MonoBehaviour {

    int hunger, fatigue;
    int health, speed, dexterity, vegetables, meat;
    public int maxHunger, maxFatique;
    public MonsterState state;

    public void UpdateStats()
    {
        hunger++;
        fatigue++;
        if (hunger > maxHunger)
        {
            state.SetState(MonsterState.States.Search);
        }
        else if (fatigue > maxFatique)
        {
            //state.SetState(MonsterState.States.Sleep);
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
                speed += amount;
                break;
                
        }
    }

}
