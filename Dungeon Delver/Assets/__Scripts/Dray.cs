using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour {

    public enum eMode { idle, move, attack, transition }

    [Header("Set in Inspector")]
    public float speed = 5;
    public float attackDuration = 0.25f;
    public float attackDelay = 0.5f;

    [Header("Set Dynamically")]
    public int dirHeld = -1;
    public int facing = 1;
    public eMode mode = eMode.idle;

    private float timeAtkDone = 0;
    private float timeAtkNext = 0; 

    private Rigidbody rigid;
    private Animator anim;

    private Vector3[] directions = new Vector3[]
    {
        Vector3.right, Vector3.up, Vector3.left,
        Vector3.down
    };

    //placement of keycodes in array is important.
    //Index corresponds to radians
    private KeyCode[] keys = new KeyCode[]
    {
    KeyCode.D, //index 0 for 0 radians
    KeyCode.W, //index 1 for 1 radians
    KeyCode.A, //etc...
    KeyCode.S
    };

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        dirHeld = -1;
        //key inputs
        for(int i = 0; i<4; i++)
        {
            if (Input.GetKey(keys[i])) dirHeld = i;
            //If multiple input keys are pressed at the same time, 
            //the last line that evaluates to true will override the rest
        }

        //Pressing the attack button(s)
        if(Input.GetKeyDown(KeyCode.Space) && Time.time >= timeAtkNext)
        {
            mode = eMode.attack;
            timeAtkDone = Time.time + attackDuration;
            timeAtkNext = Time.time + attackDelay;
        }

        if(Time.time >= timeAtkDone)
        {
            mode = eMode.idle;
        }

        if(mode != eMode.attack)
        {
            if(dirHeld == -1)
            {
                mode = eMode.idle;
            }
            else
            {
                facing = dirHeld;
                mode = eMode.move;
            }
        }

        //Act on the current mode
        Vector3 vel = Vector3.zero;
        switch (mode)
        {
            case eMode.attack:
                anim.CrossFade("Dray_Attack_" + facing, 0);
                anim.speed = 0;
                break;
            
            case eMode.idle:
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 0;
                break;

            case eMode.move:
                vel = directions[dirHeld];
                anim.CrossFade("Dray_Walk_" + facing, 0);
                anim.speed = 1;
                break;
        }

        rigid.velocity = vel * speed;

        /*
        if (dirHeld > -1) vel = directions[dirHeld]; // sets direction given key input

        rigid.velocity = vel * speed;

        //Animation
        if (dirHeld == -1)
        {
            anim.speed = 0; //freezes Dray in place
        }
        else
        {
            anim.CrossFade("Dray_Walk_" + dirHeld, 0);
            anim.speed = 1;
        }
        */
	}
}
