using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // StateMachine variable stores the PlayerStateMachine, making awailable the functions Initialize and Update to change our states.

    public PlayerStateMachine StateMachine { get; private set; }


    // My states, which extends PlayerState
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, StateMachine, "Idle");
        moveState = new PlayerMoveState(this, StateMachine, "Move");
    }


    private void Start()
    {
        // Using the initialize function to enter the playerStateMachine
        StateMachine.Initialize(idleState);
    }

    private void Update()
    {
        StateMachine.currentState.Update();
    }
}
