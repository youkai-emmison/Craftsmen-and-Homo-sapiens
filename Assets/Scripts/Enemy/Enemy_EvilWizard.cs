using System.Collections;
using UnityEngine;

/// <summary>
/// 邪恶巫师 Boss —— 继承自 Enemy。
/// 大体型 Boss，使用 LuizMelo_Evil_Wizard_2 动画资源。
///
/// AI 行为：
/// - 玩家未进入 Boss 房：idle 待机
/// - 进入激活范围后追击：近距离用 idle 动画移动，远距离用 run 动画移动
/// - 攻击时随机选择 Attack1 或 Attack2
///
/// 特殊机制：
/// - 受伤时根据玩家属性获得对应抗性 buff（物理伤害高→物抗，魔法伤害高→法抗）
/// </summary>
public class Enemy_EvilWizard : Enemy
{
    #region Boss 特有参数

    [Header("Boss 激活")]
    [SerializeField] private float bossActivateRange = 15f;     // Boss 激活范围（玩家进入后开始追击）

    [Header("移动动画切换")]
    [SerializeField] private float idleChaseRange = 5f;         // 近距离阈值：用 idle 动画移动
    [SerializeField] private float runChaseRange = 10f;         // 远距离阈值：用 run 动画移动

    [Header("抗性 Buff")]
    [SerializeField] private float resistanceBuffDuration = 15f; // 抗性 buff 持续时间
    [SerializeField] private float resistanceBuffPercent = 0.5f; // 抗性 buff 倍率（+50% 护甲/魔抗）

    #endregion

    #region 运行时状态

    private EnemyStats enemyStats;          // Boss 属性组件引用
    private PlayerStats playerStats;        // 玩家属性引用（用于抗性判断）
    private bool isBossActivated;           // Boss 是否已激活

