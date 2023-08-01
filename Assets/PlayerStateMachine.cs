using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Main class that manages the state machine.
// CurrentState stores the current state of the player


public class PlayerStateMachine
{
    // {get; privateset} means that to get it, it is public, but to set it, it's private. (can be seen but not updated)
    public PlayerState currentState { get; private set; }   
    
    // Load the PlayerState filled by IdleState or MoveState
    public void Initialize(PlayerState _startState)
    {
        // Will be launch on awake
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
