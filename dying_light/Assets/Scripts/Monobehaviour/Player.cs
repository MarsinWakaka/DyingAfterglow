using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class Player : Subject
{
    public int combo;                           //连击数

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speed = 10f;
        health = 100f;
        protection = 0f;

        thisFSM = new FSM(this);
        thisFSM.AddState(PlayerState.idleState, new IdleState(this));
        thisFSM.AddState(PlayerState.runState, new RunState(this));
        thisFSM.AddState(PlayerState.charge, new ChargeState(this));
        thisFSM.AddState(PlayerState.dodge, new DodgeState(this));
        thisFSM.AddState(PlayerState.death, new DeathState(this));
        thisFSM.AddState(PlayerState.takeHit, new TakeHit(this));
        thisFSM.AddState(PlayerState.thump, new ThumpState(this));
        thisFSM.InitializeFSM(PlayerState.idleState);
        

        #region Origion
        //currentState = PlayerState.idleState;
        //TransitonState(currentState);
        //nextState = PlayerState.idleState;
        #endregion Origion
    }

    // Update is called once per frame
    void Update()
    {
        thisFSM.RunState();

        if (Input.GetKeyDown(KeyCode.H))
        {
            thisFSM.TransitionState(PlayerState.takeHit);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            thisFSM.TransitionState(PlayerState.death);
        }

        #region Origion
        ////暂且用键盘输入代替角色受伤以及死亡
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    currentState = currentState.TransitonState(takeHit);
        //}
        //else if (Input.GetKeyDown(KeyCode.K))
        //{
        //    currentState = currentState.TransitonState(deathState);
        //}
        //else
        //{
        //    currentState.OnUpdate();
        //}
        ////Debug.Log("当前角色的横向速度为:" + parameters.rg.velocity.x);
        #endregion Origion
    }

    #region Origion
    //public void TransitonState(PlayerState targetType)
    //{
    //    stateIndex[currentState].OnExit();
    //    currentState = targetType;
    //    //Debug.Log("当前角色状态为" + currentState);
    //    stateIndex[currentState].OnEnter();
    //}
    #endregion Origion
}
