using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour {
    [Header("Set in Inspector")]
    public float speed = 5;

    [Header("Set Dynamically")]
    public int dirHeld = -1;

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

        Vector3 vel = Vector3.zero;
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
	}
}
