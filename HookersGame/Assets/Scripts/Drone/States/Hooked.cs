using System;
using UnityEngine;

public class Hooked : StateAbst
{
    public Hooked(Enemy enmy) : base(enmy) { }

    public override Type Tick()
    {
        Debug.Log(gameObject.name + "Got Hooked Cannot Do Anything");
        return null;
    }
}
