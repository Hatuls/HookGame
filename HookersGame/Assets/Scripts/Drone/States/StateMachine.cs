using UnityEngine;
using System;
using System.Collections.Generic;
public class StateMachine : MonoBehaviour 
{
    
    private Dictionary<Type, StateAbst> stateDict;

    public StateAbst currentState { get; private set; }


    public void SetState(Dictionary<Type, StateAbst> states)
        => stateDict = states;
    public void SwitchToNewState(Type nextState) {
        stateDict[nextState].OnStateExit();
        currentState = stateDict[nextState];
        currentState.OnStateEnter();
    }

    private void Update()
    {
        if (currentState == null)
            currentState = stateDict[typeof(Wingle)];


        var nextState = currentState?.Tick();
        Debug.Log("Enemy Current State: " + currentState);
        if (nextState != null && nextState != currentState?.GetType())
            SwitchToNewState(nextState);


        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchToNewState(typeof(Hooked));
        }
    }



}
