using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightningDamage
}

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item Effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private float buffDuration;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        return buffType switch
        {
            StatType.strength => stats.strength,
            StatType.agility => stats.agility,
            StatType.intelligence => stats.intelligence,
            StatType.vitality => stats.vitality,
            StatType.damage => stats.damage,
            StatType.critChance => stats.critChance,
            StatType.critPower => stats.critPower,
            StatType.health => stats.maxHealth,
            StatType.armor => stats.armor,
            StatType.evasion => stats.evasion,
            StatType.magicRes => stats.magicResistance,
            StatType.fireDamage => stats.fireDamage,
            StatType.iceDamage => stats.iceDamage,
            StatType.lightningDamage => stats.lightningDamage,
            _ => null,
        };
    }
}
