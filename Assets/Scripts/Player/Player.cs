using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Player : Entity
{


    #region Player info

    [Header("Attack details")]
    // will serve to set movement speed for each attack
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    public bool isBusy { get; private set; }
   
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;

    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }

    

    
    #endregion
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
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
    public PlayerDeadState deadState { get; private set; }  


    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }  
    public PlayerCatchSwordState catchSword { get; private set; }

    public PlayerBlackholeState blackHole { get; private set; }


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
        counterAttack = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, StateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        blackHole = new PlayerBlackholeState(this, StateMachine, "Jump");

        deadState = new PlayerDeadState(this, StateMachine, "Die");
    }

    //! Start()
    protected override void Start()
    {
        base.Start();

        // Setting skill var to the skill manager instance, for short name on the states
        skill = SkillManager.instance;


        // Using the initialize function to enter the playerStateMachine
        StateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    //! UPDATE ()
    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F))
            skill.crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }

    public override void SlowEntityBy(float _slowPercentage, float _duration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);    
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _duration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;  
        dashSpeed = defaultDashSpeed;
    }

    // Assign new sword on throwing
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    // Destroy skill_sword prefab
    public void CatchSword()
    {
        StateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    // Will be used to prevent player from doing actions while busy
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
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            // If we don't have a direction for the dash, we assign the facing direction instead of the input
            if (dashDir == 0)
                dashDir = facingDir;
            

            // if (dahsDir != 0) would be a solution if we don't want a dash performed after pressing LS and no direction.
            StateMachine.ChangeState(dashState);
        }   
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(deadState);
    }

}
