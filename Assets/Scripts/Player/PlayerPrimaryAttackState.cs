using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Need xInput to 0 to fix attack on Enter() the state
        xInput = 0;

        // We add combo Window to the last known attack time, to check if the player reach the limit to make a combo (if the time is 10 and we attacked at 9, 9 + 2 = 11. As our time is bigger, we can proceed to a second attack)
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);


        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        // set velocity of each attack
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        // We set stateTimer to .1f to let the character move before the attack (update func)
        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCounter++;
        lastTimeAttacked = Time.time;
        
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        // If the boolean from playerState is true, then we switch to idleState, as the attack animation has stopped
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
