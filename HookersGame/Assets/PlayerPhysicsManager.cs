using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class PlayerPhysicsManager 
{

    public enum PhysicsStates { fall, Swing, hang, leap, DefaultAir, DefaultFloor, wall,LastDontUseMe }
    internal PhysicsStates physicsState;

    [Serializable]
    public class DragValues
    {
       public float DefaultFloor, DefaultAir, Swing, leap, hang, fall, wall;
    }
    [SerializeField] DragValues dragValues;

    [Serializable]
    public class StateValues 
    {
        [Header("State")]
        public PhysicsStates stateRequest;
        [Header("next State if there is else same")]
        public PhysicsStates nextState;
        public float TimeBetweenStates;

    }
    [SerializeField] StateValues[] StatesSettings=new StateValues[(int)PhysicsStates.LastDontUseMe-1];




    PlayerManager _playerManager;
    Rigidbody rb;




    public void InitPhysics(Rigidbody referencedRb,PlayerManager playerManager)
    {
        _playerManager = playerManager;
        rb = referencedRb;
    }


    public void CaulculatePhysics(bool grounded,bool grappled,bool wall)
    {
        if (grounded)
        {
            PhysicsRequest(PhysicsStates.DefaultFloor);
        }
        








    }

     void ChangeState(PhysicsStates newActiveState)
    {
        physicsState = newActiveState;
        switch (newActiveState) 
        {
            
            case PhysicsStates.DefaultAir:
                rb.drag = dragValues.DefaultAir;
                break;

            case PhysicsStates.DefaultFloor:
                rb.drag = dragValues.DefaultFloor;
                break;


            case PhysicsStates.fall:
                rb.drag = dragValues.fall;
                break;

            case PhysicsStates.hang:
                rb.drag = dragValues.hang;
                break;

            case PhysicsStates.leap:
                rb.drag = dragValues.leap;
                break;

            case PhysicsStates.Swing:
                rb.drag = dragValues.Swing;
                break;


            case PhysicsStates.wall:
                rb.drag = dragValues.wall;
                break;
        }



       
    }
     void PhysicsRequest(PhysicsStates stateRequested)
    {
        foreach(StateValues found in StatesSettings)
        {
            if (found.stateRequest == stateRequested)
            {
                _playerManager.StartCoroutine(PhysicsTransistor(stateRequested, found.nextState, found.TimeBetweenStates));
            }
        }
       
    }

    IEnumerator PhysicsTransistor(PhysicsStates stateRequested, PhysicsStates nextState, float TimeBeforChange)
    {

        ChangeState(stateRequested);
        yield return new WaitForSeconds(TimeBeforChange);
        ChangeState(nextState);
    }

    


    // Start is called before the first frame update
   
}
