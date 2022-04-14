using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected Animal entity;

    protected StateMachine stateMachine;
    protected string animBoolName;

    protected float startTime;

    public State(Animal entity, StateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Start()
    {
        entity.SetVelocity(0, 0);
        entity.anim.SetBool(animBoolName, true);
        startTime = Time.time;
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicUpdate()
    {

    }
}
