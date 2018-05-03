using UnityEngine;
using Statestuff;

public class AlertState : State<MyAnky>
{
    private static AlertState _instance;

    int[,] details;

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
        _owner.anim.SetBool("isAlerted", true);
        _owner.wanderScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Alert State");
        _owner.anim.SetBool("isAlerted", false);
        _owner.wanderScript.enabled = false;
        _owner.seekingScript.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {

        foreach (Transform Raptors in _owner.RaptorsInView)
        {
            float distance = Vector3.Distance(Raptors.position, _owner.transform.position);
            if (distance < 30 && distance > 10)
            {
                _owner.fleeScript.target = Raptors.gameObject;
                _owner.stateMachine.ChangeState(FleeingState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.FLEEING;
            }
            if (distance < 10)
            {
                _owner.stateMachine.ChangeState(AttackState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.ATTACKING;
            }
        }

        foreach (Transform i in _owner.AnkyInView)
        {
            float distance = Vector3.Distance(i.transform.position, _owner.transform.position);
            if (distance > 40)
            {
                _owner.seekingScript.target = i.gameObject;
                _owner.wanderScript.enabled = false;
                _owner.seekingScript.enabled = true;
            }
            if (distance < 15)
            {                
                _owner.seekingScript.enabled = false;
                _owner.seekingScript.target = null;
                _owner.wanderScript.enabled = true;
            }
        }

        if (_owner.RaptorsInView.Count < 1)
        {
            _owner.stateMachine.ChangeState(GrazingState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.GRAZING;
        }

        if (_owner.hydration < 40)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.DRINKING;
        }

        

        details = _owner.TerrainScript.Terrain.terrainData.GetDetailLayer(0, 0, _owner.TerrainScript.Terrain.terrainData.detailWidth, _owner.TerrainScript.Terrain.terrainData.detailHeight, 0);
        if (details[(int)_owner.transform.position.z, (int)_owner.transform.position.x] != 0)
        {
            if (_owner.sustenance < 45)
            {
                _owner.stateMachine.ChangeState(EatingState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.EATING;
            }
        }

    }
}
