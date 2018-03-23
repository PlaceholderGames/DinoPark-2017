using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://www.youtube.com/watch?v=PaLD1t-kIwM&feature=youtu.be
namespace StateMachineBase
{ 
    public class StateMachine<T>
    {
        public State<T> currentState{get; private set;}
        public T Owner;

        public StateMachine(T _Owner)
        {
            Owner = _Owner;
            currentState = null;
        }

        public void ChangeState(State<T> _newState)
        {
            if (currentState != null)
                currentState.ExitState(Owner);
            currentState = _newState;
            currentState.EnterState(Owner);
        }

        public void Update()
        {

            if (currentState != null)
                currentState.UpdateState(Owner);
            
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T _Owner);
        public abstract void ExitState(T _Owner);
        public abstract void UpdateState(T _Owner);
    }
}