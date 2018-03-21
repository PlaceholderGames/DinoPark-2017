using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    public float speed = 10f;
    float rotationSpeed = 20.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 20.0f;

    bool turning = false;


	// Use this for initialization
	void Start () {
        speed = Random.Range(5, 10);
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, new Vector3(((globalFlock.sizeXMax - globalFlock.sizeXMin)/2) + globalFlock.sizeXMin, 
            globalFlock.sizeY, 
            ((globalFlock.sizeZMax - globalFlock.sizeZMin)/2) + globalFlock.sizeZMin)) >= 100)
        {

            turning = true;
        }
        //else if (Vector3.Distance(transform.position, new Vector3(globalFlock.sizeXMax - globalFlock.sizeXMin, globalFlock.sizeY, globalFlock.sizeZMax - globalFlock.sizeZMin)) >= globalFlock.sizeZMin)
        //{
          //  turning = true;
        //}
        else
        {
            turning = false;
        }
        if (turning)
        {
            //Debug.Log("turning");
            Vector3 direction = new Vector3((((globalFlock.sizeXMax - globalFlock.sizeXMin) / 2) + globalFlock.sizeXMin), globalFlock.sizeY, ((globalFlock.sizeZMax - globalFlock.sizeZMin) / 2) + globalFlock.sizeZMin) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

            speed = Random.Range(5, 10);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
	}

    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allDino;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach(GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if(dist <= neighbourDistance)
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
