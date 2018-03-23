using UnityEngine;
using System.Collections;

public class Face : Align {

    protected GameObject targetAux;

    public override void Awake()
    {
        base.Awake();
        target = new GameObject();
        target.AddComponent<Agent>();
        targetAux = target;
    }

   
    public override Steering GetSteering()
    {
        Vector3 direction =  transform.position - targetAux.transform.position;
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
        Destroy(target);
    }
}
