using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class AI : MonoBehaviour
{
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;

    public Animator anim;
    public FieldOfView view;
    public Face face;
    public Wander wander;
    public Pursue pursue;
    public Agent agent;

    public StateMachine<AI> stateMachine { get; set; }

    void Awake()
    {
        view = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        face = GetComponent<Face>();
        wander = GetComponent<Wander>();
        pursue = GetComponent<Pursue>();
    }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(IdleState.Instance);
        gameTimer = Time.time;
    }

    private void Update()
    {
        // Idle - should only be used at startup

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        stateMachine.Update();
    }

    public void hunt()
    {
        //this.SetSteering(face.GetSteering());
        //this.SetSteering(pursue.GetSteering());
    }
}