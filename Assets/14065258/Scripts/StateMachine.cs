using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateStuff
{ 
    public class StateMachine<T>
    {
        public State<T> currentState { get; private set; }
        public T Owner; //using state machine

        public StateMachine(T _o)
        {
            Owner = _o;
            currentState = null; // no stae at start
        }

        public void ChangeState(State<T> _newState)
        {
            if (currentState != null)
            {
                currentState.ExitState(Owner);
            }
            currentState = _newState;
            currentState.EnterState(Owner);
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState(Owner);
            }
        }
    }

    public abstract class State<T>
    {
        public abstract void EnterState(T _owner);

        public abstract void ExitState(T _owner);

        public abstract void UpdateState(T _owner);
    }
}
