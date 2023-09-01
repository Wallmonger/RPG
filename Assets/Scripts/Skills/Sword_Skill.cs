using System;
using UnityEngine;


public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    // will be used to calculate the ending direction of the sword, based on gravity
    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            // Getting the values of direction between 0 and 1, then multiply by the force of each axis
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }

    }

    public void CreateSword()
    {
        // Instanciate can take two more value, to be precise about the position and rotation when created
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);

        // Getting access of Sword_Skill_Controller via created prefab
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        

        newSwordScript.SetupSword(finalDir, swordGravity, player);

        player.AssignNewSword(newSword);

        // When releasing the key, no need to see the dots anymore
        DotsActive(false);
    }


    #region Aim region
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

    // Making all dots visible or invisible
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        // Creating dots array with length equals to numberOfDots. Values will be null until we enter the for loop
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            // Instantiation of prefab gameObject for each numberOfdots (gameObject, position, rotation, parent who makes possible to move each one at the same time) 
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        // (Vector2)player gets explicitly the V2 values out of the player, not the V3
        // y-Axis => Making gravity fall on the dot, representing the trajectory of the throw

        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

    #endregion
}
