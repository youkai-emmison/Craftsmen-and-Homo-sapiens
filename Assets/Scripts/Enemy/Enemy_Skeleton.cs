using System.Collections;
using UnityEngine;

/// <summary>
/// 骷髅敌人 —— 继承自 Enemy。
/// 最基础的近战敌人类型，行为模式：
/// - 巡逻时缓慢来回走动
/// - 发现玩家后追击
/// - 靠近后挥刀攻击
/// - 攻击后暴露反击窗口
/// - 受到足够伤害后眩晕
/// </summary>
public class Enemy_Skeleton : Enemy
{
    #region 骷髅特有参数

    [Header("骷髅特有")]
    [SerializeField] private float skeletonAttackDamage = 15f;  // 骷髅攻击伤害
    [SerializeField] private int attackComboMax = 2;            // 最大连击次数
    [SerializeField] private float comboResetTime = 2f;         // 连击重置时间

    #endregion

    #region 运行时状态

    private int attackComboCount;       // 当前连击计数
    private float comboResetTimer;      // 连击重置计时器

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();

        // 骷髅默认参数覆盖（如果 Inspector 未手动设置）
        // 这些值可通过 Inspector 面板覆盖
        if (attackDamage == 10f) // 仅在使用基类默认值时覆盖
            attackDamage = skeletonAttackDamage;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        // 连击重置计时
        if (attackComboCount > 0)
        {
            comboResetTimer -= Time.deltaTime;
            if (comboResetTimer <= 0f)
                attackComboCount = 0;
        }
    }

    #endregion

    #region 攻击行为（重写）

    /// <summary>
    /// 重写攻击更新：实现骷髅的连击逻辑。
    /// 如果在攻击范围内且连击未达上限，连续攻击。
    /// </summary>
    protected override void UpdateAttack()
    {
        // 停止移动
        SetVelocity(0f);

        // 执行攻击
        if (anim != null)
            anim.SetTrigger("Attack");

        // 连击计数
        attackComboCount++;
        comboResetTimer = comboResetTime;

        // 攻击冷却：连击结束后进入冷却
        if (attackComboCount >= attackComboMax)
        {
            isAttackOnCooldown = true;
            attackCooldownTimer = attackCooldown;
            attackComboCount = 0;
        }

        // 打开反击窗口
        StartCoroutine(CounterWindowCoroutine());

        // 攻击后返回追击状态
        SwitchState(EnemyState.Chase);
    }

    #endregion

    #region 反击回调（重写）

    /// <summary>
    /// 骷髅被反击命中时：延长眩晕时间，给玩家更多输出窗口。
    /// </summary>
    protected override void OnCounterHit()
    {
        // 反击成功时眩晕时间延长
        if (isStunned)
        {
            stateTimer += 0.5f;
        }
    }

    #endregion

    #region 动画事件

    /// <summary>
    /// 由攻击动画的关键帧调用，触发实际攻击判定。
    /// 在 Animator 的攻击动画中设置事件调用此方法。
    /// </summary>
    private void SkeletonAttackHit()
    {
        PerformAttack();
    }

    #endregion
}
