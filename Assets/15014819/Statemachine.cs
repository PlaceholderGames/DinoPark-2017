using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statedino
{ 
    public class Statemachine<T>
    {
        public State<T> currentState { get; private set; }
       
        public T owner;
        
        public Statemachine(T _o)
        {
            owner = _o;
            currentState = null;
        }


        public void ChangeState(State<T> _newstate)
        {
            if (currentState != null)
                currentState.Exitstate(owner);
            currentState = _newstate;
            currentState.Enterstate(owner);
        }
        public void Update()
        {
            if (currentState != null)
                currentState.Updatestate(owner);
        }
    }
    public abstract class State<T>
    {
        public abstract void Enterstate(T _onwer);

        public abstract void Exitstate(T _owner);

        public abstract void Updatestate(T _owner);

    }
}