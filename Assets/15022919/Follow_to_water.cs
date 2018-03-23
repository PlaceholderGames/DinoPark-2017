using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_to_water : MonoBehaviour {

    public float speed = 1.0f;//set this to ankys 1 for speed

    private AStarSearch aStar;
    private ASPathFollower follower;
    public int follower_count;

    
	// Use this for initialization
	void Start () {
        follower = GetComponent<ASPathFollower>();
        aStar = GetComponent<AStarSearch>();
	}
	
	// Update is called once per frame
	void Update () {
        if (aStar.target == null)
        {
            return;
        }
		if((follower.path.nodes.Count < 1) || (follower.path == null))
                follower.path = aStar.path;

        move(follower.getDirectionVector());
        follower_count = follower.path.nodes.Count;
	}
    void move(Vector3 directionvector)
    {
        directionvector *= speed * Time.deltaTime;

        transform.Translate(directionvector, Space.World);
        transform.LookAt(transform.position + directionvector);

    }
}
