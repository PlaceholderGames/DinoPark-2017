using UnityEngine;
using System.Collections.Generic;
using System;
using FiniteStateMachine;

public class Ankylosaurus : Agent
{
    [HideInInspector] public GameObject herdCentre;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public Wander Wander;
    [HideInInspector] public FieldOfView Sight;
    [HideInInspector] public AStarSearch Search;
    [HideInInspector] public ASPathFollower Path;
    [HideInInspector] public GameObject Terrain;
    [HideInInspector] public ASAgent SearchAgent;
    [HideInInspector] public Face Face;
    [HideInInspector] public Flee Flee;
    [HideInInspector] public Agent Agent;

    [HideInInspector] public FiniteStateMachine<Ankylosaurus> State { get; private set; }

    public bool death = false;

    public float Health = 100.0f;
    public float Hunger = 100.0f;
    public float Thirst = 100.0f;
    public float Attack = 20.0f;
    public float Meat = 100.0f;

    [HideInInspector] private float collision_time;
    [HideInInspector] private float health_time;
    [HideInInspector] private float hunger_time;
    [HideInInspector] private float thirst_time;

    [HideInInspector] private float collision_ticks = 2.5f;
    [HideInInspector] private float health_ticks = 4.75f;
    [HideInInspector] private float hunger_ticks = 2.35f;
    [HideInInspector] private float thirst_ticks = 3.5f;

    [HideInInspector] public List<Transform> Predators;

    // Use this for initialization
    protected override void Start()
    {
        Sight = GetComponent<FieldOfView>();
        Search = GetComponent<AStarSearch>();
        Path = GetComponent<ASPathFollower>();
        Wander = GetComponent<Wander>();
        Flee = GetComponent<Flee>();
        Face = GetComponent<Face>();
        Agent = GetComponent<Agent>();
        Animator = GetComponent<Animator>();

        Terrain = GameObject.Find("Terrain");
        SearchAgent = GetComponent<ASAgent>();
        herdCentre = GameObject.Find("HerdCentre").gameObject;

        Path.enabled = true;
        Path.path = new ASPath();
        Path.enabled = false;

        State = new FiniteStateMachine<Ankylosaurus>(this);
        State.Change(Idle.Instance);

        collision_time = 0.0f;
        health_time = 0.0f;
        hunger_time = 0.0f;
        thirst_time = 0.0f;

        base.Start();
    }

    protected override void Update()
    {
        UpdatePredators();

        State.Update();

        health_time += Time.deltaTime;
        if (health_time > health_ticks)
        {
            float regeneration = (Hunger / 2 + Thirst / 2) * 0.03f;

            health_time = 0.0f;
            if (Health < 100.0)
                Health += regeneration;
            else
                Health = 100.0f;
        }

        hunger_time += Time.deltaTime;
        if (hunger_time > hunger_ticks)
        {
            float decay = 0.75f;

            hunger_time = 0.0f;
            Hunger -= decay;
        }

        thirst_time += Time.deltaTime;
        if (thirst_time > thirst_ticks)
        {
            float decay = 0.45f;

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

    public Transform GetClosestPredator()
    {
        Transform closestPredator = null;

        foreach (var predator in Predators)
        {
            var distance = Vector3.Distance(transform.position, predator.position);

            if (closestPredator == null)
                closestPredator = predator;
            else if (distance < Vector3.Distance(transform.position, closestPredator.position))
                closestPredator = predator;
        }

        return closestPredator;
    }

    private void UpdatePredators()
    {
        Predators = new List<Transform>();

        foreach (var target in Sight.visibleTargets)
        {
            if (target.gameObject.CompareTag("Rapty"))
                Predators.Add(target);
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Rapty" && State.CurrentState is Attacking && collision_time > collision_ticks &&
            !(col.gameObject.GetComponent<Velociraptor>().State.CurrentState is V_Dead))
        {
            collision_time = 0.0f;
            col.gameObject.GetComponent<Velociraptor>().Health -= Attack;
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