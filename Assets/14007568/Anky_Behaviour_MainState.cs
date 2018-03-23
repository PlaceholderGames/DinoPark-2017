using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



//Finite state 



namespace DinoStateMachine
{
    public class Anky_Behaviour<T>
    {

        public State<T> currentState { get; private set; }

        public T Owner;


        public Anky_Behaviour(T _o)

        {

            Owner = _o;
            currentState = null;

        }

        public void ChangeDinoState(State<T> _newState)
        {

            if (currentState != null)
                currentState.exitState(Owner);
            currentState = _newState;
            currentState.enterState(Owner);



        }

        public void Update()
        {
            if (currentState != null)
                currentState.updateState(Owner);

        }




    }


    public abstract class State<T>
    {

        public abstract void enterState(T _owner);

        public abstract void exitState(T _owner);

        public abstract void updateState(T _owner);

    }



}

/* if.changestatemachine = name {

     //do This
     }*/
