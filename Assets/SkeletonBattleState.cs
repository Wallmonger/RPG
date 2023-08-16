using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    Enemy_Skeleton enemy;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("I'm in battleState");
    }
    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
