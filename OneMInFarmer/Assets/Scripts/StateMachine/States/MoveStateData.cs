using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/StateMachine/StateData/Move", fileName = "D_MoveState")]
public class MoveStateData : ScriptableObject
{
    public float moveSpeed = 275;
    public float maxMoveLength = 2.5f;
}
