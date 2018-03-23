using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -- Josh --
// This script is attached to the prefab of the Raptors
// It will limit the area that the dino's are allowed to go in 
// while they follow the goal position.
public class Flock : MonoBehaviour
{

    public float speed = 10f;
    float rotationSpeed = 20.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 20.0f;

    bool turning = false;


    // Use this for initialization
    void Start()
    {
        // -- Josh --
        // Will make some of the dino's move faster then others
        // when created.
        speed = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        // -- Josh -- 
        // When the dinsours reach the edge of the area, they are forced into turning back into the center of the area
        // else they set to not turn as they are already in the center.
        if (Vector3.Distance(transform.position, new Vector3(((globalFlock.sizeXMax - globalFlock.sizeXMin) / 2) + globalFlock.sizeXMin,
            globalFlock.sizeY,
            ((globalFlock.sizeZMax - globalFlock.sizeZMin) / 2) + globalFlock.sizeZMin)) >= 100)
        {

            turning = true;
        }
        else
        {
            turning = false;
        }
        // -- Josh --
        // However, if they are said to turn, their direction is changed randomly towards the center of the square
        // they rotate to this location and their speed is randomly changed again to move faster or slower then before.
        // otherwise, they apply "the Rules". 
        if (turning)
        {
            Vector3 direction = new Vector3((((globalFlock.sizeXMax - globalFlock.sizeXMin) / 2) + globalFlock.sizeXMin), globalFlock.sizeY, ((globalFlock.sizeZMax - globalFlock.sizeZMin) / 2) + globalFlock.sizeZMin) - transform.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

            speed = Random.Range(5, 10);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }
        // -- Josh --
        // Finially, they are always told to move forward
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allDino;

        Vector3 goalPos = globalFlock.goalPos;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;
        float dist;
        int groupSize = 0;

        // -- Josh --
        // for every Dinosaur that isnt this dinosaur, they all move forward at the same time
        // it will also add each dino into the group once its its smaller then a range, otherwise
        // it will push them away if it gets too close.
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 10.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        // -- Josh --
        // if the groupsize is bigger then 0, the center and speed is normalized for the whole group and if they arent heading towards the goal, they are
        // all told to turn to face the goal to keep them all inline with it.
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);

            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre - vavoid) - transform.position;

            if (direction != goalPos)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
        }
    }
}
