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
        SM.StartDrone();
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.G))
                Init();
          //  EveryTickCheck();
        }
    }
    void RotateTowardThePlayer()
    => _body.localRotation = Quaternion.Lerp(_body.localRotation, ToolClass.RotateToLookTowards(_body,_target), Time.deltaTime * 1);
    


}
