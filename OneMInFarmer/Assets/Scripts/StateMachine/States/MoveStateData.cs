using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/StateMachine/StateData/Move", fileName = "D_MoveState")]
public class MoveStateData : ScriptableObject
{
    public float moveSpeed = 1.5f;

    public float minMoveTime = 0.5f;
    public float maxMoveTime = 5f;
}
