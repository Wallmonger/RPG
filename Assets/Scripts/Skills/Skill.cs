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

        return false;
    }

    public virtual void UseSkill()
    {
        // Specific skill
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        // Will search for colliders in a circle, where _checkTransform is the center
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // Retrieve distance to enemy currently in the loop, compared to the _checkTransform position
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                // If the distance is less than the last occurence, register enemyPosition and new closestDistance
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }

            }
        }

        return closestEnemy;
    }
}
