using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dray : MonoBehaviour, IFacingMover, IKeyMaster {

    public enum eMode { idle, move, attack, transition, knockback }

    [Header("Set in Inspector")]
    public float speed = 5;
    public float attackDuration = 0.25f;
    public float attackDelay = 0.5f;
    public float transitionDelay = 0.5f;
    public int maxHealth = 10;
    public float knockbackSpeed = 10;
    public float knockbackDuration = 0.25f;
    public float invincibleDuration = 0.5f;

    [Header("Set Dynamically")]
    public int dirHeld = -1;
    public int facing = 1;
    public eMode mode = eMode.idle;
    public int numKeys = 0;
    public bool invincible = false;

    [SerializeField]
    private int _health;

    public int health {
        get { return _health; }
        set { _health = value; }
    }

    private float timeAtkDone = 0;
    private float timeAtkNext = 0;
    private float transitionDone = 0;
    private Vector2 transitionPos;
    private float knockbackDone = 0;
    private float invincibleDone = 0;
    private Vector3 knockbackVel;

    private SpriteRenderer sRend;
    private Rigidbody rigid;
    private Animator anim;
    private InRoom inRm;

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
        sRend = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inRm = GetComponent<InRoom>();
        health = maxHealth;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //Check knockback and invincibility
        if (invincible && Time.time > invincibleDone) invincible = false;
        sRend.color = invincible ? Color.red : Color.white;
        if(mode == eMode.knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }

        if (mode == eMode.transition)
        {
            rigid.velocity = Vector3.zero;
            anim.speed = 0;
            roomPos = transitionPos;    //Keeps Dray in place

            if (Time.time < transitionDone)
                return;

            //The following line is only reached if Time.time >= transitionDone
            mode = eMode.idle;
        }

        //Handle Keyboard Input and manage eDrayModes
        dirHeld = -1;

        //key inputs
        for (int i = 0; i<4; i++)
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

    void LateUpdate()
    {
        //Get the half-grid location of this GameObject
        Vector2 rPos = GetRoomPosOnGrid(0.5f);

        //Check to see whether we're in a Door tile
        int doorNum;
        for (doorNum = 0; doorNum < 4; doorNum++)
        {
            if (rPos == InRoom.DOORS[doorNum])
                break;
        }

        if (doorNum > 3 || doorNum != facing)
            return;

        //Move to the next room
        Vector2 rm = roomNum;

        switch(doorNum)
        {
            case 0:
                rm.x += 1;
                break;
            case 1:
                rm.y += 1;
                break;
            case 2:
                rm.x -= 1;
                break;
            case 3:
                rm.y -= 1;
                break;
        }

        if(rm.x >= 0 && rm.x <= InRoom.MAX_RM_X)
        {
            if(rm.y >= 0 && rm.y <= InRoom.MAX_RM_Y)
            {
                roomNum = rm;
                transitionPos = InRoom.DOORS[(doorNum+2)%4];
                roomPos = transitionPos;
                mode = eMode.transition;
                transitionDone = Time.time + transitionDelay;

            }
        }
    }

    void OnCollisionEnter (Collision coll)
    {
        if (invincible) return; //Return ifn Dray can't be damaged
        DamageEffect dEf = coll.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return; //If no DamageEffect, exit this method

        health -= dEf.damage; // Subtract the damage amount from health
        invincible = true; //Make Dray invincible
        invincibleDone = Time.time + invincibleDuration;

        if (dEf.knockback)
        {
            //Determine the direction of knockback
            Vector3 delta = transform.position - coll.transform.position;
            if(Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                //Knockback should be horizontal
                delta.x = (delta.x > 0) ? 1 : -1;
                delta.y = 0;
            }
            else
            {
                //Knockback should be vertical
                delta.y = (delta.y > 0) ? 1 : -1;
                delta.x = 0;
            }

            //Apply knockback speed to the Rigidbody
            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;

            //Set mode to knockback and set time to stop knockback
            mode = eMode.knockback;
            knockbackDone = Time.time + knockbackDuration;
        }
    }

    //Implementation of IFacingMover
    public int GetFacing()
    {
        return facing;
    }

    public bool moving
    {
        get
        {
            return (mode == eMode.move);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float gridMult
    {
        get { return inRm.gridMult; }
    }

    public Vector2 roomPos
    {
        get { return inRm.roomPos; }
        set { inRm.roomPos = value; }
    }

    public Vector2 roomNum
    {
        get { return inRm.roomNum; }
        set { inRm.roomNum = value; }
    }

    public Vector2 GetRoomPosOnGrid(float mult = -1)
    {
        return inRm.GetRoomPosOnGrid(mult);
    }

    public int keyCount
    {
        get { return numKeys; }
        set { numKeys = value; }
    }
}
