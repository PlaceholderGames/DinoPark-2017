using UnityEngine;
using System.Collections;
public class Flee : AgentBehaviour {
    internal static Transform RaptyInSight;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        steering.linear = transform.position - target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
