﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    PathFinding pathFinding;

    public Vector3 position; //REMEMBER TO USE THIS INSTEAD OF transform.position
    public List<Node> neighbors =  new List<Node>();
    public float gCost, hCost, fCost;


    public Node(Vector3 _pos, PathFinding _path)
    {
        position = _pos;
        pathFinding = _path;
    }

    public void CalculateCosts(Vector3 startPoint, Vector3 goalPoint)
    {
        gCost = Vector3.Distance(position, startPoint);
        hCost = Vector3.Distance(position, goalPoint);
        fCost = gCost + hCost;
    }


}
