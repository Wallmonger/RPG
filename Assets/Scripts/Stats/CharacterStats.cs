using JetBrains.Annotations;
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

    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public int currentHealth;
    public System.Action onHealthChanged;
    protected bool isDead;

    
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

        if (isIgnited)
            ApplyIgniteDamage();
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
        _targetStats.TakeDamage(totalDamage);
        /*DoMagicalDamage(_targetStats);*/

        //TODO Create condition if weapon has elemental damage, DoMagicalDamage()
    }

    #region Magical damage and Ailments

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

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }
    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
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

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        // Checking conditions, shock can be cumulated
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;
            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .3f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration); 
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
/*
        isChilled = _chill;
        isShocked = _shock;*/
    }
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;
        fx.ShockFxFor(ailmentsDuration);
    }
    private void HitNearestTargetWithShockStrike()
    {
        // Will search for colliders in a circle, where _checkTransform is the center
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)  // Second condition to be sure it's another enemy
            {
                // Retrieve distance to enemy currently in the loop, compared to the _checkTransform position
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                // If the distance is less than the last occurence, register enemyPosition and new closestDistance
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }

            }

            // If there is only one enemy, target him with the shock attack
            if (closestEnemy == null)
                closestEnemy = transform;

        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgniteDamage()
    {
        // wait .3f seconds to make another damage, if ignited
        if (igniteDamageTimer < 0)
        {
            /*currentHealth -= igniteDamage;*/
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead) 
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        // Trigger death anim for entities
        if (currentHealth < 0 && !isDead)
            Die();
        
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void Die()
    {
        // Prevent death loop
        isDead = true;
    }

    #region Stat calculations

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
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
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

    #endregion
}
