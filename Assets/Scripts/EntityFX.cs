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

    [Header("Ailment FX Colors")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;




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

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else 
            sr.color = Color.red;
    }

    private void CancelColorChange ()
    {
        CancelInvoke(); // Removes all Invoke on script
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];

    }
}
