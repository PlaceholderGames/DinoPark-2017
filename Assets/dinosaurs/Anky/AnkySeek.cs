﻿public class AnkySeek : AnkyBehaviour
{
    public override AnkySteering GetAnkySteering()
    {
        AnkySteering steering = new AnkySteering();
        steering.linear = target.transform.position - transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
