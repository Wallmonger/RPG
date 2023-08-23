using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
 
    // Setting the created object on the position of the player, when entering dashState
    public void SetupClone (Transform _newTransform)
    {
        transform.position = _newTransform.position;
    }
}
