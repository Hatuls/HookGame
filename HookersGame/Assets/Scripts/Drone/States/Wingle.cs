using System;
using UnityEngine;

public class Wingle : StateAbst
{


    public Wingle(Enemy enmy) : base(enmy)
    {
       
    }
    
    public override Type Tick()
    {
        Debug.Log("Enter Wingle");

        return null;
    }

}