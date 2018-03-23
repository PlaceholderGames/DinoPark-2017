using UnityEngine;
using Statestuff;

public class GrazeState : State<MyAnky>
{
    private static GrazeState _instance;
    int[,] grass;
    private GrazeState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static GrazeState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazeState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", true);
        Debug.Log("entering Grazingstate");
        _owner.ankyFlee.enabled = false;
        _owner.ankyWander.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        _owner.anim.SetBool("isGrazing", false);
        Debug.Log("exiting GrazeState");
        _owner.ankyWander.enabled = false;
        _owner.ankySeek.enabled = false;
    }

    public override void UpdateState(MyAnky _owner)
    {

        if (_owner.Enemies.Count > 0)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
        }
        if (_owner.hydration < 50)
        {

            _owner.stateMachine.ChangeState(DrinkingState.Instance);
        }
      
        grass = _owner.GrassScript.terrain.terrainData.GetDetailLayer(0, 0, _owner.GrassScript.terrain.terrainData.detailWidth, _owner.GrassScript.terrain.terrainData.detailHeight, 0);
        if (_owner.saturation < 99 && grass[(int)_owner.transform.position.z, (int)_owner.transform.position.x] != 0)
        {

            _owner.stateMachine.ChangeState(EatingState.Instance);
        }

        foreach (Transform x in _owner.friends)
        {

            float FriendDistance = Vector3.Distance(x.position, _owner.transform.position);
            if (FriendDistance > 50 )
            {
                _owner.ankySeek.target = x.gameObject;
                _owner.ankyWander.enabled = false;
                _owner.ankySeek.enabled = true;         

            }
            else if (FriendDistance < 25)
            {
                _owner.ankySeek.enabled = false;
                _owner.ankyWander.enabled = true;
            }

        }


        //for (int i = 0; i < _owner.fov.visibleTargets.Count; i++)
        //{
        //    Debug.Log("1");
        //    Transform target = _owner.fov.visibleTargets[i];
        //    if (target.tag == "Rapty")
        //    {
        //        Debug.Log("2");
        //        _owner.Enemies.Add(target);
        //        _owner.stateMachine.ChangeState(AlertState.Instance);
        //    }
        //}

    }
}
