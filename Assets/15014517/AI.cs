using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stateMachine;

public class AI : MonoBehaviour
{
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
	
    public StateMachine<AI> stateMachine { get; set; }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        //stateMachine.ChangeState(Idle.Instance);
        //gameTimer = Time.time;
    }

    private void Update()
    {
        
    }

}
