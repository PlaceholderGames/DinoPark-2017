using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heron : MonoBehaviour
{
    enum HeronStatus { Idle = 0, Walking = 1, Running = 2 }

    public float acceleration = 5.00f;
    public float turning = 3.00f;

    public float maxIdleTime = 4.00f;
    public float seekPlayerTime = 6.00f;
    public float scaredTime = 4.00f;
    public float fishingTime = 30.00f;

    public float shyDistance = 10.00f;
    public float scaredDistance = 5.00f;

    public float strechNeckProbability = 10.00f;

    public float fishWalkSpeed = 1.00f;
    public float walkSpeed = 0.75f;
    public float runSpeed = 2.00f;

    private int status = (int)HeronStatus.Idle;

    private float fishWalkAnimSpeed = 0.50f;
    private float walkAnimSpeed = 2.00f;
    private float runAnimSpeed = 9.00f;

    private float minHeight = 64.1f;
    private float maxHeight = 82.00f;
    private HeronCollider colliders = new HeronCollider[];

    private float hitTestDistanceIncrement = 1.00f;
    private float hitTestDistanceMax = 50.00f;
    private float hitTestTimeIncrement = 0.2f;

    private Transform myT;
    private Animation anim;
    private Transform leftKnee;
    private Transform leftAnkle;
    private Transform leftFoot;
    private Transform rightKnee;
    private Transform rightAnkle;
    private Transform rightFoot;

    private Transform player;
    private TerrainData terrain;

    private Vector3 offsetMoveDirection;
    private Vector3 usedMoveDirection;
    private Vector3 velocity;
    private Vector3 forward;
    private bool strechNeck = false;
    private bool fishing = false;
    private double lastSpeed = 0.00;


    private double time = 0.00;
   

    // Use this for initialization
    void Start()
    {
        forward = transform.forward;

        forward = transform.forward;

        var obj = GameObject.FindWithTag("Player");
        player = obj.transform;
        myT = transform;
        var terr = Terrain.activeTerrain;
        if (terr)
            terrain = terr.terrainData;

        anim = GetComponentInChildren(Animation);
        anim["Walk"].speed = walkSpeed;
        anim["Run"].speed = runSpeed;
        anim["FishingWalk"].speed = fishWalkSpeed;

        leftKnee = myT.Find("HeronAnimated/MasterMover/RootDummy/Root/Lhip/knee2");
        leftAnkle = leftKnee.Find("ankle2");
        leftFoot = leftAnkle.Find("foot2");
        rightKnee = myT.Find("HeronAnimated/MasterMover/RootDummy/Root/Rhip/knee3");
        rightAnkle = rightKnee.Find("ankle3");
        rightFoot = rightAnkle.Find("foot3");

        colliders = FindObjectsOfType(HeronCollider);

        MainLoop();
        MoveLoop();
        AwareLoop();


    }

    void SeekPlayer()
    {
        while (time < seekPlayerTime)
        {
            double moveDirection = player.position - myT.position;

            if(moveDirection.magnitude < shyDistance)
            {
                yield;
                return;
            }
            moveDirection.y = 0;
        }
    }
// Update is called once per frame
private void Update()
    {

    }

    void MainLoop()
    {
        while (true)
        {
            yield SeekPlayer();
            yield Idle();
            yield Fish();

        }
    }
}






