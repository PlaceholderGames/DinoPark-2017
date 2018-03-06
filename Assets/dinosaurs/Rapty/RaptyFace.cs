using UnityEngine;
using System.Collections;

public class RaptyFace : RaptyAlign {

    protected GameObject targetAux;

    public override void Awake()
    {
        base.Awake();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<RaptyAgent>();
    }

    public override RaptySteering GetRaptySteering()
    {
        Vector3 direction = targetAux.transform.position - transform.position;
        if (direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            target.GetComponent<RaptyAgent>().orientation = targetOrientation;
        }
        return base.GetRaptySteering();
    }

    void OnDestroy ()
    {
        Destroy(target);
    }
}
