using System;
using UnityEngine;

public abstract class StateAbst
{
    protected Enemy _enemy;
    protected GameObject gameObject;
    protected Transform transform;
    public StateAbst(Enemy enmy) {
        this._enemy = enmy;
        this.gameObject = enmy.gameObject;
        this.transform = enmy.transform;
    }
    public abstract Type Tick();
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}
