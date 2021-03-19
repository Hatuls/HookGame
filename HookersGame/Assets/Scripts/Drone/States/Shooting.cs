using System;
using UnityEngine;

public class Shooting : StateAbst  
{
    float currentTime;
    float timeTillAnimationFinished = 2f;
    bool toShoot;
    public Shooting(Enemy enmy) : base(enmy)
    {
    
    }

    public override void OnStateEnter()
    {
        currentTime = Time.time;
        toShoot = false;
    }
    public override Type Tick()
    {
        if (IsAnimationDurationOver())
        {
            Debug.Log("ChargingAnimation");
            toShoot = true;
        }

        if (toShoot)
        {
            _enemy.ShootProjectile();
                return typeof(RePositioning);
        }

        return null;
    }
    public bool IsAnimationDurationOver()
        => Time.time > currentTime + timeTillAnimationFinished;
}
