using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFlee : ASAgent
{

    AStarSearch aStar;
    ASPathFollower pathFoll;
    public void Start()
    {
        aStar = gameObject.GetComponent<AStarSearch>();
        pathFoll = gameObject.GetComponent<ASPathFollower>();
        aStar.enabled = true;
        pathFoll.enabled = true;
    }
    new void Update()
    {
        if (pathFoll.path.nodes.Count < 1 || pathFoll.path == null)
            pathFoll.path = aStar.path;

        directionVector = pathFoll.getDirectionVector();

        base.Update();
    }
    public void TurnOffAs()
    {
        aStar.enabled = false;
        pathFoll.enabled = false;
    }
}
