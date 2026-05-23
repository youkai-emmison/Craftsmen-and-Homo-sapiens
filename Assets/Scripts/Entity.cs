using System.Collections;
using UnityEngine;

/// <summary>
/// Entity 基类 —— 所有可交互实体（玩家、敌人等）的公共父类。
/// 提供：组件缓存、生命值管理、朝向翻转、受伤击退、速度控制。
/// 子类只需关注自身特有逻辑（如移动、AI）。
/// </summary>
public class Entity : MonoBehaviour
{
    #region 组件引用（Inspector 面板）

    [Header("基础组件")]
    [SerializeField] protected Animator anim;           // 动画控制器
    [SerializeField] protected SpriteRenderer spriteRenderer; // 精灵渲染器

    #endregion

    #region 生命值

    [Header("生命值")]
    public float maxHealth = 100f;       // 最大生命值
    public float currentHealth;          // 当前生命值

    #endregion

    #region 击退参数

    [Header("击退")]
    [SerializeField] protected float knockbackDuration = 0.15f; // 击退持续时间（秒）
    protected bool isKnockedBack;       // 是否正在被击退中

    #endregion

    #region 运行时状态

    protected Rigidbody2D rb;           // 刚体组件（物理移动）
    protected int facingDirection = 1;   // 朝向：1 = 右，-1 = 左
    protected bool isFacingRight = true; // 是否朝右（用于翻转判断）
    protected float defaultMoveSpeed;    // 记录初始速度，用于速度修改后恢复

    #endregion

    #region 生命周期

    /// <summary>
    /// Awake：初始化组件引用和基础数值。
    /// 子类重写时必须调用 base.Awake()。
    /// </summary>
    protected virtual void Awake()
    {
        // 优先尝试从 Inspector 拖拽获取，未拖拽则自动查找
        rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    /// <summary>
    /// Start：记录初始速度。子类重写时必须调用 base.Start()。
    /// </summary>
    protected virtual void Start()
    {
        defaultMoveSpeed = GetCurrentMoveSpeed();
    }

    #endregion

    #region 朝向翻转

    /// <summary>
    /// 根据传入的水平速度翻转实体朝向。
    /// 正值朝右，负值朝左。仅在朝向发生变化时执行翻转。
    /// </summary>
    /// <param name="xVelocity">当前水平速度</param>
    protected void FlipController(float xVelocity)
    {
        // 速度为 0 时保持当前朝向，避免抖动
        if (xVelocity > 0 && !isFacingRight)
            Flip();
        else if (xVelocity < 0 && isFacingRight)
            Flip();
    }

    /// <summary>
    /// 执行翻转：翻转 SpriteRenderer 的 X 轴，更新朝向状态。
    /// </summary>
    protected virtual void Flip()
    {
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    #endregion

    #region 速度控制

    /// <summary>
    /// 设置刚体速度的 X 分量，保持 Y 分量不变。
    /// 用于移动类操作。
    /// </summary>
    /// <param name="xVelocity">目标水平速度</param>
    protected void SetVelocity(float xVelocity)
    {
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    /// <summary>
    /// 同时设置水平和垂直速度。用于冲刺、击退等特殊移动。
    /// </summary>
    protected void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    /// <summary>
    /// 获取当前水平速度的绝对值，用于动画参数等。
    /// </summary>
    protected float GetCurrentMoveSpeed()
    {
        return Mathf.Abs(rb.velocity.x);
    }

    #endregion

    #region 受伤与死亡

    /// <summary>
    /// 受伤接口。扣除生命值，触发击退，播放受击动画。
    /// 子类可重写以添加额外逻辑（如无敌帧、受击音效等）。
    /// </summary>
    /// <param name="damage">受到的伤害值</param>
    /// <param name="damageSource">伤害来源位置，用于计算击退方向</param>
    public virtual void TakeDamage(float damage, Vector2 damageSource)
    {
        if (currentHealth <= 0f) return; // 已死亡不再处理

        currentHealth -= damage;

        // 从伤害来源方向施加击退
        Vector2 knockbackDir = ((Vector2)transform.position - damageSource).normalized;
        StartCoroutine(KnockbackCoroutine(knockbackDir));

        // 触发受击动画
        if (anim != null)
            anim.SetTrigger("Hit");

        if (currentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// 击退协程：施加击退力，在持续时间内禁止其他移动输入。
    /// </summary>
    protected IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;
        // 击退力 = 方向 * 速度（使用刚体当前质量做适当缩放）
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * 8f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    /// <summary>
    /// 死亡处理。默认行为：播放死亡动画后销毁物体。
    /// 子类应重写以实现各自的死亡逻辑（如掉落物、经验等）。
    /// </summary>
    protected virtual void Die()
    {
        if (anim != null)
            anim.SetTrigger("Die");

        // 延迟销毁，让死亡动画有时间播放
        Destroy(gameObject, 0.5f);
    }

    #endregion

    #region 动画事件

    /// <summary>
    /// 由动画事件调用，用于在特定动画帧触发逻辑（如攻击判定结束）。
    /// 子类按需重写。
    /// </summary>
    protected virtual void AnimationTrigger()
    {
        // 默认空实现，由子类填充
    }

    #endregion

    #region 碰撞检测（基础）

    /// <summary>
    /// 检测实体是否接触地面。依赖 Layer 设置。
    /// 子类可直接调用或重写以自定义地面检测逻辑。
    /// </summary>
    /// <param name="groundLayer">地面层的 LayerMask</param>
    /// <returns>是否站在地面上</returns>
    protected bool IsGroundDetected(LayerMask groundLayer)
    {
        // 从实体底部向下发射短射线检测地面
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        return Physics2D.Raycast(origin, Vector2.down, 0.1f, groundLayer);
    }

    #endregion

    #region Gizmos 辅助

    /// <summary>
    /// 在 Scene 视图中绘制朝向指示，方便调试。
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        // 朝向指示箭头
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDirection * 1.5f);
    }

    #endregion
}
