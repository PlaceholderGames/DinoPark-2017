using UnityEngine;
using System.Collections;

public class RaptyEvade : RaptyFlee
{
    public float maxPrediction;
    private GameObject targetAux;
    private RaptyAgent targetAgent;

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<RaptyAgent>();
        targetAux = target;
        target = new GameObject();
    }

    public override RaptySteering GetRaptySteering()
    {
        Vector3 direction = targetAux.transform.position - transform.position;
        float distance = direction.magnitude;
        float speed = agent.velocity.magnitude;
        float prediction;
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;
        target.transform.position = targetAux.transform.position;
        target.transform.position += targetAgent.velocity * prediction;
        return base.GetRaptySteering();
    }

    void OnDestroy ()
    {
        Destroy(targetAux);
    }
}
