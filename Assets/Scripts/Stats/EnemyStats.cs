using UnityEngine;

/// <summary>
/// 敌人属性 —— 继承自 CharacterStats。
/// 敌人一般直接配置派生属性值，不需要像玩家那样从主属性推算。
/// 也可选择性设置主属性来参与某些特殊计算。
/// </summary>
public class EnemyStats : CharacterStats
{
    #region 敌人特有配置

    [Header("敌人配置")]
    [SerializeField] private float xpReward = 20f;         // 击杀经验奖励

    #endregion

    #region 派生属性缓存

    [HideInInspector] public float finalMaxHealth;
    [HideInInspector] public float finalDamage;
    [HideInInspector] public float finalArmor;
    [HideInInspector] public float finalMagicResist;

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
        RecalculateStats();
    }

    #endregion

    #region 刷新派生属性

    public override void RecalculateStats()
    {
        // 敌人的派生属性直接使用 Inspector 中配置的 Stat 值
        // （装备/buff 的修改器仍通过 Stat 系统叠加）
        finalMaxHealth   = maxHealth.Value;
        finalDamage      = damage.Value;
        finalArmor       = armor.Value;
        finalMagicResist = magicResist.Value;

        currentHealth = Mathf.Min(currentHealth, finalMaxHealth);
    }

    #endregion

    #region 查询

    /// <summary>
    /// 击杀此敌人获得的经验值。
    /// </summary>
    public float GetXPReward()
    {
        return xpReward;
    }

    #endregion
}
