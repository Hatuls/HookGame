using System;
using UnityEngine;

public class RePositioning : StateAbst
{
  
    Vector2 NewLocation;
    public RePositioning(Enemy enmy) : base(enmy)
    {
    }

    public override Type Tick()
    {
        _enemy.EveryTickCheck();
     
        if (NewLocation == Vector2.zero)
        {
            do
                NewLocation = EnemyManager.Instance.GetRandomPosition();
            while (PlatformManager.Instance.CheckEnviroment(NewLocation));
        }



        if (NewLocation != Vector2.zero)
            transform.position = Vector3.Slerp(transform.position, new Vector3(NewLocation.x, NewLocation.y, transform.position.z), EnemyManager.GetDroneSpeed * Time.deltaTime) ;
        if (CheckIfDroneIsAtXYLocation())
        {
            return typeof(Wingle);
        }

        return null;
    }
    public override void OnStateEnter()
    {
        NewLocation = Vector2.zero;
    }
    bool CheckIfDroneIsAtXYLocation() {

        return Vector2.Distance(transform.position, NewLocation) < 0.1f;
    }
}
