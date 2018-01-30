using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullFlightPath : MonoBehaviour
{
    float flySpeed = 15.00f;
    float highFlyHeight = 80.00f;
    float normalFlyHeight = 40.00f;
    float lowFlyHeight = 20.00f;
    float flyDownSpeed = 0.10f;
    float circleRadius = 60.00f;
    float circleSpeed = 0.20f;
    float circleTime = 15.00f;
    float awayTime = 20.00f;

    Vector3 offset;

    private Transform myT;
    private Transform player;
    private Vector3 awayDir;
    private float flyHeight = 0.00f;
    private Collider col;
    private RaycastHit hit;
    private float distToTarget = 0.00f;
    private float lastHeight = 0.00f;
    private float height = 0.00f;
    private Vector3 terrainSize;
    private TerrainData terrainData;

    private float dTime = 0.1f;

    void Start()
    {
        terrainData = Terrain.activeTerrain.terrainData;
        terrainSize = terrainData.size;
        col = Terrain.activeTerrain.GetComponent<Collider>();
        myT = transform;
        player = GameObject.FindWithTag("Player").transform;
        MainRoutine();
    }

    void MainRoutine()
    {
        while (true)
        {
            //had yield
            ReturnToPlayer();
            CirclePlayer();
            FlyAway();
        }
    }

    void ReturnToPlayer()
    {
        Vector3 targetPos;

        distToTarget = 100.00f;
        while (distToTarget > 10)
        {
            Vector3 toPlayer = player.position - myT.position;
            toPlayer.y = 0;
            distToTarget = toPlayer.magnitude;
            if (distToTarget > 0)
                targetPos = transform.position + ((toPlayer / distToTarget) * 10);
            else targetPos = Vector3.zero;

            targetPos.y = terrainData.GetInterpolatedHeight(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            Vector3 normal = terrainData.GetInterpolatedNormal(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            offset = new Vector3(normal.x * 40, 0, normal.z * 40);

            flyHeight = (distToTarget > 80) ? highFlyHeight : lowFlyHeight;
            if (distToTarget > 0) Move(targetPos - transform.position);
            System.Threading.Thread.Sleep((int)dTime);
        }
    }

    void CirclePlayer()
    {
        float time = 0.00f;
        while (time < circleTime)
        {
            Vector3 circlingPos = player.position + new Vector3(Mathf.Cos(Time.time * circleSpeed) * circleRadius, 0, Mathf.Sin(Time.time * circleSpeed) * circleRadius);
            circlingPos.y = terrainData.GetInterpolatedHeight(circlingPos.x / terrainSize.x, circlingPos.z / terrainSize.z);
            Vector3 normal = terrainData.GetInterpolatedNormal(circlingPos.x / terrainSize.x, circlingPos.z / terrainSize.z);
            offset = new Vector3(normal.x * 40, 0, normal.z * 40);

            flyHeight = normalFlyHeight;
            Move(circlingPos - myT.position);
            time += dTime;
            System.Threading.Thread.Sleep((int)dTime);
        }
    }

    void FlyAway()
    {
        Vector3 targetPos;
        float radians = Random.value * 2 * Mathf.PI;
        Vector3 awayDir = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        float time = 0.00f;
        while (time < awayTime)
        {
            Vector3 away = player.position + (awayDir * 1000);
            away.y = 0;

            Vector3 toAway = away - transform.position;

            float distToTarget = toAway.magnitude;
            if (distToTarget > 0) targetPos = transform.position + ((toAway / distToTarget) * 10);
            else targetPos = Vector3.zero;

            targetPos.y = terrainData.GetInterpolatedHeight(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            Vector3 normal = terrainData.GetInterpolatedNormal(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            offset = new Vector3(normal.x * 40, 0, normal.z * 40);

            flyHeight = highFlyHeight;
            Move(targetPos - transform.position);
            time += dTime;
            System.Threading.Thread.Sleep((int)dTime);
        }
    }

    void Move(Vector3 delta)
    {
        float newHeight;

        delta.y = 0;
        delta = delta.normalized * flySpeed * dTime;
        Vector3 newPos = new Vector3(myT.position.x + delta.x, 1000, myT.position.z + delta.z);
        if (col.Raycast(Ray(newPos, -Vector3.up), hit, 2000))
        {
            newHeight = hit.point.y;
        }
        else
        {
            newHeight = 0.00f;
        }

        if (newHeight < lastHeight)
        {
            height = Mathf.Lerp(height, newHeight, flyDownSpeed * dTime);
        }
        else
        {
            height = newHeight;
        }

        lastHeight = newHeight;
        myT.position = new Vector3(newPos.x, Mathf.Clamp(height, 35.28f, 1000.00f) + flyHeight, newPos.z);
    }
}