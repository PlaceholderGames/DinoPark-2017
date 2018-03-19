using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;


public class Anky : MonoBehaviour
{
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;

    public StateMachine<Anky> stateMachine { get; set; }

    private void Start()
    {
        stateMachine = new StateMachine<Anky>(this);
        //stateMachine.ChangeState(WanderingState.Instance);
        gameTimer = Time.time;
    }

    private void Update()
    {
        if(Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
            //Debug.Log(seconds);
        }

        if(seconds == 5)
        {
            seconds = 0;
            switchState = !switchState;
        }

        stateMachine.Update();
    }
}
