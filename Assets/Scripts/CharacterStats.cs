using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength; // dmg / crit power
    public Stat agility;  // evasion / crit chances
    public Stat intelligence; // magic / magic resistance
    public Stat vitality;  // health

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;


    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;


    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;





    [SerializeField] private int currentHealth;
    
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        // Avoid Damages by evasion %
        if (TargetCanAvoidAttack(_targetStats))
            return;


        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage); 
            Debug.Log($"CRITICAL {totalDamage}!");
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        /*_targetStats.TakeDamage(totalDamage);*/
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        // Remove damage based on magic resistance
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage);

        // If there is no magical damage, skip status effects
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        // Generate random float from 0 to 1, chance to add ailment type if all boolean return false, then stop the loop
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            
            if (Random.value < .3f && _fireDamage > 0) 
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Ignited");
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Frozen");
                return;
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Shocked");
                return;
            }
        }


        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        // If character already have status effect
        if (isIgnited || isChilled || isShocked)
            return;

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

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
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();

        // Fix totalDamage between 0 and MaxValue to prevent healing from negative damage
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();


        if (Random.Range(0,100) <= totalCriticalChance)
            return true;

        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        // Getting percentage of critic
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;
       
        return Mathf.RoundToInt(critDamage);
    }
}
