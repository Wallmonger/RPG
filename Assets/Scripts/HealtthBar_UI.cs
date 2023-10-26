using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();

        // When system action onFlipped is trigger, add FlipUI
        entity.onFlipped += FlipUI;
    }

    // Prevent healthBar to flip
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
}
