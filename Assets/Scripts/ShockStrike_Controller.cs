using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        targetStats = _targetStats;
        damage = _damage;
    }
    
    void Update()
    {
        if (!targetStats)
            return;

        if (triggered)
            return;

        // Origin, Target, Time
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);

        // Rotating lightning bolt to face enemy
        transform.right = transform.position - targetStats.transform.position;

        // If lightning close to enemy, deal damage, then delete
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            // Return original rotation of anim, and make it 3x bigger on impact
            anim.transform.localPosition = new Vector3(0, .5f);  // Fix ground offset
            anim.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3,3);

            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}
