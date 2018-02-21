using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : MonoBehaviour{

    public Monster monster;
    public MonsterState state;
    bool hasObject;
    public void FetchObject(GameObject fetchObj)
    {
        if (!hasObject && Vector3.Distance(gameObject.transform.position, fetchObj.transform.position) > 0.4f)
        {
            monster.MoveTo(fetchObj.transform.position);
        }
        else if (!hasObject)
        {
            fetchObj.transform.position = monster.mHead.transform.position;
            hasObject = true;
            monster.hasPath = false;
        }
        else if (Vector3.Distance(gameObject.transform.position, monster.mainPlayer.transform.position) > 1.2f)
        {
            fetchObj.transform.position = monster.mHead.transform.position;
            monster.FollowPlayer(1);
        }
        else
        {
            hasObject = false;
            state.SetState(MonsterState.States.Follow);
        }
    }
}
