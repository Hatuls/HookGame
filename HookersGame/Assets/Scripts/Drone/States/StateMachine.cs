using UnityEngine;
using System;
using System.Collections.Generic;
public class StateMachine : MonoBehaviour 
{
    
    private Dictionary<Type, StateAbst> stateDict;

    public StateAbst currentState { get; private set; }
    int repeatStateCounter;
    [SerializeField] int RepeatLimit =3 ;
    public void SetState(Dictionary<Type, StateAbst> states)
        => stateDict = states;
    public void SwitchToNewState(Type nextState) {
        stateDict[nextState].OnStateExit();
        currentState = stateDict[nextState];
        currentState.OnStateEnter();
    }
    public void StartDrone()
    {
        BeatUpdate();
    }
    private void BeatUpdate()
    {
        if (gameObject.activeSelf == false)
            return;
        StateAbst state = null;

        do
        {
            state = stateDict[TypeRandomize];

            repeatStateCounter = (state == currentState) ? repeatStateCounter++ : 0;

        }
        while (state == currentState && repeatStateCounter >= RepeatLimit);


        var nextState = state?.Tick();

        if (nextState != null)       
            SwitchToNewState(nextState);
        
        currentState?.Tick();
        Debug.Log("Enemy Current State: " + currentState);



    }
    private Type TypeRandomize
    {
        get
        {
            switch (UnityEngine.Random.Range(0, 4))
            {
                case 1:
                    return typeof(RePositioning);
                case 2:
                    return typeof(Shooting);
                case 3:
                    return typeof(RePositioning);
                case 0:
                default:
                    return typeof(Wingle);
            }
        }
    }


}
