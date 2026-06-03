using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 巨浪运动体 —— 由 WaveEffect 技能生成。
/// 沿指定方向前进，检测敌人后先击退再造成伤害。
/// 使用帧动画播放水浪 sprite。
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class WaveProjectile : MonoBehaviour
{
    #region 运行时参数

    private float waveSpeed = 12f;                       // 前进速度
    private float lifetime = 0.7f;                       // 存活时间（秒）
    private Vector2 hitBoxSize = new Vector2(2f, 2.5f);  // 碰撞盒尺寸
    private float knockbackForce = 15f;                  // 击退力度
    private float knockbackDelay = 0.3f;                 // 击退后多久造成伤害
    private float skillDamageMultiplier = 1.5f;          // 技能伤害倍率
    private Sprite[] frames;                             // 水浪帧 sprite 数组
    private float frameInterval = 0.1f;                  // 每帧间隔（秒）

    #endregion

    #region 运行时状态

    private Vector2 waveDirection;       // 浪潮前进方向（normalized）
    private PlayerStats playerStats;     // 玩家属性引用
    private Vector2 waveOrigin;          // 浪潮生成位置（用于伤害来源计算）
    private LayerMask enemyLayer;        // 敌人所在层
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    // 已处理的敌人集合，防止重复命中
    private readonly HashSet<int> hitEnemies = new HashSet<int>();

    #endregion

    #region 初始化

    /// <summary>
    /// 由 WaveEffect 调用，在 Start 之前配置所有参数。
    /// </summary>
    public void Configure(Vector2 direction, PlayerStats stats, LayerMask enemyMask,
                          float speed, float life, Vector2 boxSize,
                          float kbForce, float kbDelay, float dmgMult,
                          Sprite[] spriteFrames, float frameGap)
    {
        waveDirection = direction.normalized;
        playerStats = stats;
        enemyLayer = enemyMask;
        waveOrigin = (Vector2)transform.position;

        waveSpeed = speed;
        lifetime = life;
        hitBoxSize = boxSize;
        knockbackForce = kbForce;
        knockbackDelay = kbDelay;
        skillDamageMultiplier = dmgMult;
        frames = spriteFrames;
        frameInterval = frameGap;
    }

    #endregion

    #region 生命周期

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // 配置碰撞盒（在 Configure 之后调用，确保 hitBoxSize 已更新）
        boxCollider.isTrigger = true;
        boxCollider.size = hitBoxSize;

        // 根据朝向翻转 sprite
        if (waveDirection.x < 0f && spriteRenderer != null)
        {
            spriteRenderer.flipX = true;
        }

        // 启动帧动画协程
        if (frames != null && frames.Length > 0)
        {
            StartCoroutine(AnimateFrames());
        }

        // 超时自动销毁
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // 沿方向前进
        transform.Translate(waveDirection * waveSpeed * Time.deltaTime, Space.World);
    }

    #endregion

    #region 碰撞检测

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否在敌人层
        if (((1 << other.gameObject.layer) & enemyLayer) == 0)
            return;

        // 跳过已处理的敌人
        int id = other.gameObject.GetInstanceID();
        if (hitEnemies.Contains(id))
            return;
        hitEnemies.Add(id);

        Entity entity = other.GetComponent<Entity>();
        if (entity == null)
            return;

        // 启动 击退→伤害 协程
        StartCoroutine(KnockbackThenDamage(entity));
    }

    #endregion

    #region 击退→伤害序列

    /// <summary>
    /// 先击退敌人，延迟后造成伤害。
    /// </summary>
    private IEnumerator KnockbackThenDamage(Entity entity)
    {
        // ── 第一阶段：强力击退 ──
        entity.ApplyKnockback(waveDirection, knockbackForce, knockbackDelay);

        // ── 等待击退完成 ──
        yield return new WaitForSeconds(knockbackDelay);

        // ── 第二阶段：造成伤害 ──
        if (entity == null || entity.currentHealth <= 0f)
            yield break;

        // 伤害计算：与 PlayerAttackController 保持一致
        float damage = playerStats.finalDamage * skillDamageMultiplier;

        // 暴击判定
        if (Random.value < playerStats.finalCritRate)
            damage *= playerStats.finalCritDamage;

        entity.TakeDamage(Mathf.RoundToInt(damage), waveOrigin);
    }

    #endregion

    #region 帧动画

    /// <summary>
    /// 循环播放水浪帧动画。
    /// </summary>
    private IEnumerator AnimateFrames()
    {
        int index = 0;
        while (true)
        {
            if (spriteRenderer != null && frames[index] != null)
            {
                spriteRenderer.sprite = frames[index];
            }
            index = (index + 1) % frames.Length;
            yield return new WaitForSeconds(frameInterval);
        }
    }

    #endregion
}
