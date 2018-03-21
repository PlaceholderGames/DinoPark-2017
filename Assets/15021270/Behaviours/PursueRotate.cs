using UnityEngine;
using System.Collections;

public class PursueRotate : Seek
{
    public float maxPrediction;
    private GameObject targetAux;
    private Agent targetAgent;

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    public override Steering GetSteering()
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

        //Doesn't currently work
       if (direction.magnitude > 0.0f)
       {
           float targetOrientation = Mathf.Atan2(direction.x, direction.z);
           targetOrientation *= Mathf.Rad2Deg;
           target.GetComponent<Agent>().orientation = targetOrientation;
       }
       
        return base.GetSteering();
    }

    void OnDestroy ()
    {
        Destroy(targetAux);
    }
}
