using UnityEngine;
using StateStuff;

public class IdleState : State<MyRapty>
{
    private static IdleState _instance; // static only declared once
    //private FieldOfView dinoView;

    private IdleState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (_instance == null)
            {
                new IdleState();
            }

            return _instance;
        }
    }

    public override void EnterSTate(MyRapty _owner)
    {
        Debug.Log("Entering Idle State");
        _owner.dinoView.enabled = true;
        //dinoView = _owner.raptor.GetComponent<FieldOfView>();
    }

    public override void ExitState(MyRapty _owner)
    {
        Debug.Log("exiting First STate");
        _owner.dinoView.enabled = false;
    }

    public override void UpdateState(MyRapty _owner)
    {
        /*if (_owner.switchState)
        {
            _owner.stateMachine.ChangeState(EatingState.Instance);
        }*/
        for (int i = 0; i < _owner.dinoView.stereoVisibleTargets.Count; i++)
        {
           if(_owner.dinoView.stereoVisibleTargets[i].CompareTag(_owner.targetMask.tag))
            {
                Debug.Log("found dino");
                _owner.stateMachine.ChangeState(FollowingState.Instance);
            }
            else if (_owner.dinoView.visibleTargets[i].CompareTag(_owner.targetMask.tag))
            {
                if (_owner.dinoView.visibleTargets[i].position.x > _owner.raptor.transform.position.x)
                {
                    _owner.raptor.transform.Rotate(new Vector3(5, 0, 0));
                }
            }
        }

    }
}
