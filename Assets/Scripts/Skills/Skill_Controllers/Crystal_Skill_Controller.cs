using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMoveToEnemy;
    private float moveSpeed;


    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;    

        if (crystalExistTimer < 0)
            SelfDestroy();
            
    }

    public void SelfDestroy() => Destroy(gameObject);
}
