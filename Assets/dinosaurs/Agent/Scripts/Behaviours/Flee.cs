using UnityEngine;
using System.Collections;
public class Flee : AgentBehaviour {

    public static Transform enemyTarget;

    public override Steering GetSteering()
    {

        Steering steering = new Steering();
        steering.linear = transform.position - enemyTarget.position; // target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
