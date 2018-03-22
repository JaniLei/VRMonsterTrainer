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

    public void FetchObject(GameObject fObj)
    {
        fetchObj = fObj;
        if (!hasObject && Vector3.Distance(gameObject.transform.position, fetchObj.transform.position) > 0.7f)
        {
            monster.MoveTo(fetchObj.transform.position);
        }
        else if (!hasObject)
        {
            if (timer < 1f)
            {
                state.SetAnimationState(MonsterState.animStates.Lift);
                timer+=Time.deltaTime;
            }
            else
            {
                fetchObj.transform.position = monsterMouth.transform.position;
                hasObject = true;
                monster.hasPath = false;
                fj = monsterMouth.AddComponent<FixedJoint>();
                fj.connectedBody = fetchObj.GetComponent<Rigidbody>();
                fetchObj.GetComponent<Rigidbody>().useGravity = false; //stops object from gaining velocity while attached to monster
                timer = 0;
            }

        }
        else if (Vector3.Distance(gameObject.transform.position, monster.playerGroundPosition) > 1f)
        {
            fetchObj.transform.rotation = monsterMouth.transform.rotation;
            monster.FollowPlayer(0.5f);
        }
        else
        {
            StopFetch();
        }
    }

    public void StopFetch()
    {
        fetchObj.GetComponent<Rigidbody>().useGravity = true;
        Destroy(monsterMouth.GetComponent<FixedJoint>());
        hasObject = false;
        stats.IncreaseStat("speed", 5);
        state.SetState(MonsterState.States.Follow);
    }
}
