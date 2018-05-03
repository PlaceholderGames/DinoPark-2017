using UnityEngine;
using Statestuff;

public class GrazingState : State<MyAnky>
{
    private static GrazingState _instance;
    int[,] details;
    private GrazingState()
    {
        if (_instance != null)
        {
            return;
        }
        _instance = this;

    }

    public static GrazingState Instance
    {
        get
        {
            if (_instance == null)
            {
                new GrazingState();
            }
            return _instance;
        }
    }

    public override void EnterState(MyAnky _owner)
    {
        Debug.Log("Entering Grazing State");
        _owner.anim.SetBool("isGrazing",true);
        _owner.wanderScript.enabled = true;
    }

    public override void ExitState(MyAnky _owner)
    {
        Debug.Log("Exiting Grazing State");
        _owner.anim.SetBool("isGrazing", false);
        _owner.wanderScript.enabled = false;
        _owner.seekingScript.enabled = false;
        
    }

    public override void UpdateState(MyAnky _owner)
    {

        if(_owner.hasChild==false&&_owner.numberOfChildren <4)
        {
            _owner.breeding();
            _owner.anim.SetFloat("speedMod", 0.8f);
          
        }
        else
        {
            _owner.anim.SetFloat("speedMod", 1.0f);
        }

        foreach(Transform i in _owner.AnkyInView)
        {
            float distance = Vector3.Distance(i.transform.position, _owner.transform.position);
            if(distance >50)
            {
                _owner.seekingScript.target = i.gameObject;
                _owner.wanderScript.enabled = false;
                _owner.seekingScript.enabled = true;
            }
            if(distance <10) 
            {
                _owner.seekingScript.enabled = false;
                _owner.wanderScript.enabled = true;
            }
        }

        details = _owner.TerrainScript.Terrain.terrainData.GetDetailLayer(0, 0, _owner.TerrainScript.Terrain.terrainData.detailWidth,_owner.TerrainScript.Terrain.terrainData.detailHeight,0);
        if(details[(int)_owner.transform.position.z,(int)_owner.transform.position.x] != 0)
        {
           if(_owner.sustenance < 75)
            {
                _owner.stateMachine.ChangeState(EatingState.Instance);
                _owner.currentAnkyState = MyAnky.ankyState.EATING;
            } 
        }

        ////////
        //State Changes
        ////////
        if (_owner.RaptorsInView.Count > 0)
        {
            _owner.stateMachine.ChangeState(AlertState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.ALERTED;
        }

        if(_owner.hydration < 65)
        {
            _owner.stateMachine.ChangeState(DrinkingState.Instance);
            _owner.currentAnkyState = MyAnky.ankyState.DRINKING;
        }

        
    }
}
