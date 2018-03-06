﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AnkyAgent : MonoBehaviour
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
    protected AnkySteering steering;
    private Dictionary<int, List<AnkySteering>> groups;
    void Start()
    {
        velocity = Vector3.zero;
        steering = new AnkySteering();
        groups = new Dictionary<int, List<AnkySteering>>();
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
            steering = GetPriorityAnkySteering();
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
        steering = new AnkySteering();
    }
    public void SetAnkySteering(AnkySteering steering)
    {
        this.steering = steering;
    }

    public void SetAnkySteering(AnkySteering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }
    public void SetAnkySteering(AnkySteering steering, int priority)
    {
        if (!groups.ContainsKey(priority))
        {
            groups.Add(priority, new List<AnkySteering>());
        }
        groups[priority].Add(steering);
    }
    public void SetAnkySteering(AnkySteering steering, bool pipeline)
    {
        if (!pipeline)
        {
            this.steering = steering;
            return;
        }
    }
    private AnkySteering GetPriorityAnkySteering()
    {
        AnkySteering steering = new AnkySteering();
        float sqrThreshold = priorityThreshold * priorityThreshold;
        List<int> gIdList = new List<int>(groups.Keys);
        gIdList.Sort();
        foreach (int gid in gIdList)
        {
            steering = new AnkySteering();
            foreach (AnkySteering singleSteering in groups[gid])
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