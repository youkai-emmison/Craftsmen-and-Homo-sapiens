using UnityEngine;

/// <summary>
/// 角色属性基类。
/// 包含四大主要属性（STR/AGI/INT/VIT）及通用派生属性。
/// 子类（PlayerStats/EnemyStats）实现各自的派生计算。
/// </summary>
public class CharacterStats : MonoBehaviour
{
    #region 主要属性 (Primary Stats)

    [Header("主要属性")]
    public Stat strength     = new Stat(10f);   // 力量 STR
    public Stat agility      = new Stat(10f);   // 敏捷 AGI
    public Stat intelligence = new Stat(10f);   // 智力 INT
    public Stat vitality     = new Stat(10f);   // 活力 VIT

    #endregion

    #region 攻击属性

    [Header("攻击")]
    public Stat maxHealth       = new Stat(100f);
    public Stat damage          = new Stat(10f);
    public Stat critRate        = new Stat(0.05f);   // 5%
    public Stat critDamage      = new Stat(1.5f);    // 150%

    #endregion

    #region 防御属性

    [Header("防御")]
    public Stat armor           = new Stat(5f);
    public Stat dodge           = new Stat(0.03f);   // 3%
    public Stat magicResist     = new Stat(0.02f);   // 2%

    #endregion

    #region 魔法属性

    [Header("魔法")]
    public Stat fireDamage      = new Stat(0f);
    public Stat iceDamage       = new Stat(0f);
    public Stat lightningDamage = new Stat(0f);

    #endregion

    #region 运行时状态

    [HideInInspector] public float currentHealth;

    #endregion

    #region 生命周期

    protected virtual void Awake()
    {
        currentHealth = maxHealth.Value;
    }

    #endregion

    #region 派生属性计算（子类重写）

    /// <summary>
    /// 刷新所有派生属性。装备/升级/加点后调用。
    /// 子类必须重写以实现各自的公式。
    /// </summary>
    public virtual void RecalculateStats()
    {
        // 基类默认：子类覆盖
    }

    #endregion

    #region 通用公式

    /// <summary>
    /// 物理减伤率：armor / (armor + 100)，上限 80%。
    /// </summary>
    public float GetPhysicalReduction()
    {
        float a = armor.Value;
        return Mathf.Min(a / (a + 100f), 0.8f);
    }

    /// <summary>
    /// 魔法减伤率：magicResist / (magicResist + 100)，上限 80%。
    /// </summary>
    public float GetMagicalReduction()
    {
        float mr = magicResist.Value;
        return Mathf.Min(mr / (mr + 100f), 0.8f);
    }

    /// <summary>
    /// 暴击率（含上限 75% 溢出转化逻辑）。
    /// </summary>
    public float GetEffectiveCritRate()
    {
        return Mathf.Min(critRate.Value, 0.75f);
    }

    /// <summary>
    /// 暴击伤害（含溢出转化后的额外加成）。
    /// </summary>
    public float GetEffectiveCritDamage()
    {
        float overflow = critRate.Value - 0.75f;
        float bonus = overflow > 0f ? overflow * 0.05f : 0f;
        return critDamage.Value + bonus;
    }

    /// <summary>
    /// 闪避率（上限 40%）。
    /// </summary>
    public float GetEffectiveDodge()
    {
        return Mathf.Min(dodge.Value, 0.4f);
    }

    /// <summary>
    /// 魔法抗性（上限 75%）。
    /// </summary>
    public float GetEffectiveMagicResist()
    {
        return Mathf.Min(magicResist.Value, 0.75f);
    }

    #endregion

    #region 伤害结算

    /// <summary>
    /// 受到伤害的通用结算流程：闪避 → 暴击（由调用方判定）→ 减伤 → 扣血。
    /// 返回实际造成的伤害值。
    /// </summary>
    public float TakeDamage(float rawDamage, bool isMagical = false)
    {
        // 闪避判定
        if (Random.value < GetEffectiveDodge())
            return 0f;

        // 减伤
        float reduction = isMagical ? GetMagicalReduction() : GetPhysicalReduction();
        float finalDamage = rawDamage * (1f - reduction);

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        return finalDamage;
    }

    #endregion
}
