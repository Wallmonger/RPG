using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

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


    public bool isIgnited; // Does damage over time
    public bool isChilled; // Armor -20%
    public bool isShocked; // Accuracy -20%

    [SerializeField] private float ailmentsDuration = 4;

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;   // Tick damage cooldown
    private float igniteDamageTimer;            // Tick damage clock
    private int igniteDamage;


    public int currentHealth;
    public System.Action onHealthChanged;
    
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        // Remove ignite ailment if cooldown is reached
        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        // wait .3f seconds to make another damage, if ignited
        if (igniteDamageTimer < 0 && isIgnited)
        {
            /*currentHealth -= igniteDamage;*/
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
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
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
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

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = ailmentsDuration;
            fx.ShockFxFor(ailmentsDuration);
        }

        isChilled = _chill;
        isShocked = _shock;
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

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

        // Add more chance of avoiding attack if target entity is shocked
        if (isShocked)
            totalEvasion += 20;
        
  
        if (Random.Range(0, 100) < totalEvasion)
            return true;
        
        return false;
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        // Reduce armor by 20% if chilled
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else 
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


    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5; // Each point in vitality gives 5 hp
    }
}
