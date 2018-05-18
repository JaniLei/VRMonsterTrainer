using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxing : MonoBehaviour {

    [HideInInspector] public GameObject player;
    [HideInInspector] public Monster monster;
    [HideInInspector] public LayerMask obstacleMask;
    [HideInInspector] public MonsterStats stats;
    [HideInInspector] public MonsterState state;
    public ParticleSystem particleSys;
    int monsterHit;
    float dir = 1;


    public void Dodge()
    {

        if (Random.Range(0, 150) < stats.mStats.agility)
        {
            dir = Mathf.Sign(Random.Range(-1, 1));
            if (stats.mStats.agility > 75)
            {
                DodgeTeleport();
            }
            else
            {
                state.SetAnimationState(MonsterState.animStates.Dodge);
            }
        }
        else
        {
            state.SetAnimationState(MonsterState.animStates.GetHit);
        }
        stats.IncreaseStat("agility", Random.Range(1, 3));
        state.SetAnimationState(MonsterState.animStates.Fight);
    }

    public void GetHit(bool gloves)
    {
        if (gloves)
        {
            Dodge();
        }
        else
        {
            state.SetAnimationState(MonsterState.animStates.GetHit);
            //Ragdoll ?
            monsterHit++;
            if (monsterHit > 2)
            {
                state.trustPlayer = false;
                state.SetEmotion(MonsterState.Emotions.Scared);
                state.SetState(MonsterState.States.Whine);
                state.stateInQueue = MonsterState.States.Search;
                monsterHit = 0;
            }
            else if (monsterHit > 1)
            {
                state.SetEmotion(MonsterState.Emotions.Furious);
            }
            else
            {
                state.SetEmotion(MonsterState.Emotions.Angry);
            }
        }
    }


    public void DodgeAttack()
    {
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

    public void DodgeTeleport()
    {

        if (!Physics.Raycast(transform.position,transform.right* dir, 1, obstacleMask))
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * dir * 1, Color.red, 6);
            //Dodge left + animation...
            transform.Translate(Vector3.right * dir * 0.5f);
            StartCoroutine(CreateParticle());
        }
        else if (!Physics.Raycast(transform.position,transform.right * dir, 1, obstacleMask))
        {
            Debug.DrawLine(transform.position, transform.position - transform.right * dir * 1, Color.red, 6);
            //Dodge right + animation...
            transform.Translate(-Vector3.right * dir * 0.5f);
            StartCoroutine(CreateParticle());
        }
        else
        {
            state.SetAnimationState(MonsterState.animStates.Dodge);
        }
    }

    IEnumerator CreateParticle()
    {
        particleSys.Play();

        particleSys.enableEmission = true;

        yield return new WaitForSeconds(0.325f);
        particleSys.enableEmission = false;
    }



}
