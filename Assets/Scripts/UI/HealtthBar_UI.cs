using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtthBar_UI : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        //TODO Fix execution order in projectSettings/ScriptExecutionOrder
        UpdateHealthUI();


        // When system action onFlipped is trigger, add FlipUI
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

    }

    // Transform slider
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    // Prevent healthBar to flip
    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    // When entity is dead, unsubscribe event
    private void OnDisabled() {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
