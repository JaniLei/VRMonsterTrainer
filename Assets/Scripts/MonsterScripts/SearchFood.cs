using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchFood : MonoBehaviour {

    public LayerMask foodMask;
    int rnd;
    bool foodOnSight = false;
    GameObject foodObj;
    float trustTimer;

    [HideInInspector] public Valve.VR.InteractionSystem.Player player;

    [HideInInspector] public Monster monster;
    [HideInInspector] public List<Vector3> nodes;
    [HideInInspector] public Vector3 movePoint;
    [HideInInspector] public Fetch fetch;
    [HideInInspector] public MonsterState state;


    public void Search(bool trustPlayer)
    {

        if (!trustPlayer)
        {
            trustTimer += Time.deltaTime;
            if (trustTimer > 1)
            {
                StartCoroutine(CheckPlayerPos());
                trustTimer = 0;
            }
        }

        /*for (int i = 0; i < player.hands.Length; i++)
        {
            if (player.hands[i].currentAttachedObject.GetComponent<Valve.VR.InteractionSystem.Edible>())
            {
                monster.FollowPlayer(1);
            }
        }*/

        if (foodOnSight)
        {
            foodOnSight = !monster.EatObject(foodObj);
            if (!foodOnSight && !trustPlayer)
            {
                foodOnSight = false;
                state.SetState(MonsterState.States.Search);
            }
        }
        else if (Vector3.Distance(transform.position, movePoint) < 0.3f)
        {
            rnd = Random.Range(0, nodes.Count);
            movePoint = (nodes[rnd]);

        }
        else
        {
            monster.MoveTo(movePoint);
            Collider[] collisions = Physics.OverlapSphere(transform.position, 3, foodMask);
            float tempDist = 100;
            foreach (Collider col in collisions)
            {
                if (col.gameObject.tag == "Edible" && !Physics.Linecast(transform.position, col.gameObject.transform.position, monster.ObstacleMask) && (transform.position.y - col.gameObject.transform.position.y) > 0)
                {
                    if (col.gameObject.GetComponent<Valve.VR.InteractionSystem.Edible>().notEdible)
                    {
                        state.StartFetch(col.gameObject);
                    }
                    else
                    {
                        tempDist = Vector3.Distance(col.transform.position, transform.position);
                        foodObj = col.gameObject;
                        foodOnSight = true;
                    }
                }
            }
        }

    }

    IEnumerator CheckPlayerPos()
    {
        Vector3 playerStartPos = monster.mainPlayer.transform.position;
        yield return new WaitForSeconds(10);
        if (Vector3.Distance(playerStartPos, monster.mainPlayer.transform.position) < 1)
        {
            foodOnSight = false;
            state.SetState(MonsterState.States.Follow);
            state.trustPlayer = true;
            Debug.Log("trust");
        }
    }
}
