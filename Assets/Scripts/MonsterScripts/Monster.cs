using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public GameObject mHead, mainPlayer, headOrigin;

    public LayerMask ObstacleMask;
    public PathFinding pathFinding;

    public bool hasPath = false;

    Vector3 playerGroundPosition;
    Quaternion playerRotation, targetRotation, playerGroundRotation; //Rotation towards player

    List<Vector3> PathPos = new List<Vector3>();

    public GameObject bedObj;

    int pathCounter;

	

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
        if (Vector3.Distance(transform.position, bedObj.transform.position) > 0.5f)
        {
            MoveTo(bedObj.transform.position);
        }
        else
        {
            transform.position = bedObj.transform.position;
        }
    }

    public void CatchObject(GameObject g)
    {
        mHead.transform.position = Vector3.MoveTowards(mHead.transform.position, g.transform.position, 5);
        //Idea: set a joint to the object
    }

    public void EatObject(GameObject g)
    {
        mHead.transform.position = Vector3.MoveTowards(mHead.transform.position, g.transform.position, 0.5f);
        g.SetActive(false);
        mHead.transform.position = headOrigin.transform.position;
    }

    public void MoveTo(Vector3 target)
    {
        target.y = 0.5f;
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - target);
        //RaycastHit hit;
        //Physics.SphereCast(mHead.transform.position, 0.1f,target - mHead.transform.position, out hit, Vector3.Distance(mHead.transform.position, target), ObstacleMask) && !hasPath
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
                transform.position = Vector3.MoveTowards(transform.position, PathPos[pathCounter], 5 * Time.deltaTime);
                if (Vector3.Distance(transform.position, PathPos[pathCounter]) < 0.5f)
                {
                    if (Vector3.Distance(transform.position, PathPos[pathCounter]) < 0.1f)
                    {
                        pathCounter++;
                        if (Vector3.Distance(target, PathPos[PathPos.Count-1]) > 3)
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
            transform.position = Vector3.MoveTowards(transform.position, target, 5 * Time.deltaTime);
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
