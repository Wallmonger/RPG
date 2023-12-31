using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represent a single state of the player

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine; 
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        // We set the animation of the animBoolName, from the playerMoveState, playerIdleState etc ..
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        // StateTimer will be set on entering dashState
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        // We set the blend tree animator of jump to change the animation according to the value of y axis ( if y > 0 then jump anim, if not, falling anim)
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    // this function will be triggered to change state to idle, via the animator (attack)
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
