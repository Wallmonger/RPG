using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
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

        // Will be effective in idleState and moveState
        if (enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.battleState);

        
    }
}
