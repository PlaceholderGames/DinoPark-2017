using UnityEngine;
using StateMachineBase;


public class StateWalking : State<MyAnky>
{

    private static StateWalking _instance;

    private StateWalking()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static StateWalking Instance
    {
        get
        {
            if (_instance == null)
            {
                new StateWalking();
            }

            return _instance;
        }
    }

    public override void EnterState(MyAnky _Owner)
    {
        Debug.Log("Entering GRAZING state");
        _Owner.anim.SetBool("isGrazing", true);

    }

    public override void ExitState(MyAnky _Owner)
    {
        Debug.Log("Exiting GRAZING state");
        _Owner.anim.SetBool("isGrazing", false);
    }

    public override void UpdateState(MyAnky _Owner)
    {
        if (_Owner.ankyEnemies.Count >= 1)
        {
            Debug.Log("Uh oh! Enemies are about.");
            _Owner.stateMachine.ChangeState(StateAlert.Instance);
        }
        else
        {
            if (_Owner.ankyHunger <= _Owner.ankyThirst)
            {
                if (_Owner.transform.position.y <= 35)
                {
                    if (_Owner.ankyThirst <= 90)
                    { 
                        _Owner.stateMachine.ChangeState(StateDrinking.Instance);
                    }
                    else
                    {
                        Debug.Log("Anky's in water, but not thirsty enough!");
                    }
                }

            }
            else
            {
                if (_Owner.transform.position.y >= 50)
                {
                    if (_Owner.ankyHunger <= 90)
                    {
                        _Owner.stateMachine.ChangeState(StateEating.Instance);
                    }
                    else
                    {
                        Debug.Log("Anky's near food, but not hungry enough!");
                    }
                }
            }
        }
    }
}