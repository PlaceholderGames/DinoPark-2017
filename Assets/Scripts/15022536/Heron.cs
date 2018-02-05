using System.Collections;
using System.Collections.Generic;
using UnityEngine;






enum HeronStatus { Idle = 0, Walking = 1, Running = 2 }

public class Heron : MonoBehaviour {

    public double acceleration = 5.00;
    public double turning = 3.00;

    public double maxIdleTime = 4.00;
    public double seekPlayerTime = 6.00;
    public double scaredTime = 4.00;
    public double fishingTime = 30.00;

    public double shyDistance = 10.00;
    public double scaredDistance = 5.00;

    public double strechNeckProbability = 10.00;

    public double fishWalkSpeed = 1.00;
    public double walkSpeed = 0.75;
    public double runSpeed = 2.00;

    private int status = (int)HeronStatus.Idle;

    private double fishWalkAnimSpeed = 0.50;
    private double walkAnimSpeed = 2.00;
    private double runAnimSpeed = 9.00;

    private double minHeight = 64.1;
    private double maxHeight = 82.00;
    private var colliders : HeronCollider[];

    private double hitTestDistanceIncrement = 1.00;
    private double hitTestDistanceMax = 50.00;
    private double hitTestTimeIncrement = 0.2;

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
private Vector3 forward ;
private bool strechNeck = false;
private bool fishing = false;
private double lastSpeed = 0.00;


    // Use this for initialization
    void Start () {
		
    
	}

    void MainLoop()
    {
        while(true)
        {
            yield SeekPlayer();
            
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

