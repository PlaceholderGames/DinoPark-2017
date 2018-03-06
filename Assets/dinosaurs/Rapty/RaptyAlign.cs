using UnityEngine;
using System.Collections;

public class RaptyAlign : RaptyBehaviour
{

    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public override RaptySteering GetRaptySteering()
    {
        RaptySteering steering = new RaptySteering();
        float targetOrientation = target.GetComponent<RaptyAgent>().orientation;
        float rotation = targetOrientation - agent.orientation;
        rotation = MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);
        if (rotationSize < targetRadius)
            return steering;
        float targetRotation;
        if (rotationSize > slowRadius)
            targetRotation = agent.maxRotation;
            //targetRotation = maxRotation; //Errata?
        else
            targetRotation = agent.maxRotation * rotationSize / slowRadius;
            //targetRotation = maxRotation * rotationSize / slowRadius; //Errata?
        targetRotation *= rotation / rotationSize;
        steering.angular = targetRotation - agent.rotation;
        steering.angular /= timeToTarget;
        float angularAccel = Mathf.Abs(steering.angular);
        if (angularAccel > agent.maxAngularAccel)
        //if (angularAccel > maxAngularAccel) //Errata?
        {
            steering.angular /= angularAccel;
            steering.angular *= agent.maxAngularAccel;
            //steering.angular *= maxAngularAccel; // Errata?
        }
        return steering;
    }
}
