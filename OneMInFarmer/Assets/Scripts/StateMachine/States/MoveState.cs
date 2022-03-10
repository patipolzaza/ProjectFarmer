using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected MoveStateData stateData;

    protected Vector3 moveDestination = new Vector3();
    private bool hasSetDestination;

    public MoveState(Animal entity, StateMachine stateMachine, string animBoolName, MoveStateData stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Exit()
    {
        base.Exit();
        hasSetDestination = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (hasSetDestination)
        {
            MoveToDestination();
            return;
        }

    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

    public override void Start()
    {
        base.Start();

        moveDestination = FindMoveDestination();
    }

    private void MoveToDestination()
    {
        Vector2 velocity = Vector2.MoveTowards(entity.transform.position, moveDestination, stateData.moveSpeed * Time.deltaTime);
        entity.SetVelocity(velocity.x, velocity.y);
    }

    public virtual void SetDestination(Vector3 destination)
    {
        moveDestination = destination;
        hasSetDestination = true;
    }

    private Vector3 FindMoveDirection()
    {


        return Vector3.zero;
    }
}
