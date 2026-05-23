using UnityEngine;

/// <summary>
/// 玩家属性 —— 继承自 CharacterStats。
/// 根据 STR/AGI/INT/VIT 重新计算所有派生属性。
/// </summary>
public class PlayerStats : CharacterStats
{
    #region 机械属性

    [Header("机械装置")]
    public Stat mechDamage       = new Stat(0f);    // 装置基础伤害
    public Stat mechDurability   = new Stat(100f);  // 装置基础耐久
    public Stat mechAttackSpeed  = new Stat(1f);    // 装置基础攻速
    public Stat mechRepairSpeed  = new Stat(1f);    // 装置基础修复速度
    public Stat mechRange        = new Stat(3f);    // 装置基础范围
    public int   buildLimitBase  = 3;               // 建造基础上限

    #endregion

    #region 派生属性缓存

    [HideInInspector] public float finalMaxHealth;
    [HideInInspector] public float finalDamage;
    [HideInInspector] public float finalCritRate;
    [HideInInspector] public float finalCritDamage;
    [HideInInspector] public float finalArmor;
    [HideInInspector] public float finalDodge;
    [HideInInspector] public float finalMagicResist;
    [HideInInspector] public float finalHealthRegen;
    [HideInInspector] public float finalAttackSpeed;
    [HideInInspector] public float finalWeightLimit;

    // 魔法
    [HideInInspector] public float finalFireDamage;
    [HideInInspector] public float finalIceDamage;
    [HideInInspector] public float finalLightningDamage;

    // 机械
    [HideInInspector] public float finalMechDamage;
    [HideInInspector] public float finalMechDurability;
    [HideInInspector] public float finalMechAttackSpeed;
    [HideInInspector] public float finalMechRepairSpeed;
    [HideInInspector] public float finalMechRange;
    [HideInInspector] public int   finalBuildLimit;

    #endregion

    #region 刷新派生属性

    public override void RecalculateStats()
    {
        float str = strength.Value;
        float agi = agility.Value;
        float intVal = intelligence.Value;
        float vit = vitality.Value;

        // ── 最大生命：100 + VIT×12 + 装备 ──
        maxHealth.baseValue = 100f + vit * 12f;
        finalMaxHealth = maxHealth.Value;

        // ── 物理伤害：基础伤害 × (1 + STR×0.02) ──
        finalDamage = damage.Value * (1f + str * 0.02f);

        // ── 暴击率：5% + AGI×0.3% ──
        critRate.baseValue = 0.05f + agi * 0.003f;
        finalCritRate = GetEffectiveCritRate();

        // ── 暴击伤害：150% + AGI×0.5% ──
        critDamage.baseValue = 1.5f + agi * 0.005f;
        finalCritDamage = GetEffectiveCritDamage();

        // ── 护甲：5 + STR×0.5 + VIT×0.3 + 装备 ──
        armor.baseValue = 5f + str * 0.5f + vit * 0.3f;
        finalArmor = armor.Value;

        // ── 闪避：3% + AGI×0.25%（上限 40%）──
        dodge.baseValue = 0.03f + agi * 0.0025f;
        finalDodge = GetEffectiveDodge();

        // ── 魔法抗性：2% + INT×0.2%（上限 75%）──
        magicResist.baseValue = 0.02f + intVal * 0.002f;
        finalMagicResist = GetEffectiveMagicResist();

        // ── 生命回复：1 + VIT×0.3 /秒 ──
        finalHealthRegen = 1f + vit * 0.3f;

        // ── 攻击速度：1.0 × (1 + AGI×0.01) ──
        finalAttackSpeed = 1f + agi * 0.01f;

        // ── 负重上限：50 + STR×2 ──
        finalWeightLimit = 50f + str * 2f;

        // ── 魔法伤害：基础 × (1 + INT×0.025) ──
        finalFireDamage      = fireDamage.Value      * (1f + intVal * 0.025f);
        finalIceDamage       = iceDamage.Value        * (1f + intVal * 0.025f);
        finalLightningDamage = lightningDamage.Value   * (1f + intVal * 0.025f);

        // ── 机械装置 ──
        finalMechDamage      = mechDamage.Value      * (1f + str * 0.015f);
        finalMechDurability  = mechDurability.Value   * (1f + intVal * 0.02f) * (1f + vit * 0.01f);
        finalMechAttackSpeed = mechAttackSpeed.Value  * (1f + agi * 0.008f);
        finalMechRepairSpeed = mechRepairSpeed.Value  * (1f + vit * 0.01f);
        finalMechRange       = mechRange.Value        * (1f + intVal * 0.01f);
        finalBuildLimit      = buildLimitBase + Mathf.FloorToInt(str / 10f) + Mathf.FloorToInt(intVal / 15f);

        // 同步最大生命值上限
        currentHealth = Mathf.Min(currentHealth, finalMaxHealth);
    }

    #endregion

    #region 伤害计算

    /// <summary>
    /// 计算对目标造成的最终物理伤害。
    /// </summary>
    public float CalcPhysicalDamage(CharacterStats target)
    {
        float dmg = finalDamage;

        // 暴击判定
        if (Random.value < finalCritRate)
            dmg *= finalCritDamage;

        // 目标减伤
        dmg *= (1f - target.GetPhysicalReduction());

        return dmg;
    }

    /// <summary>
    /// 计算对目标造成的最终魔法伤害。
    /// </summary>
    public float CalcMagicalDamage(CharacterStats target, float baseMagicDmg)
    {
        float dmg = baseMagicDmg * (1f + intelligence.Value * 0.025f);

        // 暴击判定
        if (Random.value < finalCritRate)
            dmg *= finalCritDamage;

        // 目标魔法减伤
        dmg *= (1f - target.GetMagicalReduction());

        return dmg;
    }

    /// <summary>
    /// 计算机械装置对目标造成的伤害。
    /// </summary>
    public float CalcMechDamage(CharacterStats target, float deviceBaseDamage, float deviceCurrentHP, float deviceMaxHP)
    {
        float dmg = deviceBaseDamage * (1f + strength.Value * 0.015f);

        // 装置暴击（继承玩家暴击的 60%/75%）
        float mechCritRate = finalCritRate * 0.6f;
        float mechCritDmg  = finalCritDamage * 0.75f;

        // AGI ≥ 40 时额外暴击率
        if (agility.Value >= 40f)
            mechCritRate += (agility.Value - 40f) * 0.0015f;

        if (Random.value < mechCritRate)
            dmg *= mechCritDmg;

        // 耐久效率衰减
        float hpRatio = deviceCurrentHP / deviceMaxHP;
        float efficiency = hpRatio > 0.75f ? 1.0f
                         : hpRatio > 0.50f ? 0.8f
                         : hpRatio > 0.25f ? 0.55f
                         : 0.3f;
        dmg *= efficiency;

        // 目标减伤
        dmg *= (1f - target.GetPhysicalReduction());

        return dmg;
    }

    #endregion
}
