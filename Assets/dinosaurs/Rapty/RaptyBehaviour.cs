using UnityEngine;
using System.Collections;
public class RaptyBehaviour : MonoBehaviour
{
    public float weight = 1.0f;
    public int priority = 1;
    public GameObject target;
    protected RaptyAgent agent;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<RaptyAgent>();
    }
    public virtual void Update()
    {
        if (agent.blendWeight)
            agent.SetRaptySteering(GetRaptySteering(), weight);
        else if (agent.blendPriority)
            agent.SetRaptySteering(GetRaptySteering(), priority);
        else if (agent.blendPipeline)
            agent.SetRaptySteering(GetRaptySteering(), true);
        else
            agent.SetSteering(GetRaptySteering());
    }
    public virtual RaptySteering GetRaptySteering()
    {
        return new RaptySteering();
    }
    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }
    public Vector3 OriToVec(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
