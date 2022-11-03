using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;


public  interface IStatement
{

    public   void OnEnter() { }

    public   void OnExit() { }

    public   void OnUpdate() { }

}

public class FSM
{
    MonoBehaviour character;
    IStatement currentState;
    Dictionary<System.Enum, IStatement> stateIndex = new Dictionary<System.Enum, IStatement>();

    public FSM(Subject _character)
    {
        character = _character;
    }

    public bool AddState<T>(T stateName, IStatement state) where T : System.Enum
    {
        stateIndex.Add(stateName, state);
        //if (stateIndex.Count == 0)
        //{
        //    stateIndex.Add(stateName, state);
        //    return true;
        //}
        //else
        //{
        //    if (stateIndex.Keys.GetType() == stateName.GetType())
        //    {
        //        stateIndex.Add(stateName, state);
        //        return true;
        //    }
        //}
        return false;
    }

    public void InitializeFSM(Enum startState)
    {
        currentState = stateIndex[startState];
    }

    public void RunState()
    {
        currentState.OnUpdate();
    }

    public void TransitionState(Enum targetState)
    {
        currentState.OnExit();
        //Debug.Log("当前角色状态为" + currentState);
        stateIndex[targetState].OnEnter();
        currentState = stateIndex[targetState];
    }
}