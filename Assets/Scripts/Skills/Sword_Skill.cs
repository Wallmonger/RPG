using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        // Instanciate can take two more value, to be precise about the position and rotation when created
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);

        // Getting access of Sword_Skill_Controller via created prefab
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(launchDir, swordGravity);
    }
}
