using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // We add combo Window to the last known attack time, to check if the player reach the limit to make a combo (if the time is 10 and we attacked at 9, 9 + 2 = 11. As our time is bigger, we can proceed to a second attack)
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        Debug.Log(comboCounter);
    }

    public override void Exit()
    {
        base.Exit();

        comboCounter++;
        lastTimeAttacked = Time.time;
        
    }

    public override void Update()
    {
        base.Update();
        
        

        // If the boolean from playerState is true, then we switch to idleState, as the attack animation has stopped
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
