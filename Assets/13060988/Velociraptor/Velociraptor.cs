using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using FiniteStateMachine;
using UnityEditor;

public class Velociraptor : Agent
{
    [HideInInspector] public Animator Animator;
    [HideInInspector] public Wander Wander;
    [HideInInspector] public FieldOfView Sight;
    [HideInInspector] public AStarSearch Search;
    [HideInInspector] public ASPathFollower Path;
    [HideInInspector] public ASAgent SearchAgent;
    [HideInInspector] public GameObject Terrain;
    [HideInInspector] public Seek Seek;
    [HideInInspector] public Flee Flee;
    [HideInInspector] public Arrive Arrive;

    [HideInInspector] public FiniteStateMachine<Velociraptor> State { get; private set; }

    public float Health = 100.0f;
    public float Hunger = 100.0f;
    public float Thirst = 100.0f;
    public float Attack = 12.5f;

    [HideInInspector] private float collision_time;
    [HideInInspector] private float health_time;
    [HideInInspector] private float hunger_time;
    [HideInInspector] private float thirst_time;

    [HideInInspector] private float collision_ticks = 1.75f;
    [HideInInspector] private float health_ticks = 4.75f;
    [HideInInspector] private float hunger_ticks = 2.35f;
    [HideInInspector] private float thirst_ticks = 3.5f;

    [HideInInspector] public List<Transform> LivingPrey;
    [HideInInspector] public List<Transform> DeadPrey;

    // Use this for initialization
    protected override void Start()
    {
        Sight = GetComponent<FieldOfView>();
        Search = GetComponent<AStarSearch>();
        Path = GetComponent<ASPathFollower>();
        Wander = GetComponent<Wander>();
        Seek = GetComponent<Seek>();
        Flee = GetComponent<Flee>();
        Arrive = GetComponent<Arrive>();
        Terrain = GameObject.Find("Terrain");

        Animator = GetComponent<Animator>();
        SearchAgent = GetComponent<ASAgent>();

        State = new FiniteStateMachine<Velociraptor>(this);
        State.Change(V_Idle.Instance);

        Path.enabled = true;
        Path.path = new ASPath();
        Path.enabled = false;

        collision_ticks = 0.0f;
        health_time = 0.0f;
        hunger_time = 0.0f;
        thirst_time = 0.0f;

        base.Start();
    }

    protected override void Update()
    {
        getPrey();

        State.Update();

        health_time += Time.deltaTime;
        if (health_time > health_ticks)
        {
            var regeneration = (Hunger / 2 + Thirst / 2) * 0.03f;

            health_time = 0.0f;
            if (Health < 100.0)
                Health += regeneration;
            else
                Health = 100.0f;
        }

        hunger_time += Time.deltaTime;
        if (hunger_time > hunger_ticks)
        {
            var decay = 0.75f;

            hunger_time = 0.0f;
            Hunger -= decay;
        }

        thirst_time += Time.deltaTime;
        if (thirst_time > thirst_ticks)
        {
            var decay = 0.45f;

            thirst_time = 0.0f;
            Thirst -= decay;
        }

        collision_time += Time.deltaTime;

        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public Transform getClosestLivingPrey()
    {
        Transform closestLivingPrey = null;

        foreach (var prey in LivingPrey)
        {
            var distance = Vector3.Distance(transform.position, prey.position);

            if (closestLivingPrey == null)
                closestLivingPrey = prey;
            else if (distance < Vector3.Distance(transform.position, closestLivingPrey.position))
                closestLivingPrey = prey;
        }

        return closestLivingPrey;
    }

    public Transform getClosestDeadPrey()
    {
        Transform closestDeadPrey = null;

        foreach (var prey in DeadPrey)
        {
            var distance = Vector3.Distance(transform.position, prey.position);

            if (closestDeadPrey == null)
                closestDeadPrey = prey;
            else if (distance < Vector3.Distance(transform.position, closestDeadPrey.position))
                closestDeadPrey = prey;
        }

        return closestDeadPrey;
    }

    private void getPrey()
    {
        LivingPrey = new List<Transform>();
        DeadPrey = new List<Transform>();

        foreach (var target in Sight.visibleTargets)
            if (target.gameObject.tag == "Anky")
            {
                if (target.gameObject.GetComponent<Ankylosaurus>().State.CurrentState is Dead)
                    DeadPrey.Add(target);
                else
                    LivingPrey.Add(target);
            }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Anky" && State.CurrentState is V_Attacking && collision_time > collision_ticks &&
            !(col.gameObject.GetComponent<Ankylosaurus>().State.CurrentState is Dead))
        {
            collision_time = 0.0f;
            col.gameObject.GetComponent<Ankylosaurus>().Health -= Attack;
            Debug.Log(gameObject.name + " dealt " + Attack + " Damage to " + col.gameObject + ".");
        }
    }

    public void move(Vector3 directionVector)
    {
        var speed = SearchAgent.maxSpeed;

        directionVector *= speed * Time.deltaTime;
        transform.Translate(directionVector, Space.World);
        transform.LookAt(transform.position, directionVector);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}