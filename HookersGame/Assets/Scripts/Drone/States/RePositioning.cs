using System;
using UnityEngine;

public class RePositioning : StateAbst
{


    public RePositioning(Enemy enmy) : base(enmy)
    {
   
    }
    public override void OnStateEnter()
    {
        Debug.Log("Drone Repositioning");
    }
    public override Type Tick()
    {
        LeanTween.move(gameObject, TunnelManager.Instance.MoveOnGrid(false,TunnelManager.Instance.GetRandomMovement()), SoundManager.Instance.BeatSpeed);

        return null;
    }



 
}
