using System.Collections;
using UnityEngine;

/// <summary>
/// 敌人基类 —— 继承自 Entity。
/// 提供：有限状态机（巡逻/追击/攻击/眩晕/死亡）、玩家检测、
/// 眩晕机制（受击积累触发）、反击窗口（攻击后暴露弱点）。
/// 具体敌人类型（如骷髅）继承此类并实现攻击行为。
/// </summary>
public class Enemy : Entity
{
    #region 状态机枚举

    /// <summary>
    /// 敌人 AI 状态。
    /// </summary>
    public enum EnemyState
    {
        Idle,       // 待机
        Patrol,     // 巡逻
        Chase,      // 追击玩家
        Attack,     // 攻击中
        Stunned,    // 眩晕（可被玩家反击）
        Dead        // 死亡
    }

    #endregion

    #region Inspector 参数

    [Header("AI 参数")]
    [SerializeField] protected float moveSpeed = 3f;            // 移动速度
    [SerializeField] protected float chaseSpeedMultiplier = 1.5f; // 追击速度倍率（相对 moveSpeed）
    [SerializeField] protected float detectionRange = 8f;       // 检测玩家的范围
    [SerializeField] protected float attackRange = 1.5f;        // 发动攻击的距离
    [SerializeField] protected float patrolWaitTime = 2f;       // 巡逻到达端点后的等待时间

    [Header("平台边缘检测")]
    [SerializeField] protected float edgeCheckDistance = 1f;    // 向前探测距离
    [SerializeField] protected float edgeCheckDepth = 2f;       // 向下探测深度
    [SerializeField] protected LayerMask groundLayer;           // 地面层

    [Header("攻击")]
    [SerializeField] protected float attackCooldown = 1.5f;     // 攻击冷却时间
    [SerializeField] protected Transform attackPoint;           // 攻击判定中心点
    [SerializeField] protected float attackRadius = 1f;         // 攻击判定范围
    [SerializeField] protected LayerMask playerLayer;           // 玩家所在层
    [SerializeField] protected float attackDamage = 10f;        // 攻击伤害

    [Header("眩晕")]
    [SerializeField] protected float stunDuration = 1.5f;       // 眩晕持续时间
    [SerializeField] protected float stunResistance = 30f;      // 眩晕阈值：累计受击多少伤害后触发眩晕

    [Header("反击窗口")]
    [SerializeField] protected float counterWindowDuration = 1f; // 攻击后暴露的反击窗口时长

    #endregion

    #region 运行时状态

    // 状态机
    protected EnemyState currentState = EnemyState.Idle;   // 当前状态
    protected float stateTimer;                             // 通用状态计时器

    // 玩家引用（运行时查找）
    protected Transform playerTransform;                    // 玩家 Transform 缓存

    // 攻击
    protected float attackCooldownTimer;                    // 攻击冷却计时器
    protected bool isAttackOnCooldown;                      // 攻击是否在冷却中

    // 眩晕
    protected float stunDamageAccumulator;                  // 已累积的眩晕伤害
    protected bool isStunned;                               // 是否处于眩晕状态

    // 反击窗口
    protected bool isCounterWindowOpen;                     // 反击窗口是否打开（攻击后暴露弱点时为 true）

