using UnityEngine;
using System.Collections;
public class RaptyFlee : RaptyBehaviour {
    public override RaptySteering GetRaptySteering()
    {
        RaptySteering steering = new RaptySteering();
        steering.linear = transform.position - target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
