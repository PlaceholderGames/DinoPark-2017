using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Seagull : MonoBehaviour
{
    AudioClip[] sounds = new AudioClip[0];
    float soundFrequency = 1.00f;

    float minSpeed = 0.00f;
    float turnSpeed = 0.00f;
    float randomFreq = 1.0f / 0.0f;

    float randomForce = 0.00f;
    float toOriginForce = 0.00f;
    float toOriginRange = 0.00f;
    float damping = 0.00f;
    float gravity = 0.00f;
    float avoidanceRadius = 0.00f;
    float avoidanceForce = 0.00f;
    float followVelocity = 0.00f;
    float followRadius = 0.00f;
    float bankTurn = 0.00f;
    bool raycast = false;
    float bounce = 0.80f;

    private SeagullFlightPath target;
    private Transform origin;
    private Vector3 velocity;
    private Vector3 normalizedVelocity;
    private Vector3 randomPush;
    private Vector3 originPush;
    private Vector3 gravPush;
    private RaycastHit hit;
    private Transform[] objects;
    private Seagull[] otherSeagulls;
    private Animation animationComponent;
    private Transform transformComponent;
    private bool gliding = false;
    private float bank = 0.00f;
    private AnimationState glide;

    void Start()
    {
        gameObject.tag = transform.parent.gameObject.tag;

        animationComponent = GetComponentInChildren<Animation>();
        animationComponent.Blend("fly");
        animationComponent["fly"].normalizedTime = Random.value;
        glide = animationComponent["glide"];

        origin = transform.parent;
        target = origin.GetComponent<SeagullFlightPath>();
        transform.parent = null;
        transformComponent = transform;

        Seagull[] tempSeagulls = new Seagull[0];
        if (transform.parent)
            tempSeagulls = transform.parent.GetComponentsInChildren<Seagull>();
        objects = new Transform[tempSeagulls.Length];
        otherSeagulls = new Seagull[tempSeagulls.Length];
        for (int i = 0; i < tempSeagulls.Length; i++)
        {
            objects[i] = tempSeagulls[i].transform;
            otherSeagulls[i] = tempSeagulls[i];
        }

        UpdateRandom();
    }

    void UpdateRandom()
    {
        while (true)
        {
            randomPush = Random.insideUnitSphere * randomForce;
            float wait = randomFreq + Random.Range(-randomFreq / 2, randomFreq / 2) ;
            System.Threading.Thread.Sleep((int)wait);
        }
    }

    void Update()
    {
        float speed = velocity.magnitude;
        Vector3 avoidPush = Vector3.zero;
        Vector3 avgPoint = Vector3.zero;
        Vector3 forceV;
        Vector3 toAvg;
        Vector3 diff;
        float d;
        int count = 0;
        float f = 0.0f;
        Vector3 myPosition = transformComponent.position;

        for (int i = 0; i < objects.Length; i++)
        {
            Transform o = objects[i];
            if (o != transformComponent)
            {
                Vector3 otherPosition = o.position;
                avgPoint += otherPosition;
                count++;

                forceV = myPosition - otherPosition;
                d = forceV.magnitude;
                if (d < followRadius)
                {
                    if (d < avoidanceRadius)
                    {
                        f = 1.0f - (d / avoidanceRadius);
                        if (d > 0) avoidPush += (forceV / d) * f * avoidanceForce;
                    }

                    f = d / followRadius;
                    Seagull otherSealgull = otherSeagulls[i];
                    avoidPush += otherSealgull.normalizedVelocity * f * followVelocity;
                }
            }
        }

        if (count > 0)
        {
            avoidPush /= count;
            toAvg = (avgPoint / count) - myPosition;
        }
        else
        {
            toAvg = Vector3.zero;
        }

        forceV = origin.position + target.offset - myPosition;
        d = forceV.magnitude;
        f = d / toOriginRange;
        if (d > 0) originPush = (forceV / d) * f * toOriginForce;

        if (speed < minSpeed && speed > 0)
        {
            velocity = (velocity / speed) * minSpeed;
        }

        Vector3 wantedVel;

        wantedVel = velocity;
        wantedVel -= wantedVel * damping * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avoidPush * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;
        diff = transformComponent.InverseTransformDirection(wantedVel - velocity).normalized;
        bank = Mathf.Lerp(bank, diff.x, Time.deltaTime * 0.8f);
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.00f);

        transformComponent.rotation = Quaternion.LookRotation(velocity);
        transformComponent.Rotate(0, 0, -bank * bankTurn);

        // Raycast
        float distance = speed * Time.deltaTime;
        if (raycast && distance > 0.00f && Physics.Raycast(myPosition, velocity, 0.0f, (int)distance)) // fix
        {
            velocity = Vector3.Reflect(velocity, hit.normal) * bounce;
        }
        else
        {
            transformComponent.Translate(velocity * Time.deltaTime, Space.World);
        }

        // Animation Controls
        if (speed > 0)
        {
            float up = (velocity / speed).y;
            if (gliding && up > 0)
            {
                gliding = false;
                animationComponent.Blend("glide", 0.00f, 0.2f);
                animationComponent.Blend("fly", 1.00f, 0.2f);
            }
            if (!gliding && up < -0.20f)
            {
                gliding = true;
                animationComponent.Blend("glide", 1.00f, 0.2f);
                animationComponent.Blend("fly", 0.00f, 0.2f);
                glide.speed = 0;
            }
        }

        // Sounds
        if (SeagullSoundHeat.heat < Mathf.Pow(Random.value, 1 / soundFrequency / Time.deltaTime))
        {
            AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.Length)], myPosition, 0.90f);
            SeagullSoundHeat.heat += (1 / soundFrequency) / 10;
        }

        normalizedVelocity = velocity.normalized;
    }
}
