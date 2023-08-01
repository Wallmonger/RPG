using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represent a single state of the player

public class PlayerState 
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine; 
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("i enter " + animBoolName);
    }

    public virtual void Update()
    {
        Debug.Log("I'm in " + animBoolName);
    }

    public virtual void Exit()
    {
        Debug.Log("I exit " + animBoolName);
    }

}
