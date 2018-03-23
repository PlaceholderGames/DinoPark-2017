using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class Grazing : State<Ankylosaurus>
{
    private static Grazing _instance;

    private Grazing()
    {
        if (_instance != null)
            return;

        _instance = this;
    }

    public static Grazing Instance
    {
        get
        {
            if (_instance == null)
                new Grazing();

            return _instance;
        }
    }

    public override void Enter(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isGrazing", true);
    }

    public override void Exit(Ankylosaurus parent)
    {
        parent.Animator.SetBool("isGrazing", false);
    }

    public override void Update(Ankylosaurus parent)
    {
        if (parent.Predators.Count > 0)
        {
            parent.Search.enabled = false;
            parent.Path.enabled = false;
            parent.SearchAgent.enabled = false;
            parent.Search.target = null;
            parent.Wander.enabled = false;
            parent.Wander.target = null;
            parent.State.Change(Alerted.Instance);
        }
        else if (parent.Hunger < 65.0f)
        {
            RaycastHit hit;
            var rayLength = 2.0f;

            var mouth = parent.transform
                .Find("Armature/Chest/Neck/HeadRig/DownMouthStart/DownMouthRig/DownMouthRig_end").position;

            Debug.DrawRay(mouth, -parent.transform.up * rayLength, Color.red);

            // Shoot raycast
            if (Physics.Raycast(mouth, -parent.transform.up, out hit, rayLength))
            {
                if (parent.Terrain.GetComponent<Grass>().Details[(int) hit.point.z, (int) hit.point.x] != 0)
                {
                    parent.Wander.enabled = false;
                    parent.Wander.target = null;
                    parent.State.Change(Eating.Instance);
                }
            }
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
                parent.State.Change(Drinking.Instance);
            }
        }
        else if (Vector3.Distance(parent.transform.position, parent.herdCentre.transform.position) > 40.0f &&
                 parent.Path.path.nodes.Count == 0)
        {
            parent.Wander.enabled = false;

            if (!parent.Search.enabled)
            {
                parent.Search.enabled = true;
                parent.Path.enabled = true;
                parent.SearchAgent.enabled = true;

                parent.Search.target = parent.herdCentre;

                parent.Path.path = parent.Search.path;
            }
        }
        else if (Vector3.Distance(parent.transform.position, parent.herdCentre.transform.position) > 15.0f &&
                 parent.Path.path.nodes.Count > 0)
        {
            parent.move(parent.Path.getDirectionVector());
        }
        else
        {
            if (parent.Search.enabled)
            {
                parent.Search.enabled = false;
                parent.Path.enabled = false;
                parent.SearchAgent.enabled = false;
                parent.Search.target = null;
                parent.Path.path = null;
            }
            else
            {
                if (!parent.Wander.enabled)
                    parent.Wander.enabled = true;
            }
        }
    }
}