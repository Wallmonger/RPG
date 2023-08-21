using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;

        // Instantiate animation for successful attack but disabled, to activate it in case of a SuccessCounterAtk
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        // Like the gizmos created, registers all collision with object in a circle (center, radius)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // If at least one enemy is found on the array, trigger Damage()
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // CanBeStunned() will activate stunnedState on enemy, and return true to trigger this condition
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10f;
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                }
            }
        }

        // If player has not made a good counterAttack, we return to idleState (stateTimer 2f upon entering the state, 10 if successful / triggerCalled at the end of CounterAnim)
        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
