using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;


    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;


    public override void UseSkill()
    {
        base.UseSkill();

        // No crystal = create one. Else teleport to existing crystal then remove it
        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform));

        }
            
        else
        {
            // If crystal can move, prevent teleport
            if (canMoveToEnemy)
                return;
            

            // Switching player position with the crystal 
            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
        }

    }

}
