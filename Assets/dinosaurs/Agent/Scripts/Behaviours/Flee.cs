using UnityEngine;
using System.Collections;
public class Flee : AgentBehaviour {
    public override Steering GetSteering()
    {
        Vector3 direction = target.transform.position - transform.position;
        if (direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            
            //Rotates the model and not field of view currently
            //agent.orientation = targetOrientation;
        }
        Steering steering = new Steering();
        
        steering.linear = transform.position - target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
