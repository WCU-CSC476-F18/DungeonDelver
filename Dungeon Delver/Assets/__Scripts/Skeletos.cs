using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeletos : Enemy {

    [Header("Set in Inspector: Skeletos")]
    public int speed = 2;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;

    [Header("Set Dynamically: Skeletos")]
    public int facing = 0;
    public float timeNextDecision = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= timeNextDecision)
            DecideDirection();

        //rigid is inherited from Enemy and is initialized in Enemy.Awake()
        rigid.velocity = directions[facing] * speed;
	}

    void DecideDirection()
    {
        facing = Random.Range(0, 4);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);

        Debug.Log("Test Modification");
    }
}
