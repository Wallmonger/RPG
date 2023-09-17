using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();

        // Sending to CloneSkill, the position value of the player, to make a clone image
        player.skill.clone.CreateClone(player.transform, Vector3.zero);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);

        
        
        // Facing Dir is 1 or -1
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        // As stateTimer = dashDuration, when it will hit 0, we change state to Idle
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    
}
