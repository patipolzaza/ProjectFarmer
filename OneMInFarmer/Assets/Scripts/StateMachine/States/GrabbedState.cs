using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedState : State
{
    private bool isUnleashed;

    public GrabbedState(Animal entity, StateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {

    }
    public override void Start()
    {
        base.Start();
        isUnleashed = false;

        entity.rb.isKinematic = true;
    }

    public override void Exit()
    {
        base.Exit();
        entity.rb.isKinematic = false;
        isUnleashed = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isUnleashed)
        {
            stateMachine.ChangeState(entity.idleState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

    public void Unleash()
    {
        isUnleashed = true;
    }
}
