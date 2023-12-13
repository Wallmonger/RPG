using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    // Other way to assign value, without the need of going on an awake or start function
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger ()
    {
        // Like the gizmos created, registers all collision with object in a circle (center, radius)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        // If at least one enemy is found on the array, trigger Damage()
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target);

                // Get effect of weapon
                ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                    weaponData.Effect(_target.transform);
                
            }
        }
    }


    private void throwSword()
    {
        // will be launch at frame of throwSwordAnim
        SkillManager.instance.sword.CreateSword();
    }
}
