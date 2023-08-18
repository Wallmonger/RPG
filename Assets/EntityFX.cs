using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private int flashOcurrence;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        // Assign sr to the sprite renderer in animator, from player or enemy
        sr = GetComponentInChildren<SpriteRenderer>();

        // Save the original material to set it back after taking damage
        originalMat = sr.material;
    }


    private IEnumerator FlashFX()
    {
        for (int i = 0; i < flashOcurrence; i++)
        {
            sr.material = hitMat;
            yield return new WaitForSeconds(flashDuration);
            sr.material = originalMat;
            yield return new WaitForSeconds(flashDuration);
        }
        

    }
}
