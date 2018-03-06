using UnityEngine;
using System.Collections;

public class RaptyDodge : RaptySeek
{
    public float avoidDistance;
    public float lookAhead;

    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
    }

    public override RaptySteering GetRaptySteering()
    {
        RaptySteering steering = new RaptySteering();
        Vector3 position = transform.position;
        Vector3 rayVector = agent.velocity.normalized * lookAhead;
        rayVector += position;;
        Vector3 direction = rayVector - position;
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, lookAhead))
        {
            position = hit.point + hit.normal * avoidDistance;
            target.transform.position = position;
            steering = base.GetRaptySteering();
        }
        return steering;
    }
}
