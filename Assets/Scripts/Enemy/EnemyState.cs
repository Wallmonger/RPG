using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; 
    }

    public virtual void Enter()
    {
        triggerCalled = false;

        // Making rb accessible without writting "ennemy.rb.velocity ...."
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);

        // On exiting each state of enemy, register last animation played
        enemyBase.AssignLastAnimName(animBoolName);
    }


    // Will be used to trigger boolean after animation
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
    
}
