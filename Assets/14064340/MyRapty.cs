using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Statestuff;

public class MyRapty : Agent
{
    public FieldOfView fov;
    public Transform go;
    public Transform t;


    public enum raptyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };
    public Animator anim;
    public Seek RaptySeek;
    public Face RaptyFace;
    public bool switchState = false;
    public double saturation = 100;
    public double hydration = 100;
    public double health = 100;
    public float gameTimer;
    public int seconds = 0;
    public GameObject Water;
    public List<Transform> Enemies = new List<Transform>();
    public List<Transform> friends = new List<Transform>();

    public StateMachine<MyRapty> stateMachine { get; set; }

    public bool takeDamage(int damage)
    {
        health -= damage;

        return false;
    }


    // Use this for initialization
    protected override void Start()
    {
        stateMachine = new StateMachine<MyRapty>(this);
        stateMachine.ChangeState(HuntState.Instance);
        gameTimer = Time.time;


        anim = GetComponent<Animator>();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isHunting", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        path = new UnityEngine.AI.NavMeshPath();
        base.Start();
    }

    UnityEngine.AI.NavMeshPath path;

    public Vector3[] corners;

    public int pathDone = 0;

    Vector3 pathDir;

    

    protected override void Update()
    {
        // Eating - Drinking 
        if (hydration > 0)
        {
            hydration -= (Time.deltaTime * 0.2) * 1.0;
        }
        if (saturation > 0)
        {
            saturation -= (Time.deltaTime * 0.1) * 1.0;
        }

        if (hydration <= 0 || saturation <= 0 || health <= 0)
        {

            stateMachine.ChangeState(RaptyDeadState.Instance);
        }


        // Alerted - up to the student what you do here
        Enemies.Clear();
        foreach (Transform i in fov.visibleTargets)
        {
            if (i.tag == "Anky")
            {
                Enemies.Add(i);
            }
        }
        foreach (Transform i in fov.stereoVisibleTargets)
        {
            if (i.tag == "Anky")
            {
                Enemies.Add(i);
            }
        }


        foreach (Transform x in Enemies)
        {
            Vector3 Difference = new Vector3();
            Vector3 RaptorDiff = new Vector3();

            Difference = (transform.position - x.position);
            RaptorDiff = (transform.position - t.transform.position);

            if (Difference.magnitude < RaptorDiff.magnitude)
            {
               t= x.transform;
            }

        }

        if (t)
        {
            RaptySeek.target = t.gameObject;
        }

        UnityEngine.AI.NavMesh.CalculatePath(transform.position, t.position, UnityEngine.AI.NavMesh.AllAreas, path);
        corners = path.corners;
        if (path.corners.Length <= 1) return;

        for (int i = 0; i < path.corners.Length; i++)
        {
            int indd = i - 1;
            if (indd <= 0) indd = 0;
            Debug.DrawLine(path.corners[i], path.corners[indd], Color.red, 0.2f);
        }
        go.position = path.corners[1];
        RaptySeek.target = t.gameObject;
        RaptyFace.enabled = false;
        RaptySeek.enabled = true;

        stateMachine.Update();
        base.Update();

        //find ankys
        //anky = found anky
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.tag == "Anky")
        {
            takeDamage(30);
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
}
