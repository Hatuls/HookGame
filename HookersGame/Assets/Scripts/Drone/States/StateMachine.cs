using UnityEngine;
using System;
using System.Collections.Generic;
public class StateMachine : MonoBehaviour 
{
    
    private Dictionary<Type, StateAbst> stateDict;

    public StateAbst currentState { get; private set; }
    int repeatStateCounter;
    [SerializeField] int RepeatLimit =3 ;

    [SerializeField] TypeChances typeChances;



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
            state = stateDict[typeChances.TypeRandomize];

            repeatStateCounter = (state == currentState) ? repeatStateCounter++ : 0;

        }
        while (state == currentState && repeatStateCounter >= RepeatLimit);


        var nextState = state?.Tick();

        if (nextState != null)       
            SwitchToNewState(nextState);
        
        currentState?.Tick();
        Debug.Log("Enemy Current State: " + currentState);



    }
   
}
[Serializable]
public class TypeChances  {

    [Header("Drone Chances Parameter:")]
    [Tooltip("Chance to be Idle")]
    [SerializeField] int idleChance = 1;
    [Tooltip("Chance to Change Position")]
    [SerializeField] int changePositionChance = 1;
    [Tooltip("Chance to Shoot At player")]
    [SerializeField] int shootingChance = 1;


    public Type TypeRandomize
    {
        get
        {
            int randomNum = UnityEngine.Random.Range(0, idleChance + changePositionChance + shootingChance);

            if (randomNum >=0 && randomNum < idleChance)
            return typeof(Wingle);

            else if (randomNum>= idleChance && randomNum < changePositionChance)
            return typeof(RePositioning);

            else
            return typeof(Shooting); 
        }
    }
}