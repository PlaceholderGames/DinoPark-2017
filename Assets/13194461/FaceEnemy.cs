using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceEnemy : Align {

    public static Transform enemyTarget;

    protected GameObject targetAux;

    public override void Awake()
    {
        base.Awake();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    public override Steering GetSteering()
    {
        Vector3 direction = targetAux.transform.position - transform.position; // enemyTarget.position targetAux.transform.position
        if (direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            target.GetComponent<Agent>().orientation = targetOrientation;
        }
        return base.GetSteering();
    }

    void OnDestroy()
    {
        Destroy(target);
    }
}
