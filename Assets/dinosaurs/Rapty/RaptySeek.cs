public class RaptySeek : RaptyBehaviour
{
    public override RaptySteering GetRaptySteering()
    {
        RaptySteering steering = new RaptySteering();
        steering.linear = target.transform.position - transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }
}
