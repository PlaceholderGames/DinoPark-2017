using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RaptyAgent : MonoBehaviour
{
    public bool blendWeight = false;
    public bool blendPriority = false;
    public float priorityThreshold = 0.2f;
    public bool blendPipeline = false;
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected RaptySteering steering;
    private Dictionary<int, List<RaptySteering>> groups;
    void Start()
    {
        velocity = Vector3.zero;
        steering = new RaptySteering();
        groups = new Dictionary<int, List<RaptySteering>>();
    }
    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }
    public virtual void LateUpdate()
    {
        if (blendPriority)
        {
            steering = GetPriorityRaptySteering();
            groups.Clear();
        }
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (rotation > maxRotation)
        {
            rotation = maxRotation;
        }
        if (steering.angular == 0.0f)
        {
            rotation = 0.0f;
        }
        if (steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }
        steering = new RaptySteering();
    }
    public void SetSteering(RaptySteering steering)
    {
        this.steering = steering;
    }

    public void SetRaptySteering(RaptySteering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }
    public void SetRaptySteering(RaptySteering steering, int priority)
    {
        if (!groups.ContainsKey(priority))
        {
            groups.Add(priority, new List<RaptySteering>());
        }
        groups[priority].Add(steering);
    }
    public void SetRaptySteering(RaptySteering steering, bool pipeline)
    {
        if (!pipeline)
        {
            this.steering = steering;
            return;
        }
    }
    private RaptySteering GetPriorityRaptySteering()
    {
        RaptySteering steering = new RaptySteering();
        float sqrThreshold = priorityThreshold * priorityThreshold;
        List<int> gIdList = new List<int>(groups.Keys);
        gIdList.Sort();
        foreach (int gid in gIdList)
        {
            steering = new RaptySteering();
            foreach (RaptySteering singleSteering in groups[gid])
            {
                steering.linear += singleSteering.linear;
                steering.angular += singleSteering.angular;
            }
            if (steering.linear.magnitude > priorityThreshold ||
                    Mathf.Abs(steering.angular) > priorityThreshold)
            {
                return steering;
            }
        }
        return steering;
    }
}