using UnityEngine;
using Statestuff;

public class ankyGrazingState : State<MyAnky>
{
    private static ankyGrazingState _instance;
    int[,] details;
    private ankyGrazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static ankyGrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ankyGrazingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("entering graze state");
        _owner.currentAnkyState = MyAnky.ankyState.GRAZING;
        _owner.anim.SetBool("isGrazing", true);
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("exiting graze state");
        _owner.anim.SetBool("isGrazing", false);
        _owner.ankyWander.enabled = false;
        _owner.ankySeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {
        if (_owner.enemies.Count > 0)
         {
             _owner.stateMachine.ChangeState(ankyAlertState.Instance);
         }

        if (_owner.hydration < 75) {
            _owner.stateMachine.ChangeState(ankyDrinkingState.Instance);
        }

        details = _owner.Terrain.Terrain.terrainData.GetDetailLayer(0, 0, _owner.Terrain.Terrain.terrainData.detailWidth, _owner.Terrain.Terrain.terrainData.detailHeight, 0);
        if (details[(int)_owner.transform.position.z, (int)_owner.transform.position.x] != 0)
        {
            if (_owner.energy < 75)
            {
                _owner.stateMachine.ChangeState(ankyEatingState.Instance);
            }
        }


        foreach (Transform friend in _owner.friendlies)
        {
            float distance = Vector3.Distance(friend.position, _owner.transform.position);
            if (distance > 50)
            {
                _owner.ankySeek.target = friend.gameObject;
                _owner.ankyWander.enabled = false;
                _owner.ankySeek.enabled = true;
            }
            if (distance < 20)
            {
                _owner.ankySeek.enabled = false;
                _owner.ankyWander.enabled = true;
            }
        }

        if (_owner.health <= 0)
        {
            _owner.stateMachine.ChangeState(ankyDeadState.Instance);
        }
    }
}