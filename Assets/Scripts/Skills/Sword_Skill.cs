using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    // will be used to calculate the ending direction of the sword, based on gravity
    private Vector2 finalDir;

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            // Getting the values of direction between 0 and 1, then multiply by the force of each axis
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
    }

    public void CreateSword()
    {
        // Instanciate can take two more value, to be precise about the position and rotation when created
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);

        // Getting access of Sword_Skill_Controller via created prefab
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(finalDir, swordGravity);
    }

    // Will return a vector2 value of the mouse aiming
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;

        // Get the mouse position based on the screen ( x-axis / y-axis )
        // ScreenToWorldPoint takes an object to check its position based on the screen
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Getting the direction of the sword's throwing
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

}
