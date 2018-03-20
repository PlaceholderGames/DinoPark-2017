using UnityEngine;
using Statestuff;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;
    
    private AlertState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static AlertState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AlertState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Alert State");
        _owner.wanderScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
        _owner.wanderScript.enabled = false;
    }

    public Vector3 closestRaptor = new Vector3(10000, 10000, 10000);
    public int closestRaptorIndex = 0;
    public GameObject Raptor = new GameObject();

    public override void UpdateState(MyAnky _owner)
    {
        foreach (Transform i in _owner.fov.visibleTargets)
        {
            if (i.tag == "Rapty" && !_owner.RaptorsInView.Contains(i))
            {
                _owner.RaptorsInView.Add(i);
                _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
            }
        }

        foreach (Transform i in _owner.fov.stereoVisibleTargets)
        {
            if (i.tag == "Rapty" && !_owner.RaptorsInView.Contains(i))
            {
                _owner.RaptorsInView.Add(i);
                _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
            }
        }

        /*
        for (int i = 0; i < _owner.RaptorsInView.Count;i++ )
        {
            Debug.Log(_owner.RaptorsInView.Count);
        }
        */
        /*for(int i =0; i < _owner.RaptorsInView.Count; i++)
        {
            Vector3 Difference = new Vector3();
            Difference = (_owner.transform.position - _owner.RaptorsInView[i].position);
            if(Difference.magnitude < closestRaptor.magnitude)
            {
                closestRaptor = Difference;
                closestRaptorIndex = 1;
                Raptor = _owner.RaptorsInView[i].gameObject;
            }
        }
        if(Raptor)
        {
            _owner.stateMachine.ChangeState(FleeingState.Instance);
            _owner.fleeScript.target = Raptor;
        }*/
        
        foreach (Transform Raptors in _owner.RaptorsInView)
        {
            float distance = Vector3.Distance(Raptors.position, _owner.transform.position);
            if (distance < 30 && distance > 5)
            {
                _owner.fleeScript.target = Raptors.gameObject;
                _owner.stateMachine.ChangeState(FleeingState.Instance);
            }
            else if (distance < 5)
            {
                _owner.stateMachine.ChangeState(AttackState.Instance);
            }
        }
       
        ////////////////////////////
        //Grazing State//
        ////////////////////////////
        if (_owner.currentAnkyState == MyAnky.ankyState.GRAZING)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
        }
        ////////////////////////////
        //Drinking State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.DRINKING)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
        ////////////////////////////
        //Eating State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.EATING)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }
        ////////////////////////////
        //Fleeing State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.FLEEING)
        {
            _owner.stateMachine.ChangeState(FleeingState.Instance);
        }
        ////////////////////////////
        //Attacking State//
        ////////////////////////////
        else if (_owner.currentAnkyState == MyAnky.ankyState.ATTACKING)
        {
            _owner.stateMachine.ChangeState(AttackState.Instance);
        }
    }
}
