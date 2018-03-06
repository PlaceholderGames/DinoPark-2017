﻿using UnityEngine;
using System.Collections;
public class AnkyFlee : AnkyBehaviour {
    public override AnkySteering GetAnkySteering()
    {
        AnkySteering steering = new AnkySteering();
        steering.linear = transform.position - target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
