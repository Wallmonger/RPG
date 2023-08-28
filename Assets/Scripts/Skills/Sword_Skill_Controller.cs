using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

    }


    public void SetupSword(Vector2 _dir, float _gravityScale)
    {
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }

    private void Update()
    {
        // Making the sword rotate on air
        if (canRotate)
            transform.right = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;

        // isKinematic will make the object not affected by physics such as gravity or collisions, making the object only rely on script or anim
        rb.isKinematic = true;

        // Freezes all axis on collision
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Object hit by the sword will become his new parent
        transform.parent = collision.transform;
    }
}
