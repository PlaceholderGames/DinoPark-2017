using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullFlightPath : MonoBehaviour {

    public float flySpeed = 15.0f;
    public float highFlyHeight = 80.0f;
    public float normalFlyHeight = 40.0f;
    public float lowFlyHeight = 20.0f;
    public float flyDownSpeed = 0.1f;
    public float circleRadius = 60.0f;
    public float circleSpeed = 0.2f;
    public float circleTime = 15.0f;
    public float awayTime = 20.0f;
    Vector3 normal;
    Vector3 targetPos;


    public Vector3 offset;

    private Transform myT;
    private Transform player;
    private Vector3 awayDir;
    private float flyHeight = 0.0f;
    private Collider col;
    private RaycastHit hit;
    private float distToTarget = 0.0f;
    private float lastHeight = 0.0f;
    private float height = 0.0f;
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
        Update();
    }

    void Update()
    {
        while (true)
        {
            ReturnToPlayer();
            CirclePlayer();
            FlyAway();
        }
    }

    void ReturnToPlayer()
    {
        distToTarget = 100.00f;
        while (distToTarget > 10)
        {
            Vector3 toPlayer = player.position - myT.position;
            toPlayer.y = 0;
            distToTarget = toPlayer.magnitude;

            if (distToTarget > 0) targetPos = transform.position + ((toPlayer / distToTarget) * 10);
            else targetPos = Vector3.zero;

            targetPos.y = terrainData.GetInterpolatedHeight(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            normal = terrainData.GetInterpolatedNormal(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            offset = new Vector3(normal.x * 40, 0, normal.z * 40);

            flyHeight = (distToTarget > 80) ? highFlyHeight : lowFlyHeight;
            if (distToTarget > 0) Move(targetPos - transform.position);
            new WaitForSeconds(dTime);
        }
    }

    void CirclePlayer()
    {
        var time = 0.00;
        while (time < circleTime)
        {
            Vector3 circlingPos = player.position + new Vector3(Mathf.Cos(Time.time * circleSpeed) * circleRadius, 0, Mathf.Sin(Time.time * circleSpeed) * circleRadius);
            circlingPos.y = terrainData.GetInterpolatedHeight(circlingPos.x / terrainSize.x, circlingPos.z / terrainSize.z);
            normal = terrainData.GetInterpolatedNormal(circlingPos.x / terrainSize.x, circlingPos.z / terrainSize.z);
            offset = new Vector3(normal.x * 40, 0, normal.z * 40);

            flyHeight = normalFlyHeight;
            Move(circlingPos - myT.position);
            time += dTime;
            new WaitForSeconds(dTime);
        }
    }

    void FlyAway()
    {
        float radians = Random.value * 2 * Mathf.PI;
        awayDir = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
        var time = 0.00;
        while (time < awayTime)
        {
            Vector3 away = player.position + (awayDir * 1000);
            away.y = 0;

            Vector3 toAway = away - transform.position;

            distToTarget = toAway.magnitude;
            if (distToTarget > 0) targetPos = transform.position + ((toAway / distToTarget) * 10);
            else targetPos = Vector3.zero;

            targetPos.y = terrainData.GetInterpolatedHeight(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            normal = terrainData.GetInterpolatedNormal(targetPos.x / terrainSize.x, targetPos.z / terrainSize.z);
            offset = new Vector3(normal.x * 40, 0, normal.z * 40);

            flyHeight = highFlyHeight;
            Move(targetPos - transform.position);
            time += dTime;
            new WaitForSeconds(dTime);
        }
    }

    void Move(Vector3 delta)
    {
        float newHeight;
        delta.y = 0;
        delta = delta.normalized * flySpeed * dTime;
        Vector3 newPos = new Vector3(myT.position.x + delta.x, 1000, myT.position.z + delta.z);
        if (col.Raycast(new Ray(newPos, -Vector3.up), out hit, 2000)) newHeight = hit.point.y;
        else newHeight = 0.00f;
        if (newHeight < lastHeight) height = Mathf.Lerp(height, newHeight, flyDownSpeed * dTime);
        else height = newHeight;
        lastHeight = newHeight;
        myT.position = new Vector3(newPos.x, Mathf.Clamp(height, 35.28f, 1000.00f) + flyHeight, newPos.z);
    }
}
