using UnityEngine;
using System.Collections;

public class Pursue : Seek
{
    public float maxPrediction;
    private GameObject targetAux;
    private Agent targetAgent;

    public override void Awake()
    {
        base.Awake();
        //target.AddComponent<Agent>();
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject();



        //new
    }

    public void newTarget ()
    {
        base.Awake();
        //target.AddComponent<Agent>();
        
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject();



        //new
    }
    public override Steering GetSteering()
    {
        
        Vector3 test = targetAux.transform.position;
        Vector3 test2 = transform.position;
        Vector3 direction = test - test2;
        float distance = direction.magnitude;
       
        float speed = agent.velocity.magnitude;
        float prediction;
        if (speed <= distance / maxPrediction)
            prediction = maxPrediction;
        else
            prediction = distance / speed;
        target.transform.position = targetAux.transform.position;

        Vector3 anotherTest = targetAgent.velocity;
        float evenMoreTests = prediction;

        target.transform.position += (anotherTest * evenMoreTests);
        return base.GetSteering();
    }

    void OnDestroy ()
    {
        Destroy(targetAux);
        Destroy(target);
    }
}
