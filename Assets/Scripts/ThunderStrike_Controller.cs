using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;

    private Animator anim;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        // Origin, Target, Time
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);

        // If lightning close to enemy, deal damage, then delete
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            targetStats.TakeDamage(1);
            anim.SetTrigger("Hit");
            Destroy(gameObject, .4f);
        }
    }
}
