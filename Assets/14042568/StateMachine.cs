using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateStuff
{
    public class StateMachine<T>
    {

       public State<T> currentState { get; private set; }
       public T owner;

        public StateMachine (T _o)
        {
            owner = _o;
            currentState = null; 

        }


        public void ChangeState(State<T> _newState)
        {
            if ( currentState != null)
            {
                currentState.ExitState(owner);
            }
           
            currentState = _newState;
            currentState.enterState(owner);
        }

        public void Update()
        {
            
            if (currentState != null)
            {
                Debug.Log("Current state = " + currentState);
                currentState.UpdateState(owner);
            }

        }
    }

    public abstract class State<T>
    {

        public abstract void enterState(T _object);
        public abstract void ExitState(T _owner);
        public abstract void UpdateState(T _owner);


    }
}