    // 巡逻
    protected float patrolDirection = 1f;                   // 当前巡逻方向：1 = 右，-1 = 左
    protected bool isPatrolWaiting;                         // 巡逻中是否在等待转向

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        // 查找玩家
        if (PlayerManager.Instance != null)
            playerTransform = PlayerManager.Instance.GetPlayer().transform;
    }

    /// <summary>
    /// Update：驱动状态机、更新计时器。
    /// </summary>
    protected virtual void Update()
    {
        // 冷却计时
        if (isAttackOnCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0f)
                isAttackOnCooldown = false;
        }

        // 状态机驱动
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;
            case EnemyState.Patrol:
                UpdatePatrol();
                break;
            case EnemyState.Chase:
                UpdateChase();
                break;
            case EnemyState.Attack:
                UpdateAttack();
                break;
            case EnemyState.Stunned:
                UpdateStunned();
                break;
            case EnemyState.Dead:
                break;
        }

        // 动画参数
        UpdateAnimations();
    }

    #endregion

    #region 状态机 —— 状态更新

    /// <summary>
    /// 待机状态：检测到玩家则切换到追击，否则进入巡逻。
    /// </summary>
    protected virtual void UpdateIdle()
    {
        if (IsPlayerInRange(detectionRange))
        {
            SwitchState(EnemyState.Chase);
            return;
        }

        // 待机一段时间后开始巡逻
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
            SwitchState(EnemyState.Patrol);
    }

    /// <summary>
    /// 巡逻状态：沿当前方向来回行走，遇到平台边缘则停下转向。
    /// 检测到玩家则切换到追击。
    /// </summary>
    protected virtual void UpdatePatrol()
    {
        if (IsPlayerInRange(detectionRange))
        {
            SwitchState(EnemyState.Chase);
            return;
        }

        // 等待转向
        if (isPatrolWaiting)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0f)
            {
                isPatrolWaiting = false;
                patrolDirection *= -1f;
            }
            return;
        }

        // 前方没有地面（平台边缘），停下等待后转向
        if (!IsGroundAhead(patrolDirection))
        {
            isPatrolWaiting = true;
            stateTimer = patrolWaitTime;
            SetVelocity(0f);
            return;
        }

        // 沿当前方向移动
        SetVelocity(patrolDirection * moveSpeed);
        FlipController(patrolDirection);
    }

    /// <summary>
    /// 追击状态：以 1.5 倍速向玩家移动，进入攻击范围则切换到攻击。
    /// 玩家脱离检测范围或到达平台边缘则返回巡逻。
    /// </summary>
    protected virtual void UpdateChase()
    {
        if (playerTransform == null)
        {
            SwitchState(EnemyState.Idle);
            return;
        }

        // 玩家脱离检测范围
        if (!IsPlayerInRange(detectionRange))
        {
            SwitchState(EnemyState.Patrol);
            return;
        }

        // 进入攻击范围
        if (IsPlayerInRange(attackRange) && !isAttackOnCooldown)
        {
            SwitchState(EnemyState.Attack);
            return;
        }

        // 追击方向
        float directionX = Mathf.Sign(playerTransform.position.x - transform.position.x);

        // 前方没有地面（平台边缘），放弃追击
        if (!IsGroundAhead(directionX))
        {
            SwitchState(EnemyState.Patrol);
            return;
        }

        // 向玩家移动（1.5 倍速）
        SetVelocity(directionX * moveSpeed * chaseSpeedMultiplier);
        FlipController(directionX);
    }

    /// <summary>
    /// 攻击状态：执行攻击逻辑。攻击完成后进入冷却并返回追击。
    /// 子类必须重写 AttackAction() 来定义具体的攻击行为。
    /// </summary>
    protected virtual void UpdateAttack()
    {
        // 停止移动
        SetVelocity(0f);

        // 触发攻击动画（由子类实现具体攻击逻辑）
        SafeSetTrigger("Attack");

        // 攻击完成后：开始冷却，打开反击窗口
        isAttackOnCooldown = true;
        attackCooldownTimer = attackCooldown;

        StartCoroutine(CounterWindowCoroutine());

        SwitchState(EnemyState.Chase);
    }

    /// <summary>
    /// 眩晕状态：无法行动，持续一段时间后恢复。
    /// </summary>
    protected virtual void UpdateStunned()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            isStunned = false;
            stunDamageAccumulator = 0f;
            SwitchState(EnemyState.Chase);
        }
    }

    #endregion

    #region 状态机 —— 状态切换

    /// <summary>
    /// 切换到新状态，重置相关计时器。
    /// </summary>
    /// <param name="newState">目标状态</param>
    protected virtual void SwitchState(EnemyState newState)
    {
        // 离开当前状态的清理
        if (currentState == EnemyState.Patrol)
        {
            isPatrolWaiting = false;
        }

        currentState = newState;

        // 进入新状态的初始化
        switch (newState)
        {
            case EnemyState.Idle:
                stateTimer = patrolWaitTime;
                SetVelocity(0f);
                break;
            case EnemyState.Stunned:
                isStunned = true;
                stateTimer = stunDuration;
                SetVelocity(0f);
                SafeSetTrigger("Stunned");
                break;
            case EnemyState.Dead:
                SetVelocity(0f);
                Die();
                break;
        }
    }

    #endregion

    #region 受伤（重写基类）

    /// <summary>
    /// 重写受伤逻辑：额外处理眩晕累积和反击窗口判定。
    /// </summary>
    public override void TakeDamage(float damage, Vector2 damageSource)
    {
        if (currentState == EnemyState.Dead) return;

        base.TakeDamage(damage, damageSource);

        // 眩晕累积
        stunDamageAccumulator += damage;
        if (stunDamageAccumulator >= stunResistance && !isStunned)
        {
            SwitchState(EnemyState.Stunned);
        }

        // 反击窗口期间受伤：受到额外伤害（暴击）
        if (isCounterWindowOpen)
        {
            // 反击成功：额外扣除 50% 伤害
            currentHealth -= damage * 0.5f;
            OnCounterHit();
        }
    }

    #endregion

    #region 攻击判定

    /// <summary>
    /// 执行攻击判定：在攻击点范围内检测玩家并造成伤害。
    /// 由动画事件或子类调用。
    /// </summary>
    protected virtual void PerformAttack()
    {
        if (attackPoint == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            Entity target = hit.GetComponent<Entity>();
            if (target != null)
            {
                target.TakeDamage(attackDamage, transform.position);
            }
        }
    }

    #endregion

    #region 眩晕与反击窗口

    /// <summary>
    /// 反击窗口协程：攻击后暴露弱点一段时间。
    /// </summary>
    protected IEnumerator CounterWindowCoroutine()
    {
        isCounterWindowOpen = true;
        SafeSetBool("CounterWindowOpen", true);

        yield return new WaitForSeconds(counterWindowDuration);

        isCounterWindowOpen = false;
        SafeSetBool("CounterWindowOpen", false);
    }

    /// <summary>
    /// 反击命中时的回调。子类可重写以实现特殊效果（如眩晕延长、额外掉落等）。
    /// </summary>
    protected virtual void OnCounterHit()
    {
        // 默认空实现，子类按需扩展
    }

    #endregion

    #region 死亡

    protected override void Die()
    {
        // 停止所有协程，避免死后仍执行反击窗口等逻辑
        StopAllCoroutines();

        SafeSetTrigger("Die");

        // 禁用碰撞，防止死后仍与玩家交互
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1f);
    }

    #endregion

    #region 工具方法

    /// <summary>
    /// 检测玩家是否在指定范围内。
    /// </summary>
    protected bool IsPlayerInRange(float range)
    {
        if (playerTransform == null) return false;
        return Vector2.Distance(transform.position, playerTransform.position) <= range;
    }

    /// <summary>
    /// 检测指定方向前方是否有地面（防止走到平台边缘）。
    /// 从前方底部向下发射射线，如果没有命中地面则表示前方是悬崖。
    /// </summary>
    protected bool IsGroundAhead(float directionX)
    {
        // 起点：前方底部
        Vector2 origin = (Vector2)transform.position
            + Vector2.right * directionX * edgeCheckDistance
            + Vector2.down * 0.5f;
        return Physics2D.Raycast(origin, Vector2.down, edgeCheckDepth, groundLayer);
    }

    #endregion

    #region 动画

    /// <summary>
    /// 更新 Animator 参数。
    /// </summary>
    protected virtual void UpdateAnimations()
    {
        if (anim == null) return;

        SafeSetFloat("MoveSpeed", GetCurrentMoveSpeed());
        SafeSetBool("IsStunned", isStunned);
    }

    #endregion

    #region Gizmos 辅助

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 检测范围（黄色）
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 攻击范围（红色）
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 攻击判定范围（洋红色）
        if (attackPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        // 平台边缘检测（青色射线，左右各一条）
        Gizmos.color = Color.cyan;
        Vector2 leftOrigin = (Vector2)transform.position + Vector2.right * -edgeCheckDistance + Vector2.down * 0.5f;
        Vector2 rightOrigin = (Vector2)transform.position + Vector2.right * edgeCheckDistance + Vector2.down * 0.5f;
        Gizmos.DrawLine(leftOrigin, leftOrigin + Vector2.down * edgeCheckDepth);
        Gizmos.DrawLine(rightOrigin, rightOrigin + Vector2.down * edgeCheckDepth);
    }

    #endregion
}
