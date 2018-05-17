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

    public Vector3 headNormalizer = new Vector3(-1,-1f,-2.5f);
    public float GroundLevel;
    bool headFollow;
    Quaternion headRotation;

    void Start()
    {
        totalSpeed = moveSpeed;
        mStats = gameObject.GetComponent<MonsterStats>();
        state = gameObject.GetComponent<MonsterState>();
        headOrigin = gameObject.transform.GetChild(0).gameObject;
    }
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
        playerGroundPosition.y = GroundLevel;

        //Monster head and player
        playerRotation = Quaternion.LookRotation(mHead.transform.position - mainPlayer.transform.position);

        //Monster body and player(y=0.5)
        playerGroundRotation = Quaternion.LookRotation(transform.position - playerGroundPosition);
        headRotation = Quaternion.Slerp(mHead.transform.rotation, playerRotation, 5 * Time.deltaTime);
        

        if (Vector3.Distance(transform.position, playerGroundPosition) < distance) //Monster stands still and looks towards the player
        {
            headFollow = false;
            state.SetAnimationState(MonsterState.animStates.Idle);
            hasPath = false;

            if (Vector3.Distance(transform.position, playerGroundPosition) < 1f) //Look forward
            {
                headRotation = Quaternion.Slerp(mHead.transform.rotation, transform.rotation, 5 * Time.deltaTime);
                headRotation *= Quaternion.Euler(headNormalizer);
                mHead.transform.rotation = headRotation;
                headFollow = true;
            }
            else if (Vector3.Distance(transform.position + -transform.forward * 4, playerGroundPosition) < 4) //Look at player
            {
                headRotation *= Quaternion.Euler(headNormalizer);
                mHead.transform.rotation = headRotation;
                headFollow = true;
            }
            else //Rotate Body towards player
            {
                headRotation *= Quaternion.Euler(headNormalizer);
                mHead.transform.rotation = headRotation;
                headFollow = true;
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
        if (Vector3.Distance(transform.position, EventManager.instance.targetObj.transform.position) > 1) //Move to bed
        {
            MoveTo(EventManager.instance.targetObj.transform.position);
        }
        else //Do sleep stuff
        {
            Vector3 tempVec = EventManager.instance.targetObj.transform.position;
            tempVec.y = GroundLevel + 0.15f;
            transform.position = tempVec;
            SteamVR_Fade.Start(Color.black, 2);
            Invoke("StopSleep", 4);
            state.SetAnimationState(MonsterState.animStates.Sleep);
            state.SetState(MonsterState.States.Sleep);
            mStats.mStats.fatigue = 0;
        }
    }

    void StopSleep()
    {
        state.SetState(MonsterState.States.Follow);
        state.SetAnimationState(MonsterState.animStates.Idle);
        SteamVR_Fade.Start(Color.clear, 2);
    }

    public void SniffObject(GameObject sniffObj)
    {
        Quaternion tempQuaternion = Quaternion.LookRotation(mHead.transform.position - sniffObj.transform.position);
        headRotation = Quaternion.Slerp(mHead.transform.rotation, tempQuaternion, 5 * Time.deltaTime);
        headRotation *= Quaternion.Euler(headNormalizer);
        mHead.transform.rotation = headRotation;
        headFollow = true;
        state.SetAnimationState(MonsterState.animStates.Sniff);
    }

    public bool WaitStarted;
    float waitTimer = 0;

    public void WaitFor(float t)
    {
        if (WaitStarted)
        {
            Wait(t);
        }
        else
        {
            waitTimer = t + waitTimer;
            Wait(waitTimer);
            WaitStarted = true;
        }
    }

    void Wait(float t)
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer < 0)
        {
            waitTimer = 0;
            WaitStarted = false;
            state.SetState(state.stateInQueue);
        }
    }


    float timer = 0;

    public bool EatObject(GameObject g) //returns true once the object has been eaten
    {
        Vector3 foodGroundPos = g.transform.position;
        foodGroundPos.y = GroundLevel;
        if (Vector3.Distance(mHead.transform.position, g.transform.position) < 0.35f) //eats from hand
        {
            state.SetState(MonsterState.States.EatHand);
            state.SetAnimationState(MonsterState.animStates.EatHand);
            mStats.EatFood(g.GetComponent<Valve.VR.InteractionSystem.Edible>().type.ToString());
            g.SetActive(false);
            state.SetEmotion(MonsterState.Emotions.Happy);
            timer += Time.deltaTime;
            if (timer > 2)
            {
                return true;
            }
        }
        else if (Vector3.Distance(mHead.transform.position, g.transform.position) < 0.8f) //eats from ground
        {
            state.foodObj = g;
            state.SetState(MonsterState.States.EatGround);
            state.SetAnimationState(MonsterState.animStates.EatGround);
            Quaternion foodRotation = Quaternion.LookRotation(transform.position - g.transform.position);
            Quaternion foodGroundRotation = Quaternion.LookRotation(transform.position - foodGroundPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, foodGroundRotation, 3 * Time.deltaTime);
            mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, foodRotation, 5 * Time.deltaTime);
            timer += Time.deltaTime;
            if (timer > 2)
            {
                mStats.EatFood(g.GetComponent<Valve.VR.InteractionSystem.Edible>().type.ToString());
                g.SetActive(false);
                state.SetEmotion(MonsterState.Emotions.Happy);
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
            state.SetAnimationState(MonsterState.animStates.Walking);

        }
        
        return false;


    }

    public void MoveTo(Vector3 target)
    {
        state.SetAnimationState(MonsterState.animStates.Walking);
        target.y = GroundLevel;
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
