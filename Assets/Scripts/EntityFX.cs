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
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;




    private void Start()
    {
        // Assign sr to the sprite renderer in animator, from player or enemy
        sr = GetComponentInChildren<SpriteRenderer>();

        // Save the original material to set it back after taking damage
        originalMat = sr.material;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    private IEnumerator FlashFX()
    {
        #region Multiple blink
        /*for (int i = 0; i < flashOcurrence; i++)
        {
            sr.material = hitMat;
            yield return new WaitForSeconds(flashDuration);
            sr.material = originalMat;
            yield return new WaitForSeconds(flashDuration);
        }*/
        #endregion

        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        // Return original colors and material of the entity
        sr.color = currentColor;
        sr.material = originalMat;
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
    public void ChillFxFor(float _seconds)
    {
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }
    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

   
}
