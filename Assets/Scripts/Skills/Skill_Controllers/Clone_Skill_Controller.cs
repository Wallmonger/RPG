using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLoosingSpeed;

    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();    
        anim = GetComponent<Animator>();    
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;   
        
        if (cloneTimer < 0 )
        {
            // Decreasing the transparency of the sprite's color to make it disapear
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            // If not visible, destruction
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    // Setting the created object on the position of the player, when entering dashState
    public void SetupClone (Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        // If the player has unlocked the skill, attack animation will play on the clone, at random from 1 to 3
        if (_canAttack )
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        


        cloneTimer = _cloneDuration;
        transform.position = _newTransform.position;


        FaceClosestTarget();
    }


    private void AnimationTrigger()
    {
        // If clone is at the end of animation, cloneTimer set to negative, so the condition of spritecolor disapear will be met
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        // Like the gizmos created, registers all collision with object in a circle (center, radius)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        // If at least one enemy is found on the array, trigger Damage()
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    private void FaceClosestTarget()
    {
        // Will search for colliders in a circle, where clone is the center
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null )
            {
                // Retrieve distance to enemy currently in the loop, compared to the clone position
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                // If the distance is less than the last occurence, register enemyPosition and new closestDistance
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
                
            }
        }

        // at the end of the loop, we check the distance of the closest enemy, if the player is on the right of the enemy (positive value), we flip the clone
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }

    }
}