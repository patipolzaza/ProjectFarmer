using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateMachine
{
    public State currentState { get; private set; }

    public void InitializeState(State initialState)
    {
        currentState = initialState;
    }

    public void ChangeState(State nextState)
    {
        currentState.Exit();

        currentState = nextState;
        currentState.Start();
    }
}
