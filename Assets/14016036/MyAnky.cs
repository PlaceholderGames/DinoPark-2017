using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StateStuff;


public class MyAnky : Agent
{
    int grassDensity = 20;
    int patchDetail = 20;
    public Animation animClip;
    public float energy = 100;
    public float hunger = 100;
    public float water = 100;
    public Agent ankyAgent;
    public FieldOfView ankyView;
    public Flee ankyFlee;
    public Wander ankyWander;
    public Seek ankySeek;
    //public Face ankyFace;
    public List<Transform> ankyFriendliesClose = new List<Transform>();
    public List<Transform> ankyFriendliesFar = new List<Transform>();
    public List<Transform> ankyEnemies = new List<Transform>();
    public List<Transform> attackableEnemy = new List<Transform>();
    public StateMachine<MyAnky> stateMachine { get; set; }
    public Transform target;
    public int fleeingIndex = 0;
    //public Terrain ankyTerrain;
    public tScript Terrain;
    public float time = 5;
    public int health = 52;
    public int anky;
    public float closestDist = 9999;
    public MapGrid mapGrid;
    public AStarSearch AStarSearch;
    public ASPathFollower PathFollowerO;
    public GameObject AStarTarget;
    public enum ankyState
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on y value of the object to denote grass level
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        GRAZING,    // Moving with the intent to find food (will happen after a random period)
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        HERDING,
        DEAD
    };

    public Animator anim;
    public ankyState currentState;

    // Use this for initialization
    protected override void Start()
    {
        animClip = GetComponent<Animation>();
        
        stateMachine = new StateMachine<MyAnky>(this);
        currentState = ankyState.IDLE;
        stateMachine.ChangeState(WanderingState.Instance);
        ankyView = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        AStarSearch = GetComponent<AStarSearch>();
        PathFollowerO = GetComponent<ASPathFollower>();
        mapGrid = AStarSearch.mapGrid;
        AStarTarget = new GameObject();
        // Assert default animation booleans and floats
        anim.SetBool("isIdle", true);
        anim.SetBool("isEating", false);
        anim.SetBool("isDrinking", false);
        anim.SetBool("isAlerted", false);
        anim.SetBool("isGrazing", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isFleeing", false);
        anim.SetBool("isDead", false);
        anim.SetFloat("speedMod", 1.0f);
        // This with GetBool and GetFloat allows 
        // you to see how to change the flag parameters in the animation controller
        base.Start();
        
    }

    // A Star uses this to move the Anky location, needs to be called many time for the anky to complete the path
    public void move(Vector3 moveDirection)
    {
        float speed = 10.0f;

        moveDirection *= speed * Time.deltaTime;

        transform.Translate(moveDirection, Space.World);
        transform.LookAt(transform.position + moveDirection);
    }
    // Searchs the Map grid for a walkable tile at a Y value of 35.5 or less
    public Vector3 waterLocation()
    {
        Vector2 currentTile = mapGrid.getCoordFromPosition(transform.position);
        float waterLevel = 35.5f;
        bool waterFound = false;
        int step = 1;

        while(!waterFound)
        {
            for (int x = -step; x < step; x++)
            {
                for(int y = -step; y < step; y++)
                {
                    if(Mathf.Abs(x) == step || Mathf.Abs(y) == step) // limits search to tiles on the outside
                    {
                        if(!((currentTile.x + x < 0 || currentTile.x + x > mapGrid.gridWorldSize.x) || (currentTile.x + x < 0 || currentTile.x + x > mapGrid.gridWorldSize.x))) //keeps search in bounds
                        {
                            MapTile searchTile = mapGrid.tiles[(int)(currentTile.x + x), (int)(currentTile.y + y)]; // puts current tile into own variable to be checked

                            if(searchTile.position.y <= waterLevel) // checks if current tile is below or equal to desired y position
                            {
                                return searchTile.position;
                            }
                        }
                    }
                }
                step++;
            }
        }


        return Vector3.zero; // should never reach this point
    }

    protected override void Update()
    {
        hunger -= Time.deltaTime * 0.7f;
        water -= Time.deltaTime * 0.4f;
        // Idle - should only be used at startup
        stateMachine.Update();
        // Eating - requires a box collision with a dead dino

        //// Alerted - up to the student what you do here
        //ankyTerrain.terrainData.SetDetailResolution(grassDensity, patchDetail);
        //int[,] details = new int[grassDensity, patchDetail];
        //for (int i = 0; i < grassDensity; i++)
        //{
        //    for (int j = 0; j < grassDensity; j++)
        //    {
        //        // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
        //        float height = ankyTerrain.terrainData.GetHeight(j, i);
        //        if (height < 10.0f)
        //        {
        //            details[i, j] = 6;
        //        }
        //        else
        //        {
        //            details[i, j] = 0;
        //        }
        //    }
        //}
        //ankyTerrain.terrainData.SetDetailLayer(0, 0, 0, details);
        //details = ankyTerrain.terrainData.GetDetailLayer(0, 0, ankyTerrain.terrainData.detailWidth, ankyTerrain.terrainData.detailHeight, 0);
        //
        //details[(int)transform.position.z / 2000 * 1024, (int)transform.position.x / 2000 * 1024] = 6;
        //
        //ankyTerrain.terrainData.SetDetailLayer(0, 0, 0, details);
        // Drinking - requires y value to be below 32 (?)

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        if(health <= 0)
        {
            ankyFlee.enabled = false;
            ankySeek.enabled = false;
            ankyWander.enabled = false;
            anim.SetBool("isDead", true);
            anim.SetBool("isIdle", false);
            anim.SetBool("isEating", false);
            anim.SetBool("isDrinking", false);
            anim.SetBool("isAlerted", false);
            anim.SetBool("isGrazing", false);
            anim.SetBool("isHerding", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isFleeing", false);
            time -= Time.deltaTime;

            if(time <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        //// aligning ToDo: figure out how to get it to to find the nearest anky in its circle and then move to a distance close by. 

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed
        //ankyFriendliesFar.Clear();
        //findFriendlies();
        base.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    IEnumerator myCoroutine()
    {
        Debug.Log("its getting in this myCoroutine");
        yield return new WaitForSeconds(20);
    }

    public void OnTriggerEnter(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;

        if(Vector3.Dot(transform.forward, direction) == 0)
        {
            Debug.Log("I've been hit");
            health = health - 20;
        }
    }

    public void findFriendlies()
    {
        //ankyFriendliesFar.Clear();
        for (int i = 0; i < ankyView.visibleTargets.Count; i++)
        {
            target = ankyView.visibleTargets[i];
            if (ankyView.visibleTargets[i].tag == "Anky" && Vector3.Distance(target.position, transform.position) > 80)
            {
                if (!ankyFriendliesFar.Contains(target))
                {
                    ankyFriendliesFar.Add(target);
                }
            }
            else if (ankyView.visibleTargets[i].tag == "Anky" && Vector3.Distance(target.position, transform.position) <= 80)
            {
                if (!ankyFriendliesFar.Contains(target))
                {
                    ankyFriendliesClose.Add(target);
                }
            }
        }
        for (int i = 0; i < ankyView.stereoVisibleTargets.Count; i++)
        {
            target = ankyView.stereoVisibleTargets[i];
            if (ankyView.stereoVisibleTargets[i].tag == "Anky" && Vector3.Distance(target.position, transform.position) > 80 && !ankyFriendliesFar.Contains(target))
            {
                if (!ankyFriendliesFar.Contains(target))
                {
                    ankyFriendliesFar.Add(target);
                }
            }
            else if (ankyView.stereoVisibleTargets[i].tag == "Anky" && Vector3.Distance(target.position, transform.position) <= 80 && !ankyFriendliesFar.Contains(target))
            {
                if (!ankyFriendliesFar.Contains(target))
                {
                    ankyFriendliesClose.Add(target);
                }
            }
        }
    }
}
