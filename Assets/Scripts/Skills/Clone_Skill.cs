using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{

    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;


    // After getting _clonePosition from DashState
    public void CreateClone(Transform _clonePosition)
    {
        // Creating a gameObject variable which contains our prefab (clone object)
        GameObject newClone = Instantiate(clonePrefab);

        // Sending the data of position to our script on the prefab
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack);
    }
}
