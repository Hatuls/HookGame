using System;
using UnityEngine;

public class Wingle : StateAbst
{
    float currentTime;
    float timeTillAttack=2f;
    float timeTillChangingPosition;
    float animationOfShooting;
    public Wingle(Enemy enmy) : base(enmy)
    {
        OnStateEnter();
    }
    public override void OnStateEnter()
    {
        currentTime = Time.time;

        timeTillAttack = UnityEngine.Random.Range(3f, 5f);
        timeTillChangingPosition = UnityEngine.Random.Range(2.5f, 6f);
    }
    public override Type Tick()
    {
        enemy.EveryTickCheck();
        return typeof(RePositioning);
        //check his currentPosition or if time passes certain amount
        if (CheckTimeToChangePosition() ) {
           return typeof(RePositioning);
        }

        if (CheckTimeToFire())
        {
            return typeof(Shooting);
        }

        return null;
    }

    private bool CheckTimeToFire()
    => Time.time > currentTime + timeTillAttack;

    private bool CheckTimeToChangePosition()
        => Time.time > currentTime + timeTillChangingPosition;
}