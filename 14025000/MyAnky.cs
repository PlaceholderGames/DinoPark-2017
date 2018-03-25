using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateStuff;

public class MyAnky : MonoBehaviour
{
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        GRAZING,    // Moving with the intent to find food (will happen after a random period)
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };

    public float hunger = 100, thirst = 100, health = 100;  //sets hunger, health and thirst to 100
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
    public bool blendWeight = false;
    public bool blendPriority = false;
    public float priorityThreshold = 0.2f;
    public bool blendPipeline = false;
    public float maxSpeed = 1;              // max speed set to 1
    public float maxAccel = 3;              // accelerate is set to 3
    public float maxRotation = 180;
    public float maxAngularAccel;
    public float orientation;
    public float rotation = 0;
    public Vector3 velocity;
    protected Steering steering;
    private Dictionary<int, List<Steering>> groups;

    public StateMachine <MyAnky> stateMachine { get; set; }

    public Animator anim;

    private void Start()
    {
        stateMachine = new StateMachine<MyAnky>(this);
        stateMachine.ChangeState(FirstState.Instance);
        gameTimer = Time.time;

        velocity = Vector3.zero;
        steering = new Steering();
        groups = new Dictionary<int, List<Steering>>();

        anim = GetComponent<Animator>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller

    }

    public void Update()
    {
        if(Time.time > gameTimer + 1)             
        {
            gameTimer = Time.time;
            seconds++;
            Debug.Log(seconds);
        }

        if (seconds == 5)
        {
            seconds = 0;
            switchState = !switchState;     // changes state every 5 seconds
        }

        stateMachine.Update();

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
    protected virtual void LateUpdate() // Changed so that I can inherit
    {
        if (blendPriority)
        {
            steering = GetPrioritySteering();
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
            rotation = 180.0f;
        }
        if (steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }
        steering = new Steering();
    }
    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public void SetSteering(Steering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }
    public void SetSteering(Steering steering, int priority)
    {
        if (!groups.ContainsKey(priority))
        {
            groups.Add(priority, new List<Steering>());
        }
        groups[priority].Add(steering);
    }
    public void SetSteering(Steering steering, bool pipeline)
    {
        if (!pipeline)
        {
            this.steering = steering;
            return;
        }
    }
    private Steering GetPrioritySteering()
    {
        Steering steering = new Steering();
        float sqrThreshold = priorityThreshold * priorityThreshold;
        List<int> gIdList = new List<int>(groups.Keys);
        gIdList.Sort();
        foreach (int gid in gIdList)
        {
            steering = new Steering();
            foreach (Steering singleSteering in groups[gid])
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

