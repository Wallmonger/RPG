using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{

    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir; 

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        // instead of making player = GameObject.Find("Player").transform, we can use our static class;
        player = PlayerManager.instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        // If the player is in range and cooldown + lastTimeAttack < currentTime, entering attackState
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            // If enemy battle time has expired, or the enemy is too far away, we stop the battleState
            //! Vector2.Distance allows to calculate the distance between 2 transform position
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }
            

        // Setting the enemy direction based on the placement of the player. 
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        // If the current time is superior to the previous attack time + cooldown, then we return true to make attack possible again
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            // Reset the timer
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        
        Debug.Log("Attack is not available");
        return false;
        
    }
}
