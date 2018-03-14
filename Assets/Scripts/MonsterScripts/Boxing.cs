using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxing : MonoBehaviour {

    [HideInInspector] public GameObject player;
    [HideInInspector] public Monster monster;
    [HideInInspector] public LayerMask obstacleMask;
    [HideInInspector] public MonsterStats stats;

    bool isDodging;
    float dodgeTimer;
    float dir;

    public void DoBoxing()
    {
        monster.FollowPlayer(1.5f);
        if (Input.GetKeyDown(KeyCode.Alpha5) && !isDodging)
        {
            if (Random.Range(0,100) < stats.agility)
            {
                dir = Mathf.Sign(Random.Range(-1, 1));
                isDodging = true;
                dodgeTimer = 0;
            }
            stats.IncreaseStat("agility", 1);
        }
        else if (isDodging)
        {
            DodgeAttack();
            dodgeTimer += Time.deltaTime;
            if(dodgeTimer > 0.075f)
            {
                isDodging = false;
            }
        }
    }


    public void DodgeAttack()
    {
        //Debug.DrawLine(transform.position - transform.right + transform.forward, transform.position - transform.right - transform.forward, Color.red, 6);
        //Debug.DrawLine(transform.position + transform.right + transform.forward, transform.position + transform.right - transform.forward, Color.red, 6);
        if (!Physics.Linecast(transform.position - transform.right + transform.forward * dir, transform.position - transform.right - transform.forward * dir, obstacleMask))
        {
            Debug.DrawLine(transform.position - transform.right + transform.forward * dir, transform.position - transform.right - transform.forward * dir, Color.red, 6);
            //Dodge left + animation...
            transform.Translate(Vector3.right * dir * 8 * Time.deltaTime);
        }
        else if (!Physics.Linecast(transform.position + transform.right + transform.forward * dir, transform.position + transform.right - transform.forward * dir, obstacleMask))
        {
            Debug.DrawLine(transform.position + transform.right + transform.forward * dir, transform.position + transform.right - transform.forward * dir, Color.red, 6);
            //Dodge right + animation...
            transform.Translate(-Vector3.right * dir * 8 * Time.deltaTime);
        }
    }

}
