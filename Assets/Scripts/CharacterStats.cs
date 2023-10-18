using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat maxHealth;


    [SerializeField] private int currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth.GetValue();

        // Stuff modifier
        damage.AddModifier(4);
        
    }

    public void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died");
    }
}
