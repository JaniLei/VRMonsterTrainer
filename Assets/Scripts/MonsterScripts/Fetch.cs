using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : MonoBehaviour{

    [HideInInspector] public Monster monster;
    [HideInInspector] public MonsterState state;
    [HideInInspector] public MonsterStats stats;
    FixedJoint fj;
    bool hasObject;
    GameObject fetchObj;

    public void FetchObject(GameObject fObj)
    {
        fetchObj = fObj;
        if (!hasObject && Vector3.Distance(gameObject.transform.position, fetchObj.transform.position) > 0.5f)
        {
            monster.MoveTo(fetchObj.transform.position);
        }
        else if (!hasObject)
        {
            fetchObj.transform.position = monster.mHead.transform.position;
            hasObject = true;
            monster.hasPath = false;
            fj = monster.mHead.AddComponent<FixedJoint>();
            fj.connectedBody = fetchObj.GetComponent<Rigidbody>();
            fetchObj.GetComponent<Rigidbody>().useGravity = false; //stops object from gaining velocity while attached to monster

        }
        else if (Vector3.Distance(gameObject.transform.position, monster.playerGroundPosition) > 1f)
        {
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
        Destroy(monster.mHead.GetComponent<FixedJoint>());
        hasObject = false;
        stats.IncreaseStat("speed", 5);
        state.SetState(MonsterState.States.Follow);
    }
}
