using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePatrol : MonoBehaviour {

    GameObject NPC;
    public  GameObject[] waypoints;
    int currentWP;


    void Awake()
    {
        waypoints = GameObject.FindGameObjectsWithTag("waypoints");
    }

    // Use this for initialization
    void Start ()
    {
        //NPC = animator.gameObject;
        NPC = this.gameObject ;
        currentWP = 0;
    }
	
   

	// Update is called once per frame
	void Update ()
    {

        if (waypoints.Length == 0)
        {
            Debug.Log("No waypoints found");
            return;
        }
        else
        { 
            Debug.Log("Waypoints found");
        }
            
        if (Vector3.Distance(waypoints[currentWP].transform.position,
                            NPC.transform.position) < 3.0f)
        {
            currentWP++;
            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
         

        }

     

        //rotate towards target
        var direction = waypoints[currentWP].transform.position - NPC.transform.position;
        direction.y = 0;
        NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                        1.0f * Time.deltaTime * 2.0f);
        NPC.transform.Translate(0, 0, Time.deltaTime * 2.0f);
    }

}
