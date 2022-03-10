using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    private MoveStateData stateData;

    private Vector2 moveDirection = new Vector3();
    private float moveTime;

    public MoveState(Animal entity, StateMachine stateMachine, string animBoolName, MoveStateData stateData) : base(entity, stateMachine, animBoolName)
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

        if (Time.time >= startTime + moveTime)
        {
            stateMachine.ChangeState(entity.idleState);
        }
    }

    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

    public override void Start()
    {
        base.Start();

        moveTime = RandomMoveTime();
        moveDirection = FindMoveDirection();

        float velocityX = moveDirection.x * stateData.moveSpeed * Time.deltaTime;
        float velocityY = moveDirection.y * stateData.moveSpeed * Time.deltaTime;

        entity.SetVelocity(velocityX, velocityY);
    }

    private Vector2 FindMoveDirection()
    {
        Vector2 direction;

        float randomedX = Random.Range(-1, 2);
        float randomedY = Random.Range(-1, 2);

        direction = new Vector2(randomedX, randomedY);

        return direction;
    }

    private float RandomMoveTime()
    {
        return Random.Range(stateData.minMoveTime, stateData.maxMoveTime + 1);
    }
}
