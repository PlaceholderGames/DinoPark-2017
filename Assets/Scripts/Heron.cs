using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	enum HeronStatus {Idle = 0, Walking = 1, Running = 2};

	float acceleration = 5.0f;
	float turning = 3.0f;

	float maxIdleTime = 4.0f;
	float seekPlayerTime = 6.0f;
	float scaredTime = 4.0f;
	float fishingTime = 30.0f;

	float shyDistance = 10.0f;
	float scaredDistance = 5.0f;

	float strechNeckProbability = 10.0f;

	float fishWalkSpeed = 1.0f;
	float walkSpeed = 1.0f;
	float runSpeed = 1.0f;

	private HeronStatus status = HeronStatus.Idle;

	private float fishWalkAnimSpeed = 0.5f;
	private float walkAnimSpeed = 2.0f;
	private float runAnimSpeed = 9.0f;
		
	private float minHeight = 34.1f;
	private float maxHeight = 42.0f;
	private GameObject[] colliders;

	private float hitTestDistanceIncrement = 1.0f;
	private float hitTestDistanceMax = 50.0f;
	private float hitTestTimeIncrement = 0.2f;

	private Transform myT ;
	private Animation anim;
	private Transform leftKnee; 
	private Transform leftAnkle;
	private Transform leftFoot ;
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

	private GameObject obj;
	private Terrain terr;

	// Use this for initialization
	void Start () {
		obj = GameObject.FindGameObjectWithTag ("Player");
		player = obj.transform;
		myT = transform;
		terr = Terrain.activeTerrain;
		if(terr)
			terrain = terr.terrainData;
		anim = GetComponentInChildren<Animation>();
		anim ["Walk"].speed = walkSpeed;
		anim ["Run"].speed = runSpeed;
		anim ["FishingWalk"].speed = fishWalkSpeed;

		leftKnee = myT.Find("HeronAnimated/MasterMover/RootDummy/Root/Lhip/knee2");
		leftAnkle = leftKnee.Find("ankle2");
		leftFoot = leftAnkle.Find("foot2");
		rightKnee = myT.Find("HeronAnimated/MasterMover/RootDummy/Root/Rhip/knee3");
		rightAnkle = rightKnee.Find("ankle3");
		rightFoot = rightAnkle.Find("foot3");

		//might not work
		colliders = GameObject.FindGameObjectsWithTag ("HeronColliders");

		MainLoop ();
	}

	private IEnumerable MainLoop()
	{
		while (true) {
			yield return SeekPlayer();
			yield return Idle();
			yield return Fish();
		}
	}

	private IEnumerable SeekPlayer()
	{
		double time = 0.0f;
		Vector3 moveDir;

		while (time < seekPlayerTime) {
			moveDir = player.position = myT.position;

			if (moveDir.magnitude < shyDistance) {
				yield return null; 
				//return;
			}

			moveDir.y = 0;
			moveDir = (moveDir.normalized + (myT.forward * 0.5f)).normalized;
			offsetMoveDirection = GetPathDirection (myT.position, moveDir);

			if (offsetMoveDirection != Vector3.zero)
				status = HeronStatus.Walking;
			else
				status = HeronStatus.Idle;

			yield return new WaitForSeconds(hitTestTimeIncrement);
			time += hitTestTimeIncrement;
		}
	}

	private IEnumerable Idle ()
	{
		
	}

	private IEnumerable Scared()
	{

	}

	private IEnumerable Fish()
	{

	}

	private IEnumerable AwareLoop()
	{

	}

	private IEnumerable MoveLoop()
	{

	}

	Vector3 GetPathDirection(Vector3 curPos, Vector3 wantedDirection)
	{
		Vector3 awayFromCollision = TestPosition (curPos);
	}

	float TestDirection ( Vector3 position, Vector3 direction)
	{


	}

	Vector3 TestPosition(Vector3 testPos)
	{

	}

	void LateUpdate()
	{

	}
}
