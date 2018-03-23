
using UnityEngine;
using Statedino;


public class DrinkingState : State<MyAnky>
{
    private static DrinkingState _instance;

    private DrinkingState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static DrinkingState instance
    {
        get
        {
            if (_instance == null)
            {
                new DrinkingState();
            }
            return _instance;
        }
    }

    public override void Enterstate(MyAnky _onwer)
    {
        Debug.Log("Entering the drinking state");
    }

    public override void Exitstate(MyAnky _owner)
    {
        _owner.ankySeek.enabled = false;
    }

    public override void Updatestate(MyAnky _owner)
    {
        _owner.Water = GameObject.FindGameObjectWithTag("Water");
        Debug.Log(_owner.Water);
        _owner.ankySeek.target = _owner.Water;
        _owner.ankySeek.Awake();
        _owner.wander.enabled = false;
        _owner.ankySeek.enabled = true;
        if (_owner.ankyPosition.transform.position.y < 36)
        {
            
            if (!_owner.drink)
            {
                _owner.drink = true;
                _owner.ankySeek.enabled = false;
                _owner.myAnky.Drinkypo();

            }

        }
        if (_owner.energy == 25)
        {
            _owner.Statemachine.ChangeState(GrazingState.instance);
        }
      
    }
}
