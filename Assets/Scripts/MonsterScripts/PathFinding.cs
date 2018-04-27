using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {


    //Attach this script along with the Node script to an empty gameobject that has the node gameobjects as children
    //Algorithm is best first (for now)

    public LayerMask obstacleLayer;
    public List<Node> allNodes = new List<Node>();

    public SearchFood searchFood;
    
    List<Node> closedNodes = new List<Node>();
    List<Node> pathNodes = new List<Node>();
    List<Node> openNodes = new List<Node>();
    List<Vector3> pathVectors = new List<Vector3>();
    Node defNode, currentNode, bestNode;


    void Start()
    {
        Monster monster = gameObject.GetComponent<Monster>();
        defNode = new Node(new Vector3(), this);
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            allNodes.Add(new Node(gameObject.transform.GetChild(i).position, this));
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        foreach (Node n in allNodes)
        {
            n.position.y = monster.GroundLevel;
            foreach (Node nn in allNodes)
            {
                if (n != nn && !Physics.Linecast(n.position, nn.position, obstacleLayer))
                {
                    //Debug.DrawLine(n.position, nn.position,Color.red, 100);
                    n.neighbors.Add(nn);
                }

            }
            searchFood.nodes.Add(n.position); //SearchFood
            
        }
        searchFood.movePoint=allNodes[Random.Range(0,allNodes.Count)].position;
    }



    public List<Vector3> FindPath(Vector3 startPoint, Vector3 goal)
    {
        closedNodes.Clear();
        pathNodes.Clear();
        //openNodes.Clear(); //available nodes to explore
        pathVectors.Clear();

        defNode.position = startPoint; //Gets returned if no path can be found

        currentNode = GetStartNode(startPoint, goal);
        if (!Physics.Linecast(currentNode.position, goal, obstacleLayer))
        {
            pathVectors.Add(currentNode.position);
            return pathVectors;
        }

        if (Vector3.Distance(startPoint, currentNode.position) > 0.1f) //Ignore start node if it is close to the starting point
        {
            pathNodes.Add(currentNode);
        }
        float temphCost = 0;

        

        while (closedNodes.Count < allNodes.Count)
        {
            closedNodes.Add(currentNode);
            //openNodes.Remove(currentNode);
            temphCost = 100;
            foreach (Node n in currentNode.neighbors)
            {
                //openNodes.Add(n);
                if (n.hCost < temphCost && !closedNodes.Contains(n))
                {
                    temphCost = n.hCost;
                    bestNode = n;
                    /*
                    if (!Physics.Linecast(n.position, goal, obstacleLayer))
                    {
                        pathNodes.Add(bestNode);
                        foreach (Node nn in pathNodes)
                        {
                            pathVectors.Add(nn.position);
                        }
                        return pathVectors;
                    }*/
                }
            }
            if (!Physics.Linecast(bestNode.position, goal, obstacleLayer)) //Remove this if not working
            {
                pathNodes.Add(bestNode);
                foreach (Node nn in pathNodes)
                {
                    pathVectors.Add(nn.position);
                }
                return pathVectors;
            }

            /*foreach (Node n in openNodes) //check if there is a better path
            {
                if (n.neighbors.Contains(currentNode) && n.hCost < temphCost && !closedNodes.Contains(n))
                {
                    temphCost = n.hCost;
                    bestNode = n;

                    if (!Physics.Linecast(n.position, goal, obstacleLayer))
                    {
                        pathNodes.Add(bestNode);
                        foreach (Node nn in pathNodes)
                        {
                            pathVectors.Add(nn.position);
                        }
                        return pathVectors;
                    }
                }
            }*/

            pathNodes.Add(bestNode);
            currentNode = bestNode;

        }
        pathVectors.Add(pathNodes[0].position);
        pathVectors.Add(pathNodes[1].position);
        return pathVectors;
    }



    Node GetStartNode(Vector3 startPoint, Vector3 goal) //Find the closest node to the monsters current position
    {
        float tempfCost = 100;
        Node tempNode = defNode;
        foreach (Node n in allNodes)
        {
            n.CalculateCosts(startPoint, goal);
            if (!Physics.Linecast(startPoint, n.position, obstacleLayer) && n.fCost < tempfCost)
            {
                tempfCost = n.fCost;
                tempNode = n;
            }
        }
        return tempNode;
    }

}
