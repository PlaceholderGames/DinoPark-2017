using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateStuff
{
    public class StateMachine<MyAnky>
    {
        public State<MyAnky> currentState { get; private set; }
        public MyAnky Owner;

        public StateMachine(MyAnky _o)
        {
            Owner = _o;
            currentState = null;
        }

        public void ChangeState(State<MyAnky> _newstate)
        {
            if (currentState != null)
                currentState.ExitState(Owner);
            currentState = _newstate;
            currentState.EnterState(Owner);
        }

        public void Update()
        {
            if (currentState != null)
                currentState.UpdateState(Owner);
        }
    }
    public abstract class State<MyAnky>
    {
        public abstract void EnterState(MyAnky _owner);
        public abstract void ExitState(MyAnky _owner);
        public abstract void UpdateState(MyAnky _owner);
    }
}