using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scripts includes following, moving to target, eating/picking up and dodge

public class Monster : MonoBehaviour {

    public GameObject mainPlayer, mHead;
    public LayerMask ObstacleMask;
    public PathFinding pathFinding;
    public float moveSpeed;
    public GameObject bedObj;

    [HideInInspector] public float totalSpeed;
    [HideInInspector] public bool hasPath = false;

    GameObject headOrigin;
    [HideInInspector] public Vector3 playerGroundPosition;
    Quaternion playerRotation, targetRotation, playerGroundRotation; //Rotation towards player
    List<Vector3> PathPos = new List<Vector3>();
    int pathCounter;
    MonsterStats mStats;
    MonsterState state;

    public Vector3 headNormalizer = new Vector3(-1,-0.75f,-2.5f);
    bool headFollow;

    void Start()
    {
        totalSpeed = moveSpeed;
        mStats = gameObject.GetComponent<MonsterStats>();
        state = gameObject.GetComponent<MonsterState>();
        headOrigin = gameObject.transform.GetChild(0).gameObject;
        //mHead = gameObject.transform.GetChild(1).gameObject;
    }
    Quaternion headRotation;
    void LateUpdate()
    {
        if (headFollow)
        {
            mHead.transform.rotation = headRotation;
            headFollow = false;
        }
    }


    public void FollowPlayer(float distance)
    {
        playerGroundPosition = mainPlayer.transform.position;
        playerGroundPosition.y = 0.5f;

        playerRotation = Quaternion.LookRotation(mHead.transform.position - mainPlayer.transform.position); //Monster head and player
        playerRotation.z = 0;
        playerGroundRotation = Quaternion.LookRotation(transform.position - playerGroundPosition); //Monster body and player(y=0.5)
        //mHead.transform.position = Vector3.MoveTowards(mHead.transform.position, headOrigin.transform.position, 1.5f * Time.deltaTime);

        headRotation = Quaternion.Slerp(mHead.transform.rotation, playerRotation, 5 * Time.deltaTime);
        headRotation *= Quaternion.Euler(headNormalizer); 
        Debug.Log(headRotation.x + " " + headRotation.y + " " + headRotation.z + " ");

        if (Vector3.Distance(transform.position, playerGroundPosition) < distance) //How close the monster will come to the player
        {
            headFollow = false;
            state.SetAnimationState(MonsterState.animStates.Idle);
            hasPath = false;
            if (Vector3.Distance(transform.position, playerGroundPosition) < 1.5f) //Look forward
            {
                mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, transform.rotation, 5 * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position + -transform.forward * 4, playerGroundPosition) < 4) //Look at player
            {
                headFollow = true;
                mHead.transform.rotation = headRotation;
            }
            else //Rotate Body towards player
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, playerGroundRotation, 5 * Time.deltaTime);
            }
        }
        else //Move towards player
        {
            MoveTo(playerGroundPosition);
        }
    }
    
    public void GoSleep()
    {
        if (Vector3.Distance(transform.position, EventManager.instance.targetObj.transform.position) > 0.5f)
        {
            MoveTo(bedObj.transform.position);
        }
        else
        {
            transform.position = bedObj.transform.position;
            //Do sleep things...
        }
    }

    public void SniffObject()
    {
        state.SetAnimationState(MonsterState.animStates.Sniff);
    }

    bool WaitStarted;

    public void WaitFor(float t)
    {
        if (WaitStarted)
        {
            Wait(t);
        }
        else
        {
            timer = 0;
            Wait(t);
            WaitStarted = true;
        }
    }

    void Wait(float t)
    {
        timer += Time.deltaTime;
        if (timer > t)
        {
            state.SetState(MonsterState.States.Follow);
            WaitStarted = false;
        }
    }

    public void CatchObject(GameObject g)
    {
        mHead.transform.position = Vector3.MoveTowards(mHead.transform.position, g.transform.position, 5);
        //Idea: set a joint to the object
    }

    float timer = 0;

    public bool EatObject(GameObject g) //returns true once the object has been eaten
    {
       
        Vector3 foodGroundPos = g.transform.position;
        foodGroundPos.y = 0.5f;
        if (Vector3.Distance(mHead.transform.position, g.transform.position) < 0.35f) //eats from hand
        {
            //Eat from hand animation
            mStats.EatFood(g.GetComponent<Valve.VR.InteractionSystem.Edible>().type.ToString()); //Type of object eaten
            g.SetActive(false);
            return true;
        }
        else if (Vector3.Distance(mHead.transform.position, g.transform.position) < 0.8f) //eats from ground
        {
            state.SetAnimationState(MonsterState.animStates.EatGround);
            Quaternion foodRotation = Quaternion.LookRotation(transform.position - g.transform.position);
            Quaternion foodGroundRotation = Quaternion.LookRotation(transform.position - foodGroundPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, foodGroundRotation, 3 * Time.deltaTime);
            mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, foodRotation, 5 * Time.deltaTime);
            timer += Time.deltaTime;
            if (timer > 2)
            {
                mStats.EatFood(g.GetComponent<Valve.VR.InteractionSystem.Edible>().type.ToString()); //Type of object eaten
                g.SetActive(false);
                return true;
            }
        }
        else
        {
            timer = 0;
            Quaternion foodRotation = Quaternion.LookRotation(transform.position - g.transform.position);
            Quaternion foodGroundRotation = Quaternion.LookRotation(transform.position - foodGroundPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, foodGroundRotation, 3 * Time.deltaTime);
            mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, foodRotation, 5 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, foodGroundPos, totalSpeed * Time.deltaTime);


        }
        
        return false;


    }

    public void MoveTo(Vector3 target)
    {
        state.SetAnimationState(MonsterState.animStates.Walking);
        target.y = 0.5f;
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - target);
        if (Physics.Linecast(transform.position, target, ObstacleMask) && !hasPath)
        {
           PathPos = pathFinding.FindPath(transform.position, target); //PATHFINDING
           pathCounter = 0;
           hasPath = true;

        }
        else if (hasPath)
        {
            if (pathCounter < PathPos.Count) 
            {
                targetRotation = Quaternion.LookRotation(transform.position - PathPos[pathCounter]);
                transform.position = Vector3.MoveTowards(transform.position, PathPos[pathCounter], totalSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, PathPos[pathCounter]) < 0.5f)
                {
                    if (Vector3.Distance(transform.position, PathPos[pathCounter]) < 0.1f)
                    {
                        pathCounter++;
                        if (Vector3.Distance(target, PathPos[PathPos.Count-1]) > 5)
                        {
                            hasPath = false;
                        }
                    }

                }
                else
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
                    //mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, targetRotation, 5 * Time.deltaTime);
                }

            }
            else
            {
                hasPath = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
            mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, targetRotation, 5 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target, totalSpeed * Time.deltaTime);
        }
    }

    void FixPosition(Vector3 target) //Obsolete
    {
        if (!Physics.Raycast(transform.position, -transform.forward * 0.2f, ObstacleMask))
        {
            transform.Translate(-Vector3.forward * Time.deltaTime);
        }
        else if (!Physics.Raycast(transform.position, -transform.right * 0.2f, ObstacleMask))
        {
            transform.Translate(-Vector3.right * Time.deltaTime);
        }
        else
        {
            transform.position = target;
        }
    }
}
