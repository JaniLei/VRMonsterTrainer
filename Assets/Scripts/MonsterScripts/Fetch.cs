using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : MonoBehaviour{

    [HideInInspector] public Monster monster;
    [HideInInspector] public MonsterState state;
    [HideInInspector] public MonsterStats stats;
    public GameObject monsterMouth;
    FixedJoint fj;
    bool hasObject;
    GameObject fetchObj;
    float timer = 0;
    int distance;
    bool AnimStarted;

    public void FetchObject(GameObject fObj)
    {
        fetchObj = fObj;
        if (!hasObject && Vector3.Distance(gameObject.transform.position, fetchObj.GetComponent<MeshCollider>().bounds.center) > 0.7f)
        {
            monster.MoveTo(fetchObj.GetComponent<MeshCollider>().bounds.center);
        }
        else if (!hasObject)
        {
            if (timer < 1f)
            {

                if (!AnimStarted)
                {
                    state.SetAnimationState(MonsterState.animStates.Lift);
                    state.SetEmotion(MonsterState.Emotions.Happy);
                    AnimStarted = true;
                }
                timer+=Time.deltaTime;
            }
            else
            {
                distance = (int)(Vector3.Distance(gameObject.transform.position, monster.playerGroundPosition));
                fetchObj.transform.position = monsterMouth.transform.position;
                hasObject = true;
                monster.hasPath = false;
                fetchObj.GetComponent<Rigidbody>().useGravity = false;
                timer = 0;
                AnimStarted = false;
            }

        }
        else if (Vector3.Distance(gameObject.transform.position, monster.playerGroundPosition) > 1.2f)
        {
            state.SetAnimationState(MonsterState.animStates.Walking);
            AnimStarted = false; 
            timer += Time.deltaTime;
            fetchObj.transform.position = monsterMouth.transform.position;
            fetchObj.transform.rotation = monsterMouth.transform.rotation;
            monster.FollowPlayer(0.5f);
        }
        else
        {
            state.SetEmotion(MonsterState.Emotions.Relaxed);
            StopFetch();
        }
    }

    public void StopFetch()
    {
        fetchObj.GetComponent<Rigidbody>().useGravity = true;
        hasObject = false;
        state.SetState(state.stateInQueue);
        stats.IncreaseStat("speed", distance * 2);
        timer = 0;
    }
}
