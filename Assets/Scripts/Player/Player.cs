using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{


    #region Player info

    [Header("Attack details")]
    // will serve to set movement speed for each attack
    public Vector2[] attackMovement;
    public bool isBusy { get; private set; }
    

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCoolDown;
    private float dashUsageTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    

    
    #endregion
    
    #region States
    // StateMachine variable stores the PlayerStateMachine, making awailable the functions Initialize and Update to change our states.
    // The state machine, who will use the Player's State to change actions
    public PlayerStateMachine StateMachine { get; private set; }


    // My states, which extends PlayerState
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }    
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }


    public PlayerPrimaryAttackState primaryAttack { get; private set; }


    #endregion

    protected override void Awake()
    {
        base.Awake();
        // Before the first frame, initialization of playerStateMachine, and all the states
        StateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, StateMachine, "Idle");
        moveState = new PlayerMoveState(this, StateMachine, "Move");
        jumpState = new PlayerJumpState(this, StateMachine, "Jump");
        airState = new PlayerAirState(this, StateMachine, "Jump");
        dashState = new PlayerDashState(this, StateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, StateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
    }


    protected override void Start()
    {
        base.Start();

        // Using the initialize function to enter the playerStateMachine
        StateMachine.Initialize(idleState);

    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();
    }

    // Determine if the player is in an action (attack) to prevent returning to moveState in a combo
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    // Will call the function to set the triggerCall boolean to true
    public void AnimationTrigger () => StateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput ()
    {
        if (IsWallDetected())
            return;
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");

            // If we don't have a direction for the dash, we assign the facing direction instead of the input
            if (dashDir == 0)
                dashDir = facingDir;
            

            // if (dahsDir != 0) would be a solution if we don't want a dash performed after pressing LS and no direction.
            StateMachine.ChangeState(dashState);
        }

        
    }
    
    
    
}
