using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This scripts includes following, moving to target, eating/picking up and dodge

public class Monster : MonoBehaviour {

    GameObject headOrigin;
    public GameObject mainPlayer, mHead;
    public LayerMask ObstacleMask;
    public PathFinding pathFinding;
    public bool hasPath = false;
    public float moveSpeed;

    Vector3 playerGroundPosition;
    Quaternion playerRotation, targetRotation, playerGroundRotation; //Rotation towards player
    List<Vector3> PathPos = new List<Vector3>();
    int pathCounter;
    public GameObject bedObj;

    public Animator anim;

    void Start()
    {
        headOrigin = gameObject.transform.GetChild(0).gameObject;
        mHead = gameObject.transform.GetChild(1).gameObject;

    }


    public void FollowPlayer(float distance)
    {
        playerGroundPosition = mainPlayer.transform.position;
        playerGroundPosition.y = 0.5f;

        playerRotation = Quaternion.LookRotation(mHead.transform.position - mainPlayer.transform.position); //Monster head and player
        playerGroundRotation = Quaternion.LookRotation(transform.position - playerGroundPosition); //Monster body and player(y=0.5)
        mHead.transform.position = Vector3.MoveTowards(mHead.transform.position, headOrigin.transform.position, 1.5f * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerGroundPosition) < distance) //How close the monster will come to the player
        {
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

    public bool EatObject(GameObject g) //returns true once the object has been eaten
    {
        Vector3 foodGroundPos = g.transform.position;
        foodGroundPos.y = 0.5f;
        if (Vector3.Distance(mHead.transform.position, g.transform.position) < 0.35f)
        {
            //Do eating stuff...
            //Check hunger...
            g.SetActive(false);
            return true;
        }
        else if (Vector3.Distance(mHead.transform.position, g.transform.position) < 0.8f)
        {
            Quaternion foodRotation = Quaternion.LookRotation(transform.position - g.transform.position);
            Quaternion foodGroundRotation = Quaternion.LookRotation(transform.position - foodGroundPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, foodGroundRotation, 3 * Time.deltaTime);
            mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, foodRotation, 5 * Time.deltaTime);
            //eat from ground animation
        }
        else
        {
            Quaternion foodRotation = Quaternion.LookRotation(transform.position - g.transform.position);
            Quaternion foodGroundRotation = Quaternion.LookRotation(transform.position - foodGroundPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, foodGroundRotation, 3 * Time.deltaTime);
            mHead.transform.rotation = Quaternion.Slerp(mHead.transform.rotation, foodRotation, 5 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, foodGroundPos, moveSpeed * Time.deltaTime);


        }
        
        return false;


    }

    public void DodgeAttack()
    {
        //Debug.DrawLine(transform.position - transform.right + transform.forward, transform.position - transform.right - transform.forward, Color.red, 6);
        //Debug.DrawLine(transform.position + transform.right + transform.forward, transform.position + transform.right - transform.forward, Color.red, 6);
        if (!Physics.Linecast(transform.position - transform.right + transform.forward, transform.position - transform.right - transform.forward, ObstacleMask))
        {
            //Dodge left + animation...
            transform.Translate(-Vector3.right * 5f * Time.deltaTime);
        }
        else if (!Physics.Linecast(transform.position + transform.right + transform.forward, transform.position + transform.right - transform.forward, ObstacleMask))
        {
            //Dodge right + animation...
            transform.Translate(Vector3.right * 5f * Time.deltaTime);
        }
    }

    public void MoveTo(Vector3 target)
    {
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
                transform.position = Vector3.MoveTowards(transform.position, PathPos[pathCounter], moveSpeed * Time.deltaTime);
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
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
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
