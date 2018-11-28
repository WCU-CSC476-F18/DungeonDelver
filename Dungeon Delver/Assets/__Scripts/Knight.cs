using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy, IFacingMover
{
    [Header("Set in Inspector: Knight")]
    public int speed = 1;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;

    [Header("Set Dynamically: Knight")]
    public int facing = 0;
    public float timeNextDecision = 0;

    private InRoom inRm;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        inRm = GetComponent<InRoom>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        if (knockback)
            return;

        if (Time.time >= timeNextDecision)
            DecideDirection();

        anim.CrossFade("Knight_" + facing, 0);
        //rigid is inherited from Enemy and is initialized in Enemy.Awake()
        rigid.velocity = directions[facing] * speed; //Makes enemy move
    }

    void DecideDirection()
    {
        facing = Random.Range(0, 4);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }


    //Implementation of IfacingMover
    public int GetFacing()
    {
        return facing;
    }

    public bool moving
    {
        get
        {
            return true;
            //Returns true because the enemy is always moving
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
}
