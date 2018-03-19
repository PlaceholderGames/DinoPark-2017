using UnityEngine;
using System.Collections;
public class Flee : AgentBehaviour {

    public GameObject goal;

    public override void Awake()
    {
        /*
        Steering steering = new Steering();
        steering.linear = transform.position - target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
        */
        goal.transform.Translate(target.transform.position.x*100, target.transform.position.y, target.transform.position.z);
  
    }
}