    /// <summary>Boss 是否已被玩家激活（进入 Boss 房间）。</summary>
    public bool IsBossActivated => isBossActivated;
    private object currentBuffSource;       // 当前 buff 来源标记（防止重复叠加）
    private bool isPhysicalBuff;            // 当前 buff 是物抗还是法抗

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
        enemyStats = GetComponent<EnemyStats>();
    }

    protected override void Start()
    {
        base.Start();

        // 同步 Entity.maxHealth 与 EnemyStats.finalMaxHealth
        // Entity 和 EnemyStats 各自有独立的 maxHealth 字段，需要手动同步
        if (enemyStats != null)
        {
            enemyStats.RecalculateStats();
            maxHealth = enemyStats.finalMaxHealth;
            currentHealth = maxHealth;
        }

        // 获取玩家属性引用
        if (playerTransform != null)
            playerStats = playerTransform.GetComponent<PlayerStats>();
    }

    #endregion

    #region 状态机 —— Idle（重写）

    /// <summary>
    /// Boss Idle：等待玩家进入激活范围。
    /// 未激活时保持 idle，激活后切换到 Chase。
    /// </summary>
    protected override void UpdateIdle()
    {
        if (!isBossActivated)
        {
            // 检测玩家是否进入激活范围
            if (IsPlayerInRange(bossActivateRange))
            {
                isBossActivated = true;
                SwitchState(EnemyState.Chase);
            }
            return;
        }

        // 已激活但玩家跑出检测范围：回到 idle
        if (!IsPlayerInRange(detectionRange))
        {
            isBossActivated = false;
            SetVelocity(0f);
            return;
        }

        // 已激活且玩家在检测范围内：继续追击
        SwitchState(EnemyState.Chase);
    }

    #endregion

    #region 状态机 —— Chase（重写）

    /// <summary>
    /// Boss Chase：根据与玩家的距离切换 idle/run 动画。
    /// 近距离用 idle 动画移动，远距离用 run 动画移动。
    /// </summary>
    protected override void UpdateChase()
    {
        Transform chaseTarget = GetChaseTarget();

        if (chaseTarget == null)
        {
            SwitchState(EnemyState.Idle);
            return;
        }

        float distToTarget = Vector2.Distance(transform.position, chaseTarget.position);

        // 进入攻击范围
        if (distToTarget <= attackRange && !isAttackOnCooldown)
        {
            SwitchState(EnemyState.Attack);
            return;
        }

        // 追击方向
        float diffX = chaseTarget.position.x - transform.position.x;
        float directionX = Mathf.Abs(diffX) < 0.1f ? facingDirection : Mathf.Sign(diffX);

        // 前方没有地面：放弃追击
        if (!IsGroundAhead(directionX))
        {
            SwitchState(EnemyState.Idle);
            return;
        }

        // 根据距离选择移动速度和动画
        if (distToTarget <= idleChaseRange)
        {
            // 近距离：用 idle 动画缓慢移动
            SetVelocity(directionX * moveSpeed);
        }
        else
        {
            // 远距离：用 run 动画快速移动
            SetVelocity(directionX * moveSpeed * chaseSpeedMultiplier);
        }

        FlipController(directionX);
    }

    #endregion

    #region 状态机 —— Attack（重写）

    /// <summary>
    /// Boss Attack：随机选择 Attack1 或 Attack2。
    /// </summary>
    protected override void UpdateAttack()
    {
        // 停止移动
        SetVelocity(0f);

        // 随机选择攻击动画
        if (Random.value < 0.5f)
            SafeSetTrigger("Attack1");
        else
            SafeSetTrigger("Attack2");

        // 执行攻击判定
        PerformAttack();

        // 开始冷却
        isAttackOnCooldown = true;
        attackCooldownTimer = attackCooldown;

        // 打开反击窗口
        StartCoroutine(CounterWindowCoroutine());

        // 返回追击
        SwitchState(EnemyState.Chase);
    }

    #endregion

    #region 受伤与抗性 Buff（重写）

    /// <summary>
    /// 重写受伤逻辑：额外处理抗性 buff 机制。
    /// 受伤时比较玩家的物理/魔法伤害属性，获得对应抗性。
    /// </summary>
    public override void TakeDamage(float damage, Vector2 damageSource)
    {
        if (currentState == EnemyState.Dead) return;

        base.TakeDamage(damage, damageSource);

        // 检查并应用抗性 buff
        CheckAndApplyResistanceBuff();
    }

    /// <summary>
    /// 比较玩家属性，应用对应抗性 buff。
    /// </summary>
    private void CheckAndApplyResistanceBuff()
    {
        if (playerStats == null || enemyStats == null) return;

        // 计算玩家的物理和魔法伤害
        float playerPhysicalDmg = playerStats.finalDamage;
        float playerMagicalDmg = playerStats.finalFireDamage
                               + playerStats.finalIceDamage
                               + playerStats.finalLightningDamage;

        bool shouldBePhysicalBuff = playerPhysicalDmg >= playerMagicalDmg;

        // 如果已经是对的 buff 类型，不重复叠加
        if (currentBuffSource != null && isPhysicalBuff == shouldBePhysicalBuff)
            return;

        // 移除旧 buff
        RemoveCurrentBuff();

        // 应用新 buff
        ApplyResistanceBuff(shouldBePhysicalBuff);
    }

    /// <summary>
    /// 应用抗性 buff。
    /// </summary>
    private void ApplyResistanceBuff(bool physical)
    {
        if (enemyStats == null) return;

        Stat targetStat = physical ? enemyStats.armor : enemyStats.magicResist;

        // 创建 modifier（以 this 为 source，便于移除）
        var mod = new StatModifier(ModifierType.Percent, resistanceBuffPercent, this);
        targetStat.AddModifier(mod);
        enemyStats.RecalculateStats();

        currentBuffSource = this;
        isPhysicalBuff = physical;

        string buffType = physical ? "物抗" : "法抗";
        Debug.Log($"EvilWizard Boss: 获得 {buffType} buff (+{resistanceBuffPercent * 100}%), 持续 {resistanceBuffDuration} 秒");

        // 启动定时移除
        StartCoroutine(RemoveBuffAfterDelay(resistanceBuffDuration));
    }

    /// <summary>
    /// 移除当前抗性 buff。
    /// </summary>
    private void RemoveCurrentBuff()
    {
        if (currentBuffSource == null || enemyStats == null) return;

        // 从 armor 和 magicResist 中移除以 this 为 source 的 modifier
        enemyStats.armor.RemoveAllModifiersFromSource(this);
        enemyStats.magicResist.RemoveAllModifiersFromSource(this);
        enemyStats.RecalculateStats();

        currentBuffSource = null;
        Debug.Log("EvilWizard Boss: 抗性 buff 已移除");
    }

    /// <summary>
    /// 延迟移除 buff 的协程。
    /// </summary>
    private IEnumerator RemoveBuffAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveCurrentBuff();
    }

    #endregion

    #region 动画（重写）

    /// <summary>
    /// 重写动画更新：Boss 使用 MoveSpeed 控制 idle/run 切换。
    /// </summary>
    protected override void UpdateAnimations()
    {
        if (anim == null) return;

        SafeSetFloat("MoveSpeed", GetCurrentMoveSpeed());
        SafeSetBool("IsStunned", isStunned);
    }

    #endregion

    #region 死亡（重写）

    protected override void Die()
    {
        // 移除 buff
        RemoveCurrentBuff();

        base.Die();
    }

    #endregion

    #region Gizmos

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Boss 激活范围（白色）
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, bossActivateRange);

        // 近距离阈值（绿色）
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, idleChaseRange);

        // 远距离阈值（蓝色）
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runChaseRange);
    }

    #endregion
}
