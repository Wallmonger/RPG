using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();    
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;   
        
        if (cloneTimer < 0 )
        {
            // Decreasing the transparency of the sprite's color to make it disapear
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
        }
    }

    // Setting the created object on the position of the player, when entering dashState
    public void SetupClone (Transform _newTransform, float _cloneDuration)
    {
        cloneTimer = _cloneDuration;
        transform.position = _newTransform.position;
    }
}
