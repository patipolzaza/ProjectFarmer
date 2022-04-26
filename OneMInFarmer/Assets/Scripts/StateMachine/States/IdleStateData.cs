using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animal/StateMachine/StateData/Idle", fileName = "D_IdleState")]
public class IdleStateData : ScriptableObject
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 10f;
}
