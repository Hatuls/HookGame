using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class PlayerPhysicsManager 
{

    public enum PhysicsStates { fall, Swing, hang, leap, DefaultAir, DefaultFloor, wall,AfterPulse,LastDontUseMe }
    internal PhysicsStates physicsState;

    [Serializable]
    public class DragValues
    {
       public float DefaultFloor, DefaultAir, Swing, leap, hang, fall, wall,afterPulse;
    }
    [SerializeField] DragValues dragValues;


    [Serializable]
    public class Speeds
    {
        public float StartswingSpeed,fallSpeed,fallSpeedGrouth;
    }
    [SerializeField] Speeds speeds;


    [Serializable]
    public class StateValues 
    {
        
        [Tooltip("State")]
        public PhysicsStates stateRequest;
        [Tooltip("next State if there is else same")]
        public PhysicsStates nextState;
        public float TimeBetweenStates;

    }
    [SerializeField] StateValues[] StatesSettings=new StateValues[(int)PhysicsStates.LastDontUseMe];




    PlayerManager _playerManager;
    Rigidbody rb;


    

    public void InitPhysics(Rigidbody referencedRb,PlayerManager playerManager)
    {
        _playerManager = playerManager;
        rb = referencedRb;
    }


    public void CaulculatePhysics(bool grounded,bool grappled,bool PulseUsed,bool wall)
    {
        
        PhysicsStates StateResult=PhysicsStates.LastDontUseMe;
        float speed = rb.velocity.magnitude;
        if (grounded)
        {
            StateResult = PhysicsStates.DefaultFloor;
        }
        else
        {

            if (grappled)
            {
                if (speed > speeds.StartswingSpeed)
                {
                    StateResult = PhysicsStates.Swing;
                }
                else if(speed>1) 
                { StateResult = PhysicsStates.hang; }
            }
            else 
            {
                if(physicsState == PhysicsStates.Swing)
                {
                    StateResult = PhysicsStates.leap;
                }
                else
                {
                    if (physicsState != PhysicsStates.AfterPulse&& physicsState != PhysicsStates.leap&& physicsState != PhysicsStates.DefaultAir)
                    {
                        StateResult = PhysicsStates.fall;
                    }
                } 
                

            }
            

        }






        if (PulseUsed)
        {
          
            StateResult = PhysicsStates.AfterPulse;
        }

        if(StateResult!=physicsState)
        PhysicsRequest(StateResult);


    }
    IEnumerator LongEffect(PhysicsStates physicsStates)
    {
        float startTime = Time.time;
        float EffectTime = 0;
        StateValues state=new StateValues();
        
        foreach(StateValues found in StatesSettings)
        {
            if (physicsState == found.stateRequest)
            {
                EffectTime = found.TimeBetweenStates;
                state = found;
                break;
            }
        }


        while (startTime + EffectTime > Time.time)
        {
           // Debug.Log(physicsStates);
            yield return null;
            physicsState = physicsStates;
        }
        ChangeState(state.nextState);

    }

     void ChangeState(PhysicsStates newActiveState)
    {
        physicsState = newActiveState;
        //Debug.Log("Current Physics State -"+newActiveState);
        switch (newActiveState) 
        {
            
            
            case PhysicsStates.DefaultAir:
                _playerManager.StartCoroutine(LongEffect(newActiveState));
                rb.drag = dragValues.DefaultAir;
                
                break;

            case PhysicsStates.DefaultFloor:
                rb.drag = dragValues.DefaultFloor;
                break;

                
            case PhysicsStates.fall:
                _playerManager.StartCoroutine(FallLoop(newActiveState));
                rb.drag = dragValues.fall;
                
                
                break;

            case PhysicsStates.hang:
                rb.drag = dragValues.hang;
                break;

            case PhysicsStates.leap:
                _playerManager.StartCoroutine(LongEffect(newActiveState));
                rb.drag = dragValues.leap;
                break;

            case PhysicsStates.Swing:
                rb.drag = dragValues.Swing;
                break;


            case PhysicsStates.wall:
                rb.drag = dragValues.wall;
                break;

            case PhysicsStates.AfterPulse:
                _playerManager.StartCoroutine(LongEffect(newActiveState));
                rb.drag = dragValues.afterPulse;
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
                break;
            }
            
            
            

            
        }
       
    }

    IEnumerator PhysicsTransistor(PhysicsStates stateRequested, PhysicsStates nextState, float TimeBeforChange)
    {
        
        ChangeState(stateRequested);
        yield return new WaitForSeconds(TimeBeforChange);
        if (stateRequested != nextState) 
        ChangeState(nextState);
    }

    IEnumerator FallLoop(PhysicsStates state)
    {
        float fallSpeed = speeds.fallSpeed;
        while (physicsState == state)
        {
            fallSpeed += speeds.fallSpeedGrouth;
            rb.AddForce(Vector3.down *fallSpeed,ForceMode.Acceleration);
            yield return new WaitForFixedUpdate();

        }
    }

    
    // Start is called before the first frame update
   
}
