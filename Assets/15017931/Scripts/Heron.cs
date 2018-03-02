using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heron : MonoBehaviour {

	public float acceleration = 5.0f;
	public float turning = 3.0f;

	public float maxIdleTime = 4.0f;
	public float seekPlayerTime = 6.0f;
	public float scaredTime = 4.0f;
	public float fishingTime = 30.0f;

	public float shyDistance = 10.0f;
	public float scaredDistance = 5.0f;

	public float strechNeckProbability = 10.0f;

	public float fishWalkSpeed = 1.0f;
	public float walkSpeed = 1.0f;
	public float runSpeed = 1.0f;

	private HeronStatus status = HeronStatus.Idle;

	private float fishWalkAnimSpeed = 0.5f;
	private float walkAnimSpeed = 2.0f;
	private float runAnimSpeed = 9.0f;
		
	private float minHeight = 34.1f;
	private float maxHeight = 42.0f;
	private HeronCollider[] colliders;

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

	private Transform player = null;
	private TerrainData terrain;

	private Vector3 offsetMoveDirection;
	private Vector3 usedMoveDirection;
	private Vector3 velocity;
	private Vector3 forward;
	private bool strechNeck = false;
	private bool fishing = false;
	private float lastSpeed = 0.00f;

	enum HeronStatus {Idle = 0, Walking = 1, Running = 2};

	// Use this for initialization
	void Start () {
		forward = transform.forward;

		GameObject obj = GameObject.FindWithTag ("Player");
		player = obj.transform;
		myT = transform;
		Terrain terr = Terrain.activeTerrain;
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

		colliders = FindObjectsOfType<HeronCollider>();

		StartCoroutine(MainLoop());
		StartCoroutine(MoveLoop());
		StartCoroutine(AwareLoop());
	}

	private IEnumerator MainLoop()
	{
		while (true) {
			yield return StartCoroutine (SeekPlayer());
			yield return StartCoroutine(Idle());
			yield return StartCoroutine(Fish());
		}
	}

	private IEnumerator SeekPlayer()
	{
		float time = 0.0f;
		Debug.Log ("Seek Player");
		while (time < seekPlayerTime) {
			Vector3 moveDir = player.position - myT.position;

			if (moveDir.magnitude < shyDistance) {
				yield return null; 
				yield break;
			}

			moveDir.y = 0;
			moveDir = (moveDir.normalized + (myT.forward * 0.5f)).normalized;
			offsetMoveDirection = GetPathDirection (myT.position, moveDir);

			if (offsetMoveDirection != Vector3.zero) status = HeronStatus.Walking;
			else status = HeronStatus.Idle;

			yield return new WaitForSeconds(hitTestTimeIncrement);
			time += hitTestTimeIncrement;
		}
	}

	private IEnumerator Idle ()
	{
		Debug.Log ("Idle");
		strechNeck = false;
		float time = 0.00f;
		while (time < seekPlayerTime)
		{
			if (time > 0.6) strechNeck = true;

			status = HeronStatus.Idle;
			offsetMoveDirection = Vector3.zero;

			yield return new WaitForSeconds (hitTestTimeIncrement);
			time += hitTestTimeIncrement;
		}
	}

	private IEnumerator Scared()
	{
		Debug.Log ("Scared");
		float dist = (player.position - myT.position).magnitude;
		if (dist > scaredDistance) yield break;

		float time = 0.00f;

		while (time < scaredTime) 
		{
			Vector3 moveDirection = myT.position - player.position;

			if (moveDirection.magnitude > shyDistance * 1.5) {
				yield return null;
				yield break;
			}

			moveDirection.y = 0;
			moveDirection = (moveDirection.normalized + (myT.forward * 0.5f)).normalized;
			offsetMoveDirection = GetPathDirection (myT.position, moveDirection);

			if (offsetMoveDirection != Vector3.zero) status = HeronStatus.Running;
			else status = HeronStatus.Idle;

			yield return new WaitForSeconds (hitTestTimeIncrement);
			time += hitTestTimeIncrement;
		}
	}

	private IEnumerator Fish()
	{
		float height = terrain.GetInterpolatedHeight (myT.position.x / terrain.size.x, myT.position.z / terrain.size.z);
		status = HeronStatus.Walking;
		Vector3 direction;
		Vector3 randomDir = Random.onUnitSphere;

		if (height > 40) {
			maxHeight = 40;
			offsetMoveDirection = GetPathDirection (myT.position, randomDir);
			yield return new WaitForSeconds(0.5f);
			if (velocity.magnitude > 0.01)
				direction = myT.right * (Random.value > 0.5 ? -1 : 1);
			}

		if (height > 38) {
			maxHeight = 38;
			offsetMoveDirection = GetPathDirection (myT.position, randomDir);
			yield return new WaitForSeconds(1f);
			if (velocity.magnitude > 0.01)
				direction = myT.right * (Random.value > 0.5 ? -1 : 1);
		}

		if (height > 36.5) {
			maxHeight = 36.5f;
			offsetMoveDirection = GetPathDirection (myT.position, randomDir);
			yield return new WaitForSeconds (1.5f);
			if (velocity.magnitude > 0.01)
				direction = myT.right * (Random.value > 0.5 ? -1 : 1);
		}
		while (height > 35) {
			maxHeight = 35;
			yield return new WaitForSeconds (0.5f);
			if(velocity.magnitude > 0.01) direction = myT.right * (Random.value > 0.5 ? -1 : 1);
			offsetMoveDirection = GetPathDirection (myT.position, randomDir);
			height = terrain.GetInterpolatedHeight (myT.position.x / terrain.size.x, myT.position.z / terrain.size.z);
		}

		fishing = true;
		status = HeronStatus.Walking;
		yield return new WaitForSeconds (fishingTime / 3);
		status = HeronStatus.Idle;
		yield return new WaitForSeconds (fishingTime / 6);
		status = HeronStatus.Walking;
		yield return new WaitForSeconds (fishingTime / 3);
		status = HeronStatus.Idle;
		yield return new WaitForSeconds (fishingTime / 6);
		fishing = false;

		maxHeight = 42;

	}

	private IEnumerator AwareLoop()
	{
		while (true) 
		{
			float dist = (player.position - myT.position).magnitude;

			if (dist < scaredDistance && status != HeronStatus.Running) {
				StopCoroutine (Fish());
				maxHeight = 42;
				StopCoroutine (Idle());
				strechNeck = false;
				StopCoroutine (SeekPlayer());
				StartCoroutine(Scared ());
			}
			yield return null;
		}
	}

	private IEnumerator MoveLoop ()
	{
		while (true) {
			Debug.Log ("Move");
			float deltaTime = Time.deltaTime;
			float targetSpeed = 0.00f;

			if (status == HeronStatus.Walking && offsetMoveDirection.magnitude > 0.01f) {
				if (!fishing) {
					targetSpeed = walkAnimSpeed * walkSpeed;
					anim.CrossFade ("Walk", 0.4f);
				} else {
					targetSpeed = fishWalkAnimSpeed * fishWalkSpeed;
					anim.CrossFade ("FishingWalk", 0.4f);
				}
			} else if (status == HeronStatus.Running) {
				targetSpeed = runAnimSpeed * runSpeed;
				anim.CrossFade ("Run", 0.4f);
			} else {
				if (!fishing) {
					targetSpeed = 0;
					if (!strechNeck)
						anim.CrossFade ("IdleHold", 0.4f);
					else
						anim.CrossFade ("IdleStrechNeck", 0.4f);
				} else {
					targetSpeed = 0;
					anim.CrossFade ("IdleFishing", 0.4f);
				}
			}

			usedMoveDirection = Vector3.Lerp (usedMoveDirection, offsetMoveDirection, deltaTime * 0.7f);
			velocity = Vector3.RotateTowards (velocity, offsetMoveDirection * targetSpeed, turning * deltaTime, acceleration * deltaTime);
			velocity.y = 0;

			if (velocity.magnitude > 0.01f) {
				if (lastSpeed < 0.01f) {
					velocity = forward * 0.1f;
				} else {
					forward = velocity.normalized;
				}
			}
			transform.position += velocity * deltaTime;
			transform.rotation = Quaternion.LookRotation (forward);
			lastSpeed = velocity.magnitude;
			yield return null;
		}
	}

	

	Vector3 GetPathDirection(Vector3 curPos, Vector3 wantedDirection)
	{
		Vector3 awayFromCollision = TestPosition (curPos);
		if (awayFromCollision != Vector3.zero) {
			//Debug.DrawRay (myT.position, awayFromCollision.normalized * 20, Color.yellow);
			return awayFromCollision.normalized;
		} else {
			//Debug.DrawRay (myT.position, Vector3.up * 5, Color.yellow);
		}

		Vector3 right = Vector3.Cross (wantedDirection, Vector3.up);
		float currentLength = TestDirection (myT.position, wantedDirection);

		if (currentLength > hitTestDistanceMax) {
			return wantedDirection;
		} else {
			float sideAmount = 1 - Mathf.Clamp01 (currentLength / 50);
			Vector3 rightDirection = Vector3.Lerp (wantedDirection, right, sideAmount * sideAmount);
			float rightLength = TestDirection (myT.position, rightDirection);
			Vector3 leftDirection = Vector3.Lerp (wantedDirection, -right, sideAmount * sideAmount);
			float leftLength = TestDirection (myT.position, leftDirection);

			if (rightLength > leftLength && rightLength > currentLength && rightLength > hitTestDistanceIncrement) {
				return rightDirection.normalized;
			}
			if (leftLength > rightLength && leftLength > currentLength && leftLength > hitTestDistanceIncrement) {
				return leftDirection.normalized;
			}
		}
		if (currentLength > hitTestDistanceIncrement) {
			return wantedDirection;
		}

		return Vector3.zero;
	}


	float TestDirection ( Vector3 position, Vector3 direction)
	{
		float length = 0.00f;

		while (true) {
			length += hitTestDistanceIncrement;
			if (length > hitTestDistanceMax)
				return length;
			Vector3 testPos = position + (direction * length);
			float height = terrain.GetInterpolatedHeight (testPos.x / terrain.size.x, testPos.z / terrain.size.z);
			if (height > maxHeight || height < minHeight) {
				break;
			} else {
				bool hit = false;
				int i = 0;

				while (i < colliders.Length) {
					HeronCollider collider = colliders [i];
				
					float x = collider.position.x - testPos.x;
					float z = collider.position.z - testPos.z;

					if (x < 0)
						x = -x;
					if (z < 0)
						z = -z;
					if (z + x < collider.radius) {
						hit = true;
						break;
					}
					i++;
				}
				if (hit)
					break;
			}
		}
		return length;
	}

	Vector3 TestPosition(Vector3 testPos)
	{
		Vector3 moveDir;
		Vector3 heightPos = testPos;
		float height = terrain.GetInterpolatedHeight (testPos.x / terrain.size.x, testPos.z / terrain.size.z);
		if (height > maxHeight || height < minHeight) {
			float heightDiff = 100.00f;
			float optimalHeight = (maxHeight * 0.5f) + (minHeight * 0.5f);

			bool found = false;
			float mult = 1.0f;

			while (!found && mult < 5) {
				float rotation = 0.0f;
				while (rotation < 360) {
					Vector3 forwardDir = Quaternion.Euler (0, rotation, 0) * Vector3.forward;
					Vector3 forwardPos = testPos + (forwardDir * hitTestDistanceIncrement * mult * 3);

					//Debug.DrawRay(forwardPos, Vector3.up, Color(0.9, 0.1, 0.1, 0.7));

					float forwardHeight = terrain.GetInterpolatedHeight (forwardPos.x / terrain.size.x, forwardPos.z / terrain.size.z);
					float diff = Mathf.Abs (forwardHeight - optimalHeight);

					if (forwardHeight < maxHeight && forwardHeight > minHeight && heightDiff > diff) {
						//Debug.DrawRay (forwardPos, Vector3.up, Color.green);
						found = true;
						heightDiff = diff;
						heightPos = forwardPos;
					}
					rotation += 45;
				}
				mult += 0.5f;
			}
		}
		Vector3 move = heightPos - testPos;
		if (move.magnitude > 0.01) {
			//print("height");
			moveDir = move.normalized;
		} else {
			//print("noheight");
			moveDir = Vector3.zero;
		}
		int i = 0;
		while (i < colliders.Length) {
			HeronCollider collider = colliders [i];
			float x = collider.position.x - testPos.x;
			float z = collider.position.z - testPos.z;
			if (x < 0)
				x = -x;
			if (z < 0)
				z = -z;
			if (z + x < collider.radius) {
				moveDir += (testPos - collider.position).normalized;
				break;
			}
			i++;
		}
		return moveDir;
	}

	void LateUpdate()
	{
		float rightHeight = terrain.GetInterpolatedHeight(rightFoot.position.x / terrain.size.x, rightFoot.position.z / terrain.size.z);
		Vector3 rightNormal = terrain.GetInterpolatedNormal(rightFoot.position.x / terrain.size.x, rightFoot.position.z / terrain.size.z);
		float leftHeight = terrain.GetInterpolatedHeight(leftFoot.position.x / terrain.size.x, leftFoot.position.z / terrain.size.z);
		Vector3 leftNormal = terrain.GetInterpolatedNormal(leftFoot.position.x / terrain.size.x, leftFoot.position.z / terrain.size.z);

		if(leftHeight < rightHeight)
		{
			transform.position = new Vector3 (transform.position.x, leftHeight, transform.position.z);
			leftFoot.rotation = Quaternion.LookRotation(leftFoot.forward, leftNormal);
			leftFoot.Rotate(Vector3.right * 15);

			float raise = (rightHeight - leftHeight) * 0.5f;

			rightKnee.position = new Vector3(rightKnee.position.x, rightKnee.position.y + raise, rightKnee.position.z);
			rightAnkle.position = new Vector3(rightAnkle.position.x, rightAnkle.position.y + raise, rightAnkle.position.z);
			rightFoot.rotation = Quaternion.LookRotation(rightNormal, rightFoot.up);
			rightFoot.Rotate(-Vector3.right * 15);
		}
		else
		{
			transform.position = new Vector3 (transform.position.x, rightHeight, transform.position.z);
			rightFoot.rotation = Quaternion.LookRotation(rightNormal, rightFoot.up);
			rightFoot.Rotate(-Vector3.right * 15);

			float raise = (leftHeight - rightHeight) * 0.5f;

			leftKnee.position = new Vector3(leftKnee.position.x, leftKnee.position.y + raise, leftKnee.position.z);
			leftAnkle.position = new Vector3(leftAnkle.position.x, leftAnkle.position.y + raise, leftAnkle.position.z);
			leftFoot.rotation = Quaternion.LookRotation(leftFoot.forward, leftNormal);
			leftFoot.Rotate(Vector3.right * 15);
		}

		transform.position = new Vector3 (transform.position.x, transform.position.y + 0.1f, transform.position.z);
		//Debug.Log ("Player Position = " + player.transform.position);
	}
}
		
