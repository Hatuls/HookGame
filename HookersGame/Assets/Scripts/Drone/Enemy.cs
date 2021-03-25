using System;

using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    StateMachine SM;

    [SerializeField] GameObject Bullet;

    [SerializeField] Transform _target;
    Transform _body;

    Dictionary<Type, StateAbst> statesDict;

    public Transform GetDroneBody => _body;
    public void Start()
    {
        Init();
    }
    public void Init()
    {    
        _body = transform.GetChild(0).transform;
        AssignStates();
    }

    internal void ShootProjectile()
    {
        Debug.Log("Shooting");
    }
    public void EveryTickCheck()
    {
        RotateTowardThePlayer();
        KeepZAxisDistance();
    }

    void AssignStates()
    {
        if (statesDict == null)
        {
            statesDict = new Dictionary<Type, StateAbst>()
              {
                { typeof(Wingle),  new Wingle(this) },
                { typeof(Shooting),  new Shooting(this) },
                { typeof(Hooked),  new Hooked(this) },
                { typeof(RePositioning),  new RePositioning(this) },

              };
        }
        if (SM == null)
            SM = GetComponent<StateMachine>();

        SM.SetState(statesDict);
    }


    void RotateTowardThePlayer()
    => _body.localRotation = Quaternion.Lerp(_body.localRotation, ToolClass.RotateToLookTowards(_body,_target), Time.deltaTime * EnemyManager.GetRotationSpeed);
    

     void KeepZAxisDistance() {

        if (Mathf.Abs(transform.position.z - _target.position.z) < EnemyManager.GetDistanceFromPlayer|| transform.position.z< _target.position.z)
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.forward * EnemyManager.GetDistanceFromPlayer, Time.deltaTime);
     
    }
}
