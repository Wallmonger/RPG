using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab;

    public void CreateClone()
    {
        // Creating a gameObject variable which contains our prefab (clone object)
        GameObject newClone = Instantiate(clonePrefab);
    }
}
