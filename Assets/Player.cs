using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;
    #endregion
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

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


    public PlayerPrimaryAttack primaryAttack { get; private set; }


    #endregion

    private void Awake()
    {
        // Before the first frame, initialization of playerStateMachine, and all the states
        StateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, StateMachine, "Idle");
        moveState = new PlayerMoveState(this, StateMachine, "Move");
        jumpState = new PlayerJumpState(this, StateMachine, "Jump");
        airState = new PlayerAirState(this, StateMachine, "Jump");
        dashState = new PlayerDashState(this, StateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, StateMachine, "Jump");

        primaryAttack = new PlayerPrimaryAttack(this, StateMachine, "Attack");
    }


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Using the initialize function to enter the playerStateMachine
        StateMachine.Initialize(idleState);

    }

    private void Update()
    {
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

    public void SetVelocity (float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    // If a function is of type return, we can use arrow 
    // Raycast takes the object position, direction of the second point, distance between two points, and the layer
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    // We multiply Vector2.right by facingDir to always detect walls in front of the character
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);


    // DrawLine takes two parameter to draw a line
    private void OnDrawGizmos()
    {
        // we substract on y axis the groundCheckDistance to make the second point far from the first one. the more groundCheckDistance, the further it will go down
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public void Flip ()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController (float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        } else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
}
