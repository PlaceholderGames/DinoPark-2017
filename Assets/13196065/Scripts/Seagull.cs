using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour {

    public AudioClip[] sounds = new AudioClip[0];
    public float soundFrequency = 0.2f;

    public float minSpeed = 10f;
    public float turnSpeed = 2.0f;
    public float randomFreq = 2.0f;
    public float randomForce = 5.0f;
    public float toOriginForce = 20.0f;
    public float toOriginRange = 100.0f;
    public float damping = 1.0f;
    public float gravity = 2.0f;
    public float avoidanceRadius = 20.0f;
    public float avoidanceForce = 20.0f;
    public float followVelocity = 4.0f;
    public float followRadius = 40.0f;
    public float bankTurn = 45.0f;
    public bool raycast = false;
    public float bounce = 0.8f;

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
    private float bank = 0.0f;
    private AnimationState glide;

    void Start()
    {
        randomFreq = (float)(1.0 / randomFreq); // undefined number, a hack to get a truly random number
        Debug.Log("Random Freq: " + randomFreq);

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
        objects = new Transform[tempSeagulls.GetLength(0)];
        otherSeagulls = new Seagull[tempSeagulls.GetLength(0)];
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
            new WaitForSeconds(randomFreq + Random.Range(-randomFreq / 2, randomFreq / 2));
        }
    }

    void Update()
    {
        float speed = velocity.magnitude;
        Vector3 avoidPush = Vector3.zero;
        Vector3 avgPoint = Vector3.zero;
        int count = 0;
        float f = 0.0f;
        Vector3 myPosition = transformComponent.position;
        Vector3 forceV;
        float d;

        for (int i = 0; i < objects.Length; i++)
        {
            Transform o = objects[i];
            if (o != transformComponent)
            {
                var otherPosition = o.position;
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

        Vector3 toAvg;

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

        Vector3 wantedVel = velocity;
        wantedVel -= wantedVel * damping * Time.deltaTime;
        wantedVel += randomPush * Time.deltaTime;
        wantedVel += originPush * Time.deltaTime;
        wantedVel += avoidPush * Time.deltaTime;
        wantedVel += toAvg.normalized * gravity * Time.deltaTime;
        Vector3 diff = transformComponent.InverseTransformDirection(wantedVel - velocity).normalized;
        bank = Mathf.Lerp(bank, diff.x, Time.deltaTime * 0.8f);
        velocity = Vector3.RotateTowards(velocity, wantedVel, turnSpeed * Time.deltaTime, 100.0f);

        transformComponent.rotation = Quaternion.LookRotation(velocity);
        transformComponent.Rotate(0, 0, -bank * bankTurn);

        // Raycast
        float distance = speed * Time.deltaTime;
        if (raycast && distance > 0.00 && Physics.Raycast(myPosition, velocity, out hit, distance))
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
                animationComponent.Blend("glide", 0.0f, 0.2f);
                animationComponent.Blend("fly", 1.0f, 0.2f);
            }
            if (!gliding && up < -0.20)
            {
                gliding = true;
                animationComponent.Blend("glide", 1.0f, 0.2f);
                animationComponent.Blend("fly", 0.0f, 0.2f);
                glide.speed = 0;
            }
        }

        // Sounds
        if (SeagullSoundHeat.heat < Mathf.Pow(Random.value, 1 / soundFrequency / Time.deltaTime))
        {
            AudioSource.PlayClipAtPoint(sounds[Random.Range(0, sounds.GetLength(0))], myPosition, 0.9f);
            SeagullSoundHeat.heat += (1 / soundFrequency) / 10;
        }

        normalizedVelocity = velocity.normalized;
    }
}
