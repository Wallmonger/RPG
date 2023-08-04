using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
        

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        // If the player is in the air, and moves, we slow down velocity to make it more controllable
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
        
    }

}
