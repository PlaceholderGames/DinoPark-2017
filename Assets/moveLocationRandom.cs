using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLocationRandom : MonoBehaviour {

    public bool active = false;
    public AStarSearch aStarScript;
    public ASPathFollower pathFollowerScript;
    // Use this for initialization
    void Start ()
    {
        aStarScript = GetComponent<AStarSearch>();
        pathFollowerScript = GetComponent<ASPathFollower>();
    }
	
	// Update is called once per frame
	void Update ()
    {


    }

    /// <summary>
    /// 
    /// </summary>
    public void moveSphere()
    {
        if (active == true)
        {
                this.gameObject.transform.position = aStarScript.mapGrid.findAnky().position;            
        }
    }
}
    