using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Inventory/Effects/Buff")]
public class BuffEffect : ItemEffect
{
    [Header("Buff 设置")]
    public StatType statType;
    public ModifierType modifierType;
    public float value;
    public float duration = 10f;

    public override void ExecuteEffect(Transform target)
    {
        if (target == null) return;
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null) return;

        Stat stat = GetStat(stats, statType);
        if (stat == null) return;

        StatModifier mod = new StatModifier(modifierType, value, this);
        stat.AddModifier(mod);
        stats.RecalculateStats();

        // 自动移除
        if (duration > 0f)
            RemoveAfterDelay(target, mod, duration);
    }

    private async void RemoveAfterDelay(Transform target, StatModifier mod, float delay)
    {
        await System.Threading.Tasks.Task.Delay((int)(delay * 1000));
        if (target == null) return;
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null) return;

        Stat stat = GetStat(stats, statType);
        if (stat != null)
        {
            stat.RemoveModifier(mod);
            stats.RecalculateStats();
        }
    }

    private Stat GetStat(PlayerStats stats, StatType type)
    {
        switch (type)
        {
            case StatType.Strength: return stats.strength;
            case StatType.Agility: return stats.agility;
            case StatType.Intelligence: return stats.intelligence;
            case StatType.Vitality: return stats.vitality;
            case StatType.Damage: return stats.damage;
            case StatType.CritRate: return stats.critRate;
            case StatType.CritDamage: return stats.critDamage;
            case StatType.MaxHealth: return stats.maxHealth;
            case StatType.Armor: return stats.armor;
            case StatType.Dodge: return stats.dodge;
            case StatType.MagicResist: return stats.magicResist;
            case StatType.FireDamage: return stats.fireDamage;
            case StatType.IceDamage: return stats.iceDamage;
            case StatType.LightningDamage: return stats.lightningDamage;
            default: return null;
        }
    }
}
