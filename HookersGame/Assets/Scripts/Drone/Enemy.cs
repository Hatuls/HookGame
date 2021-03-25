using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform Target;
  [SerializeField] GameObject Bullet;

    StateMachine SM;
    Transform Body;
    public Transform GetDroneBody => Body;
    public void Start()
    {
       
        Body = transform.GetChild(0).transform;
        AssignStates();
     
    }
    Dictionary<Type, StateAbst> states;

    void AssignStates()
    {
        if (states == null)
        {
            states = new Dictionary<Type, StateAbst>()
              {
                { typeof(Wingle),  new Wingle(this) },
                { typeof(Shooting),  new Shooting(this) },
                { typeof(Hooked),  new Hooked(this) },
                { typeof(RePositioning),  new RePositioning(this) },

              };
        }
        if (SM == null)
            SM = GetComponent<StateMachine>();

        SM.SetState(states);
    }

    internal void ShootProjectile()
    {
        Debug.Log("Shooting");
    }


    void RotateTowardThePlayer()
    => Body.localRotation = Quaternion.Lerp(Body.localRotation, ToolClass.RotateToLookTowards(Body,Target), Time.deltaTime * EnemyManager.GetRotationSpeed);
    

     void KeepZAxisDistance() {

        if (Mathf.Abs(transform.position.z - Target.position.z) < EnemyManager.GetDistanceFromPlayer|| transform.position.z< Target.position.z)
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.forward * EnemyManager.GetDistanceFromPlayer, Time.deltaTime);
     
    }
    public void EveryTickCheck() {
        RotateTowardThePlayer();
        KeepZAxisDistance();
    }
}
