using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

    }


    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        // Fix for retrieving the sword in the air
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //! rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

        
    }

    private void Update()
    {
        // Making the sword rotate on air
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            // MoveTowards(object to move, target, speed)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            // If the returning sword is close enough, calling the function to clear player sword variable
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        anim.SetBool("Rotation", false);

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
