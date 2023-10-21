using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength; // dmg / crit power
    public Stat agility;  // evasion / crit chances
    public Stat intelligence; // magic / magic resistance
    public Stat vitality;  // health

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;


    public Stat damage;


    [SerializeField] private int currentHealth;
    
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        // Avoid Damages by evasion %
        if (TargetCanAvoidAttack(_targetStats))
            return;


        int totalDamage = damage.GetValue() + strength.GetValue();



        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();

        // Fix totalDamage between 0 and MaxValue to prevent healing from negative damage
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        // Trigger death anim for entities
        if (currentHealth <= 0)
            Die();
        
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died");
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
  
        if (Random.Range(0, 100) < totalEvasion)
            return true;
        
        return false;
    }
}
