using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();    
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMoveToEnemy;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestTarget)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        // Searching for enemy in layer, then take a random one
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25, whatIsEnemy);

        if (colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;

    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;    

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        // Making crystal move towards closest enemy
        if (canMoveToEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            // Crystal explosion when close to enemy
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMoveToEnemy = false;
            }
        }

        if (canGrow)
        {
            // Growing size of explosion anim and circleCollider (initVal, finalVal, Speed)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
        }

    }

    private void AnimationExplodeEvent ()
    {
        // Explosion hitbox from crystal, taking CircleCollider as radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        // If at least one enemy is found on the array, trigger Damage()
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
