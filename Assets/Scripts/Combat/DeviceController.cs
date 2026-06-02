using UnityEngine;

/// <summary>
/// 机械装置运行时控制器 —— 管理耐久度、持续时间、自动攻击。
/// 挂载在装置预制体上，由 DeviceManager 创建和管理。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DeviceController : MonoBehaviour, IDamageable
{
    #region 配置

    private DeviceData data;
    private PlayerStats ownerStats;

    #endregion

    #region 运行时状态

    private int currentDurability;
    private float remainingDuration;
    private float attackTimer;
    private Transform currentTarget;
    private bool isInitialized;

    // 增强 buff
    private float damageMultiplier = 1f;
    private float durationBonus = 0f;
    private int durabilityBonus = 0;

    #endregion

    #region 公开属性

    public int CurrentDurability => currentDurability;
    public int MaxDurability => data != null ? data.maxDurability + durabilityBonus : 0;
    public float RemainingDuration => remainingDuration;
    public float TotalDuration => data != null ? data.duration + durationBonus : 0f;
    public DeviceData Data => data;
    public bool IsInitialized => isInitialized;

    #endregion

    #region 初始化

    /// <summary>
    /// 初始化装置。
    /// </summary>
    /// <param name="data">装置数据</param>
    /// <param name="ownerStats">拥有者属性</param>
    public void Initialize(DeviceData data, PlayerStats ownerStats)
    {
        this.data = data;
        this.ownerStats = ownerStats;

        currentDurability = data.maxDurability + durabilityBonus;
        remainingDuration = data.duration + durationBonus;
        attackTimer = 0f;
        isInitialized = true;

        // 注册到管理器
        if (DeviceManager.Instance != null)
            DeviceManager.Instance.RegisterDevice(data.deviceName);
    }

    #endregion

    #region 生命周期

    private void Update()
    {
        if (!isInitialized) return;

        // 更新持续时间
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0f)
        {
            Expire();
            return;
        }

        // 更新攻击计时器
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            // 寻找目标并攻击
            FindTarget();
            if (currentTarget != null)
            {
                ShootAtTarget();
                attackTimer = data.attackInterval;
            }
            else
            {
                // 没有目标时短暂等待后重试
                attackTimer = 0.2f;
            }
        }

        // 朝向目标
        if (currentTarget != null)
        {
            float dir = Mathf.Sign(currentTarget.position.x - transform.position.x);
            transform.localScale = new Vector3(dir > 0 ? 1 : -1, 1, 1);
        }
    }

    private void OnDestroy()
    {
        // 从管理器注销
        if (DeviceManager.Instance != null && data != null)
            DeviceManager.Instance.UnregisterDevice(data.deviceName);
    }

    #endregion

    #region 攻击逻辑

    /// <summary>
    /// 寻找范围内最近的敌人作为目标。
    /// </summary>
    private void FindTarget()
    {
        float range = data.attackRange;
        if (ownerStats != null)
            range = ownerStats.finalMechRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        float closestDist = float.MaxValue;
        Transform closest = null;

        foreach (var hit in hits)
        {
            // Enemy prefabs in this demo may use either an Enemy tag or Enemy layer.
            if (!IsValidTarget(hit)) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = hit.transform;
            }
        }

        currentTarget = closest;
    }

    private bool IsValidTarget(Collider2D hit)
    {
        if (hit == null) return false;
        if (hit.GetComponent<DeviceController>() != null) return false;

        int enemyLayer = LayerMask.NameToLayer("Enemy");
        bool isEnemyLayer = enemyLayer >= 0 && hit.gameObject.layer == enemyLayer;

        return hit.CompareTag("Enemy") || isEnemyLayer;
    }

    /// <summary>
    /// 向当前目标发射子弹。
    /// </summary>
    private void ShootAtTarget()
    {
        if (currentTarget == null || data.projectilePrefab == null) return;

        Vector2 dir = (currentTarget.position - transform.position).normalized;

        GameObject projObj = Instantiate(data.projectilePrefab, transform.position, Quaternion.identity);
        DeviceProjectile proj = projObj.GetComponent<DeviceProjectile>();
        if (proj != null)
        {
            // 计算伤害：基于玩家伤害的百分比
            int damage = 0;
            if (ownerStats != null)
            {
                float baseDmg = ownerStats.finalDamage * data.baseDamagePercent * damageMultiplier;
                damage = Mathf.RoundToInt(baseDmg);
            }

            proj.Initialize(damage, dir, data.projectileSpeed);
        }
    }

    #endregion

    #region 受伤（IDamageable）

    /// <summary>
    /// 装置受到伤害，扣减耐久。
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (!isInitialized) return;

        currentDurability -= damageAmount;
        if (currentDurability <= 0)
        {
            Destroy();
        }
    }

    #endregion

    #region 销毁

    /// <summary>
    /// 耐久耗尽销毁。
    /// </summary>
    private void Destroy()
    {
        // 可以在这里播放销毁特效
        Destroy(gameObject);
    }

    /// <summary>
    /// 持续时间到期销毁。
    /// </summary>
    private void Expire()
    {
        // 可以在这里播放消失特效
        Destroy(gameObject);
    }

    #endregion

    #region 增强 Buff

    /// <summary>
    /// 应用增强 buff。
    /// </summary>
    /// <param name="dmgMult">伤害倍率</param>
    /// <param name="durBonus">额外持续时间</param>
    /// <param name="durabilityBonus">额外耐久</param>
    public void ApplyBuff(float dmgMult, float durBonus, int durabilityBonus)
    {
        damageMultiplier = dmgMult;
        durationBonus = durBonus;
        this.durabilityBonus = durabilityBonus;

        // 刷新当前耐久和持续时间
        if (data != null)
        {
            currentDurability = Mathf.Min(currentDurability + durabilityBonus, MaxDurability);
            remainingDuration = Mathf.Min(remainingDuration + durBonus, TotalDuration);
        }
    }

    /// <summary>
    /// 移除增强 buff。
    /// </summary>
    public void RemoveBuff()
    {
        damageMultiplier = 1f;
        durationBonus = 0f;
        durabilityBonus = 0;

        // 确保不超过新的上限
        if (data != null)
        {
            currentDurability = Mathf.Min(currentDurability, data.maxDurability);
            remainingDuration = Mathf.Min(remainingDuration, data.duration);
        }
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        if (data == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }

    #endregion
}
