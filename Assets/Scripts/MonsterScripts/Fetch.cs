using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : MonoBehaviour{

    [HideInInspector] public Monster monster;
    [HideInInspector] public MonsterState state;
    [HideInInspector] public MonsterStats stats;
    bool hasObject;

    public void FetchObject(GameObject fetchObj)
    {
        if (!hasObject && Vector3.Distance(gameObject.transform.position, fetchObj.transform.position) > 0.5f)
        {
            monster.MoveTo(fetchObj.transform.position);
        }
        else if (!hasObject)
        {
            fetchObj.transform.position = monster.mHead.transform.position + Vector3.up;
            hasObject = true;
            monster.hasPath = false;
        }
        else if (Vector3.Distance(gameObject.transform.position, monster.mainPlayer.transform.position) > 1.2f)
        {
            fetchObj.GetComponent<Rigidbody>().isKinematic = true;
            fetchObj.transform.position = monster.mHead.transform.position;
            monster.FollowPlayer(1);
        }
        else
        {
            fetchObj.GetComponent<Rigidbody>().isKinematic = false;
            hasObject = false;
            stats.IncreaseStat("speed", 5);
            state.SetState(MonsterState.States.Follow);
        }
    }
}
