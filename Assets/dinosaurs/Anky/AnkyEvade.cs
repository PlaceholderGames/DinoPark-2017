using UnityEngine;
using System.Collections;

public class AnkyEvade : AnkyFlee
{
    public float maxPrediction;
    private GameObject targetAux;
    private AnkyAgent targetAgent;

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<AnkyAgent>();
        targetAux = target;
        target = new GameObject();
    }

    public override AnkySteering GetAnkySteering()
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
        return base.GetAnkySteering();
    }

    void OnDestroy ()
    {
        Destroy(targetAux);
    }
}
