using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        // Making the player available to all skill scripts
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            // Use Skill
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("In cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        // Specific skill
    }
}
