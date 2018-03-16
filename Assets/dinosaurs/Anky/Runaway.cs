using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runaway : MonoBehaviour {

     public float fleeDistance = 5.0f;
    public float fleeTime = 4.0f;
    public float shyDistance = 4.0f;
    public float hitTestTimeIncrement = 0.2f;
    //private
    private Transform player = null;
    private Transform ankyT = null;
    private AnkyCollider[] colliders;
    private Vector3 offsetMoveDirection;
    private float hitTestDistanceIncrement = 1.0f;
    private float hitTestDistanceMax = 50.0f;
    private float minHeight = 34.1f;
    private float maxHeight = 42.0f;
    private TerrainData terrain;
    private AnkyStatus status = AnkyStatus.Idle;

    enum AnkyStatus { Idle = 0, Running = 1};

    private void Start()
    {
        // init player for location 
        GameObject obj = GameObject.FindWithTag("Player");
        player = obj.transform;
        ankyT = transform;
        Terrain terr = Terrain.activeTerrain;

        if (terr)
            terrain = terr.terrainData;

        colliders = FindObjectsOfType<AnkyCollider>();
    }

    private void Update()
    {
        StartCoroutine(flee());
    }

    private IEnumerator flee()
    {
        Debug.Log("Scared");
        float dist = (player.position - ankyT.position).magnitude;
        if (dist > fleeDistance) yield break;

        float time = 0.00f;

        while (time < fleeTime)
        {
            Vector3 moveDirection = ankyT.position - player.position;

            if (moveDirection.magnitude > shyDistance * 1.5)
            {
                yield return null;
                yield break;
            }

            moveDirection.y = 0;
            moveDirection = (moveDirection.normalized + (ankyT.forward * 0.5f)).normalized;
            offsetMoveDirection = GetPathDirection(ankyT.position, moveDirection);

            if (offsetMoveDirection != Vector3.zero) status = AnkyStatus.Running;
            else status = AnkyStatus.Idle;

            yield return new WaitForSeconds(hitTestTimeIncrement);
            time += hitTestTimeIncrement;
        }
    }

    Vector3 GetPathDirection(Vector3 pos, Vector3 moveDir)
    {
        Vector3 awayFromCollision = TestPosition(pos);
        if (awayFromCollision != Vector3.zero)
        {
            //Debug.DrawRay (myT.position, awayFromCollision.normalized * 20, Color.yellow);
            return awayFromCollision.normalized;
        }
        else
        {
            //Debug.DrawRay (myT.position, Vector3.up * 5, Color.yellow);
        }

        Vector3 right = Vector3.Cross(moveDir, Vector3.up);
        float currentLength = TestDirection(ankyT.position, moveDir);

        if (currentLength > hitTestDistanceMax)
        {
            return moveDir;
        }
        else
        {
            float sideAmount = 1 - Mathf.Clamp01(currentLength / 50);
            Vector3 rightDirection = Vector3.Lerp(moveDir, right, sideAmount * sideAmount);
            float rightLength = TestDirection(ankyT.position, rightDirection);
            Vector3 leftDirection = Vector3.Lerp(moveDir, -right, sideAmount * sideAmount);
            float leftLength = TestDirection(ankyT.position, leftDirection);

            if (rightLength > leftLength && rightLength > currentLength && rightLength > hitTestDistanceIncrement)
            {
                return rightDirection.normalized;
            }
            if (leftLength > rightLength && leftLength > currentLength && leftLength > hitTestDistanceIncrement)
            {
                return leftDirection.normalized;
            }
        }
        if (currentLength > hitTestDistanceIncrement)
        {
            return moveDir;
        }

        return Vector3.zero;
    }

    float TestDirection(Vector3 position, Vector3 direction)
    {
        float length = 0.00f;

        while (true)
        {
            length += hitTestDistanceIncrement;
            if (length > hitTestDistanceMax)
                return length;
            Vector3 testPos = position + (direction * length);
            float height = terrain.GetInterpolatedHeight(testPos.x / terrain.size.x, testPos.z / terrain.size.z);
            if (height > maxHeight || height < minHeight)
            {
                break;
            }
            else
            {
                bool hit = false;
                int i = 0;

                while (i < colliders.Length)
                {
                    AnkyCollider collider = colliders[i];

                    float x = collider.position.x - testPos.x;
                    float z = collider.position.z - testPos.z;

                    if (x < 0)
                        x = -x;
                    if (z < 0)
                        z = -z;
                    if (z + x < collider.radius)
                    {
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
        float height = terrain.GetInterpolatedHeight(testPos.x / terrain.size.x, testPos.z / terrain.size.z);
        if (height > maxHeight || height < minHeight)
        {
            float heightDiff = 100.00f;
            float optimalHeight = (maxHeight * 0.5f) + (minHeight * 0.5f);

            bool found = false;
            float mult = 1.0f;

            while (!found && mult < 5)
            {
                float rotation = 0.0f;
                while (rotation < 360)
                {
                    Vector3 forwardDir = Quaternion.Euler(0, rotation, 0) * Vector3.forward;
                    Vector3 forwardPos = testPos + (forwardDir * hitTestDistanceIncrement * mult * 3);

                    //Debug.DrawRay(forwardPos, Vector3.up, Color(0.9, 0.1, 0.1, 0.7));

                    float forwardHeight = terrain.GetInterpolatedHeight(forwardPos.x / terrain.size.x, forwardPos.z / terrain.size.z);
                    float diff = Mathf.Abs(forwardHeight - optimalHeight);

                    if (forwardHeight < maxHeight && forwardHeight > minHeight && heightDiff > diff)
                    {
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
        if (move.magnitude > 0.01)
        {
            //print("height");
            moveDir = move.normalized;
        }
        else
        {
            //print("noheight");
            moveDir = Vector3.zero;
        }
        int i = 0;
        while (i < colliders.Length)
        {
            AnkyCollider collider = colliders[i];
            float x = collider.position.x - testPos.x;
            float z = collider.position.z - testPos.z;
            if (x < 0)
                x = -x;
            if (z < 0)
                z = -z;
            if (z + x < collider.radius)
            {
                moveDir += (testPos - collider.position).normalized;
                break;
            }
            i++;
        }
        return moveDir;
    }
}
