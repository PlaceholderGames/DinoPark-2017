using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heron : MonoBehaviour {

	float acceleration = 5.00f;
	float turning = 3.00f;

	float maxIdleTime = 4.00f;
	float seekPlayerTime = 6.00f;
	float scaredTime = 4.00f;
	float fishingTime = 30.00f;

	float shyDistance = 10.00f;
	float scaredDistance = 5.00f;

	float strechNeckProbability = 10.00f;

	float fishWalkSpeed = 1.00f;
	float walkSpeed = 1.00f;
	float runSpeed = 1.00f;

	private HeronStatus status = HeronStatus.Idle;

	private float fishWalkAnimSpeed = 0.50f;
	private float walkAnimSpeed = 2.00f;
	private float runAnimSpeed = 9.00f;

	private float minHeight = 34.1f;
	private float maxHeight = 42.00f;
	private HeronCollider[] colliders; //needs array size, how big?

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
	private float lastSpeed = 0.00f;

	enum HeronStatus {Idle = 0, Walking = 1, Running = 2}

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

	void Start()
	{
		forward = transform.forward;

		GameObject obj = GameObject.FindWithTag("Player");
		player = obj.transform;
		myT = transform;
		Terrain terr = Terrain.activeTerrain;
		if(terr)
			terrain = terr.terrainData;

		anim = GetComponentInChildren<Animation>();
		anim["Walk"].speed = walkSpeed;
		anim["Run"].speed = runSpeed;
		anim["FishingWalk"].speed = fishWalkSpeed;

		leftKnee = myT.Find("HeronAnimated/MasterMover/RootDummy/Root/Lhip/knee2");
		leftAnkle = leftKnee.Find("ankle2");
		leftFoot = leftAnkle.Find("foot2");
		rightKnee = myT.Find("HeronAnimated/MasterMover/RootDummy/Root/Rhip/knee3");
		rightAnkle = rightKnee.Find("ankle3");
		rightFoot = rightAnkle.Find("foot3");

		colliders =  FindObjectsOfType(typeof(HeronCollider)) as HeronCollider[];

		MainLoop();
		MoveLoop();
		AwareLoop();
	}

	void MainLoop()
	{
		while(true)
		{
            //did have yield
			SeekPlayer();
			Idle();
			Fish();	
		}
	}

	void SeekPlayer()
	{
		var time = 0.00;
		while(time < seekPlayerTime)
		{
			var moveDirection = player.position - myT.position;

			if(moveDirection.magnitude < shyDistance)
			{
                yield;
			}

			moveDirection.y = 0;
			moveDirection = (moveDirection.normalized + (myT.forward * 0.5f)).normalized;
			offsetMoveDirection = GetPathDirection(myT.position, moveDirection);

			if(offsetMoveDirection != Vector3.zero) status = HeronStatus.Walking;
			else status = HeronStatus.Idle;

			wait(hitTestTimeIncrement);
			time +=	hitTestTimeIncrement;
		}
	}

	void Idle ()
	{
		strechNeck = false;
		var time = 0.00;
		while(time < seekPlayerTime)
		{	
			if(time > 0.6) strechNeck = true;

			status = HeronStatus.Idle;
			offsetMoveDirection = Vector3.zero;

            wait(hitTestTimeIncrement);
			time +=	hitTestTimeIncrement;
		}
	}

	void Scared ()
	{
		var dist = (player.position - myT.position).magnitude;
		if(dist > scaredDistance) return;

		var time = 0.00;

		while(time < scaredTime)
		{
			var moveDirection = myT.position - player.position;

			if(moveDirection.magnitude > shyDistance * 1.5)
			{
				yield;
				return;
			}

			moveDirection.y = 0;
			moveDirection = (moveDirection.normalized + (myT.forward * 0.5f)).normalized;
			offsetMoveDirection = GetPathDirection(myT.position, moveDirection);

			if(offsetMoveDirection != Vector3.zero) status = HeronStatus.Running;
			else status = HeronStatus.Idle;

			wait(hitTestTimeIncrement);
			time +=	hitTestTimeIncrement;
		}
	}

	void Fish()
	{
		float height = terrain.GetInterpolatedHeight(myT.position.x / terrain.size.x, myT.position.z / terrain.size.z);
		status = HeronStatus.Walking;
		Vector3 direction;
		Vector3 randomDir = Random.onUnitSphere;

		if(height > 40)
		{
			maxHeight = 40;
			offsetMoveDirection = GetPathDirection(myT.position, randomDir);
			wait(0.5f);
			if(velocity.magnitude > 0.01) direction = myT.right * (Random.value > 0.5 ? -1 : 1);
		}
		if(height > 38)
		{
			maxHeight = 38;
			offsetMoveDirection = GetPathDirection(myT.position, randomDir);
			wait(1f);
			if(velocity.magnitude > 0.01) direction = myT.right * (Random.value > 0.5 ? -1 : 1);
		}
		if(height > 36.5)
		{
			maxHeight = 36.5f;
			offsetMoveDirection = GetPathDirection(myT.position, randomDir);
			wait(1.5f);
			if(velocity.magnitude > 0.01) direction = myT.right * (Random.value > 0.5 ? -1 : 1);
		}
		while(height > 35)
		{
			maxHeight = 35;
			wait(0.5f);
			if(velocity.magnitude > 0.01) direction = myT.right * (Random.value > 0.5 ? -1 : 1);
			offsetMoveDirection = GetPathDirection(myT.position, randomDir);
			height = terrain.GetInterpolatedHeight(myT.position.x / terrain.size.x, myT.position.z / terrain.size.z);
		}

		fishing = true;
		status = HeronStatus.Walking;
		wait(fishingTime / 3);
		status = HeronStatus.Idle;
        wait(fishingTime / 6);
		status = HeronStatus.Walking;
        wait(fishingTime / 3);
		status = HeronStatus.Idle;
        wait(fishingTime / 6);
		fishing = false;

		maxHeight = 42;
	}

	void AwareLoop ()
	{
		while(true)
		{
			float dist = (player.position - myT.position).magnitude;

			if(dist < scaredDistance && status != HeronStatus.Running)
			{
				StopCoroutine("Fish");
				maxHeight = 42;
				StopCoroutine("Idle");
				strechNeck = false;
				StopCoroutine("SeekPlayer");
				Scared();
			}
			yield;	
		}	
	}

	void MoveLoop ()
	{
		while(true)
		{
			var deltaTime = Time.deltaTime;
			var targetSpeed = 0.00f;
			if(status == HeronStatus.Walking && offsetMoveDirection.magnitude > 0.01)
			{
				if(!fishing)
				{
					targetSpeed = walkAnimSpeed * walkSpeed;
					anim.CrossFade("Walk", 0.4f);
				}
				else
				{
					targetSpeed = fishWalkAnimSpeed * fishWalkSpeed;
					anim.CrossFade("FishingWalk", 0.4f);
				}
			}
			else if(status == HeronStatus.Running)
			{
				targetSpeed = runAnimSpeed * runSpeed;
				anim.CrossFade("Run", 0.4f);
			}
			else
			{
				if(!fishing)
				{
					targetSpeed = 0;
					if(!strechNeck) anim.CrossFade("IdleHold", 0.4f);
					else anim.CrossFade("IdleStrechNeck", 0.4f);
				}
				else
				{
					targetSpeed = 0;
					anim.CrossFade("IdleFishing", 0.4f);
				}
			}

			usedMoveDirection = Vector3.Lerp(usedMoveDirection, offsetMoveDirection, deltaTime * 0.7f);
			velocity = Vector3.RotateTowards(velocity, offsetMoveDirection * targetSpeed, turning * deltaTime, acceleration * deltaTime);
			velocity.y = 0;

			if(velocity.magnitude > 0.01)
			{
				if(lastSpeed < 0.01)
				{
					velocity = forward * 0.1f;
				}
				else
				{
					forward = velocity.normalized;
				}
			}
			transform.position += velocity * deltaTime;
			transform.rotation = Quaternion.LookRotation(forward);
			lastSpeed = velocity.magnitude;
			yield;	
		}	
	}

	Vector3 GetPathDirection (Vector3 curPos, Vector3 wantedDirection)
	{
		Vector3 awayFromCollision = TestPosition(curPos);
		if(awayFromCollision != Vector3.zero)
		{
			//Debug.DrawRay(myT.position, awayFromCollision.normalized * 20, Color.yellow);
			return awayFromCollision.normalized;
		}
		else
		{
			///Debug.DrawRay(myT.position, Vector3.up * 5, Color.yellow);
		}

		Vector3 right = Vector3.Cross(wantedDirection, Vector3.up);
		float currentLength = TestDirection(myT.position, wantedDirection);
		if(currentLength > hitTestDistanceMax)
		{
			return wantedDirection;
		}
		else
		{
			float sideAmount = 1 - Mathf.Clamp01(currentLength / 50);
            Vector3 rightDirection = Vector3.Lerp(wantedDirection, right, sideAmount * sideAmount);
			float rightLength = TestDirection(myT.position, rightDirection);
            Vector3 leftDirection = Vector3.Lerp(wantedDirection, -right, sideAmount * sideAmount);
            float leftLength = TestDirection(myT.position, leftDirection);

			if(rightLength > leftLength && rightLength > currentLength && rightLength > hitTestDistanceIncrement)
			{
				return rightDirection.normalized;
			}

			if(leftLength > rightLength && leftLength > currentLength && leftLength > hitTestDistanceIncrement)
			{
				return leftDirection.normalized;
			}
		}

		if(currentLength > hitTestDistanceIncrement)
		{
			return wantedDirection;
		}

		return Vector3.zero;
	}

	float TestDirection (Vector3 position, Vector3 direction)
	{
		float length = 0.00f;
		while(true)
		{
			length += hitTestDistanceIncrement;
			if(length > hitTestDistanceMax) return length;
            Vector3 testPos = position + (direction * length);
            float height = terrain.GetInterpolatedHeight(testPos.x / terrain.size.x, testPos.z / terrain.size.z);
			if(height > maxHeight || height < minHeight)
			{
				break;
			}
			else
			{
				var hit = false;
				var i = 0;
				while(i < colliders.length)
				{
                    HeronCollider collider = colliders[i];
					float x = collider.position.x - testPos.x;
					float z = collider.position.z - testPos.z;
					if(x < 0) x = -x;
					if(z < 0) z = -z;
					if(z + x < collider.radius)
					{
						hit = true;
						break;
					}
					i++;	
				}

				if(hit) break;
			}
		}
		return length;
	}

	Vector3 TestPosition (Vector3 testPos) //: Vector3
	{
        Vector3 moveDir;
        Vector3 forwardDir;
        Vector3 forwardPos;
        Vector3 move;
        float forwardHeight;
        float diff;
        Vector3 heightPos = testPos;
        float height = terrain.GetInterpolatedHeight(testPos.x / terrain.size.x, testPos.z / terrain.size.z);
		if(height > maxHeight || height < minHeight)
		{
			var heightDiff = 100.00f;
			var optimalHeight = (maxHeight * 0.5f) + (minHeight * 0.5f);

			var found = false;
			var mult = 1.00f;
			while(!found && mult < 5)
			{
				float rotation = 0.00f;
				while(rotation < 360)
				{
					forwardDir = Quaternion.Euler(0, rotation, 0) * Vector3.forward;
					forwardPos = testPos + (forwardDir * hitTestDistanceIncrement * mult * 3);

					//Debug.DrawRay(forwardPos, Vector3.up, Color(0.9, 0.1, 0.1, 0.7));

					forwardHeight = terrain.GetInterpolatedHeight(forwardPos.x / terrain.size.x, forwardPos.z / terrain.size.z);
					diff = Mathf.Abs(forwardHeight - optimalHeight);
					if(forwardHeight < maxHeight && forwardHeight > minHeight && heightDiff > diff)
					{
						//Debug.DrawRay(forwardPos, Vector3.up, Color.green);
						found = true;
						heightDiff = diff;
                        heightPos = forwardPos;
					}
					rotation += 45;
				}
				mult += 0.5f;
			}
		}

		move = heightPos - testPos;
		if(move.magnitude > 0.01)
		{
			//print("height");
			moveDir = move.normalized;
		}
		else
		{
			//print("noheight");
			moveDir = Vector3.zero;
		}

		var i = 0;
		while(i < colliders.length)
		{
			HeronCollider collider = colliders[i];
			float x = collider.position.x - testPos.x;
			float z = collider.position.z - testPos.z;
			if(x < 0) x = -x;
			if(z < 0) z = -z;
			if(z + x < collider.radius)
			{
				moveDir += (testPos - collider.position).normalized;
				break;
			}
			i++;	
		}

		return moveDir;
	}

	void LateUpdate () // leg IK
	{
		float rightHeight = terrain.GetInterpolatedHeight(rightFoot.position.x / terrain.size.x, rightFoot.position.z / terrain.size.z);
        Vector3 rightNormal = terrain.GetInterpolatedNormal(rightFoot.position.x / terrain.size.x, rightFoot.position.z / terrain.size.z);
        float leftHeight = terrain.GetInterpolatedHeight(leftFoot.position.x / terrain.size.x, leftFoot.position.z / terrain.size.z);
        Vector3 leftNormal = terrain.GetInterpolatedNormal(leftFoot.position.x / terrain.size.x, leftFoot.position.z / terrain.size.z);

		if(leftHeight < rightHeight)
		{
			transform.position = new Vector3(transform.position.x, leftHeight, transform.position.z);
			leftFoot.rotation = Quaternion.LookRotation(leftFoot.forward, leftNormal);
			leftFoot.Rotate(Vector3.right * 15);

			float raise = (rightHeight - leftHeight) * 0.5f;

			rightKnee.position = rightKnee.position + new Vector3(0.0f, raise, 0.0f);
			rightAnkle.position = rightAnkle.position + new Vector3(0.0f, raise, 0.0f);
			rightFoot.rotation = Quaternion.LookRotation(rightNormal, rightFoot.up);
			rightFoot.Rotate(-Vector3.right * 15);
		}
		else
		{
			transform.position = new Vector3(rightKnee.position.x, rightHeight, rightKnee.position.z);
			rightFoot.rotation = Quaternion.LookRotation(rightNormal, rightFoot.up);
			rightFoot.Rotate(-Vector3.right * 15);

            float raise = (leftHeight - rightHeight) * 0.5f;

            leftKnee.position = leftKnee.position + new Vector3(0.0f, raise, 0.0f);
            leftAnkle.position = leftAnkle.position + new Vector3(0.0f, raise, 0.0f);
            leftFoot.rotation = Quaternion.LookRotation(leftFoot.forward, leftNormal);
			leftFoot.Rotate(Vector3.right * 15);
		}

        transform.position = transform.position + new Vector3(0.0f, 0.1f, 0.0f);
	}
}
