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

    enum HeronStatus {Idle = 0, Walking = 1, Running = 2 };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
