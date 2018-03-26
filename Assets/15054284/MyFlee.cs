using UnityEngine;
using System.Collections;
public class MyFlee : AgentBehaviour
{
   
    protected GameObject targetAux;
    private float mytargetOrientation;
    public Face facing;
    public float offset;
    public float radius;
   

    public override void Awake()
    {
        base.Awake();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
        
      
    }
    public override Steering GetSteering()
    {

        Vector3 direction = targetAux.transform.position - transform.position;
        if (direction.magnitude > 0.0f)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            agent.orientation = targetOrientation -180;
        }
        
    
        Steering steering = new Steering();
        steering.linear = transform.position - target.transform.position;
        steering.linear.Normalize();
        steering.linear = steering.linear * agent.maxAccel;
        return steering;
    }

       
       
}


