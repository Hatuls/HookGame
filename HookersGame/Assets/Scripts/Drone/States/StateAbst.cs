using System;
using UnityEngine;

public abstract class StateAbst
{
    protected Enemy enemy;
    protected GameObject gameObject;
    protected Transform transform;
    public StateAbst(Enemy enmy) {
        this.enemy = enmy;
        this.gameObject = enmy.gameObject;
        this.transform = enmy.transform;
    }
    public abstract Type Tick();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}
