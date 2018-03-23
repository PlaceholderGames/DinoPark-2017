using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class V_Hunting : State<Velociraptor>
{
    private static V_Hunting _instance;

    private V_Hunting()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static V_Hunting Instance
    {
        get
        {
            if (_instance == null)
                new V_Hunting();

            return _instance;
        }
    }

    public override void Enter(Velociraptor parent)
    {
        parent.Animator.SetBool("isHunting", true);
    }

    public override void Exit(Velociraptor parent)
    {
        parent.Animator.SetBool("isHunting", false);
    }

    public override void Update(Velociraptor parent)
    {
        if (parent.DeadPrey.Count > 0)
        {
            parent.Wander.enabled = false;
            parent.Wander.target = null;

            if (Vector3.Distance(parent.transform.position, parent.getClosestDeadPrey().transform.position) < 10.0f)
            {
                parent.Arrive.target = null;
                parent.Arrive.enabled = false;
                parent.State.Change(V_Eating.Instance);
            }
            else
            {
                if (!parent.Arrive.enabled)
                {
                    parent.Arrive.target = parent.getClosestDeadPrey().gameObject;
                    parent.Arrive.enabled = true;
                }
            }
        }
        else if (parent.LivingPrey.Count > 0)
        {
            parent.Wander.enabled = false;
            parent.Wander.target = null;
            parent.State.Change(V_Alerted.Instance);
        }
        else if (parent.Thirst < 65.0f)
        {
            if (parent.transform.position.y > 35.0f)
            {
                if (parent.Path.path.nodes.Count == 0)
                {
                    parent.Search.enabled = true;
                    parent.Path.enabled = true;
                    parent.SearchAgent.enabled = true;

                    List<Vector2> waterTiles = new List<Vector2>();

                    for (var y = 0; y < 2000; y += 25)
                    {
                        for (var x = 0; x < 2000; x += 25)
                        {
                            if (parent.Terrain.GetComponent<Terrain>().SampleHeight(new Vector3(x, 0, y)) < 35.0f)
                                waterTiles.Add(new Vector2(x, y));
                        }
                    }

                    Vector2 closestWater = new Vector2();

                    foreach (var waterTile in waterTiles)
                    {
                        var distance =
                            Vector2.Distance(new Vector2(parent.transform.position.x, parent.transform.position.z),
                                waterTile);

                        if (closestWater == new Vector2(1000, 1000))
                            closestWater = waterTile;
                        else if (distance < Vector2.Distance(
                                     new Vector2(parent.transform.position.x, parent.transform.position.z),
                                     closestWater))
                            closestWater = waterTile;
                    }

                    parent.Search.mapGrid.seaLevel = 20.0f;
                    parent.Search.target = Object.Instantiate(new GameObject(),
                        new Vector3(closestWater.x, 30.0f, closestWater.y), Quaternion.identity);
                    parent.Path.path = parent.Search.path;
                }
                else
                {
                    parent.move(parent.Path.getDirectionVector());
                }
            }
            else
            {
                parent.Search.enabled = false;
                parent.Path.enabled = false;
                parent.SearchAgent.enabled = false;
                parent.Search.target = null;
                parent.Path.path = null;
                parent.Wander.enabled = false;
                parent.Wander.target = null;
                parent.State.Change(V_Drinking.Instance);
            }
        }
        else
        {
            if (!parent.Wander.enabled)
                parent.Wander.enabled = true;
        }
    }
}