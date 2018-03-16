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
    public Animator anim;

    [HideInInspector] public float totalSpeed;
    [HideInInspector] public bool hasPath = false;

    GameObject headOrigin;
    Vector3 playerGroundPosition;
    Quaternion playerRotation, targetRotation, playerGroundRotation; //Rotation towards player
    List<Vector3> PathPos = new List<Vector3>();
    int pathCounter;
    MonsterStats mStats;


    void Start()
    {
        totalSpeed = moveSpeed;
        mStats = gameObject.GetComponent<MonsterStats>();
        headOrigin = gameObject.transform.GetChild(0).gameObject;
        //mHead = gameObject.transform.GetChild(1).gameObject;
    }

    public void FollowPlayer(float distance)
    {
        playerGroundPosition = mainPlayer.transform.position;
        playerGroundPosition.y = 0.5f;

        playerRotation = Quaternion.LookRotation(mHead.transform.position - mainPlayer.transform.position); //Monster head and player
        playerGroundRotation = Quaternion.LookRotation(transform.position - playerGroundPosition); //Monster body and player(y=0.5)
        //mHead.transform.position = Vector3.MoveTowards(mHead.transform.position, headOrigin.transform.position, 1.5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerGroundPosition) < distance) //How close the monster will come to the player
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isIdle", true);
            anim.SetBool("isEating", false);
            hasPath = false;
            if (Vector3.Distance(transform.position, playerGroundPosition) < 1.5f) //Look forward
            {
                mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, transform.rotation, 5 * Time.deltaTime);
            }
            else if (Vector3.Distance(transform.position + -transform.forward * 4, playerGroundPosition) < 4) //Look at player
            {
                mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, playerRotation, 5 * Time.deltaTime);
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
            anim.SetBool("isWalk", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", true);
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
        anim.SetBool("isWalk", true);
        anim.SetBool("isIdle", false);
        anim.SetBool("isEating", false);
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
