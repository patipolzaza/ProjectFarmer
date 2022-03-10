using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private IdleStateData stateData;
    private float idleTime;
    public IdleState(Animal entity, StateMachine stateMachine, string animBoolName, IdleStateData stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + idleTime)
        {
            //stateMachine.ChangeState(entity.moveState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

    public override void Start()
    {
        base.Start();

        RandomIdleTime();
    }

    private void RandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime + 1);
    }
}
