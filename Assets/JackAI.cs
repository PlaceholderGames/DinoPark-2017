using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class JackAI : MonoBehaviour
{
    public float DinoSpeed = 10.0f;

    private AStarSearch aStarScript; 
    private PathFollower pathFollowerScrpit;

    void start()
    {
        aStarScript = GetComponent < AStarSearch>();
        pathFollowerScrpit = GetComponent<PathFollower>();
    }

    void update()
    {
        if (pathFollowerScrpit.path.nodes.Count < 1 || pathFollowerScrpit.path == null)
            pathFollowerScrpit.path = aStarScript.ASpath;

        move(pathFollowerScrpit.getDirectionVector());

   
    }
    void move(Vector3 directionVector)
    {
        directionVector *= DinoSpeed * Time.deltaTime;


        transform.Translate(directionVector, Space.World);
        transform.LookAt(transform.position + directionVector);
    }
}
*/