using System.Collections;
using UnityEngine;

/// <summary>
/// 玩家控制器 —— 继承自 Entity。
/// 负责：水平移动、跳跃（含土狼时间）、冲刺。
/// 输入读取放在 Update，物理移动放在 FixedUpdate。
/// </summary>
public class Player : Entity
{
    #region 移动参数

    [Header("移动")]
    [SerializeField] private float moveSpeed = 8f;       // 水平移动速度

    #endregion

    #region 跳跃参数

    [Header("跳跃")]
    [SerializeField] private float jumpForce = 12f;      // 跳跃初速度
    [SerializeField] private float coyoteTime = 0.1f;    // 土狼时间：离开地面后仍可跳跃的宽限时间
    [SerializeField] private float jumpBufferTime = 0.1f; // 跳跃缓冲：提前按下跳跃键的缓存时间
    [SerializeField] private float jumpGroundGraceTime = 0.15f; // 起跳后忽略地面检测的宽限时间

    #endregion

    #region 冲刺参数

    [Header("冲刺")]
    [SerializeField] private float dashSpeed = 20f;       // 冲刺速度
    [SerializeField] private float dashDuration = 0.2f;   // 冲刺持续时间
    [SerializeField] private float dashCooldown = 1f;     // 冲刺冷却时间

    #endregion

    #region 滑墙参数

    [Header("滑墙")]
    [SerializeField] private float wallSlideSpeed = 2f;       // 滑墙时的下落速度

    #endregion

    #region 运行时状态

    // 输入
    private float inputX;                  // 水平输入值（-1 到 1）
    private bool jumpPressed;              // 本帧是否按下跳跃键

    // 跳跃状态
    private float coyoteTimer;             // 土狼时间计时器
    private float jumpBufferTimer;         // 跳跃缓冲计时器
    private float jumpGroundGraceTimer;    // 起跳宽限计时器
    private bool isGrounded;               // 是否站在地面上

    // 冲刺状态
    private bool isDashing;                // 是否正在冲刺
    private bool canDash = true;           // 冲刺是否已冷却完毕
    private float dashCooldownTimer;       // 冲刺冷却计时器

    // 滑墙状态
    private bool isTouchingWall;           // 本帧是否接触墙面
    private bool isWallSliding;            // 是否正在滑墙

    // 检测组件
    private GroundCheck groundCheck;
    private WallCheck wallCheck;

    // 动画驱动
    private MaidVisualAnimatorDriver visualAnimatorDriver;

    // 装备特效
    private MeleeHitDetector meleeHitDetector;
    private ItemEffectManager itemEffectManager;

    #endregion

    #region 生命周期

    protected override void Awake()
    {
        base.Awake();
        // Visual-only child animators use their own driver, so Player movement does not push missing parameters into imported controllers.
        anim = null;
        rb = GetComponent<Rigidbody2D>();
        rb.sharedMaterial = new PhysicsMaterial2D("NoFriction") { friction = 0f };
        visualAnimatorDriver = GetComponentInChildren<MaidVisualAnimatorDriver>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        wallCheck = GetComponentInChildren<WallCheck>();
        meleeHitDetector = GetComponentInChildren<MeleeHitDetector>();
        itemEffectManager = GetComponent<ItemEffectManager>();
    }

    private void OnEnable()
    {
        if (meleeHitDetector != null)
            meleeHitDetector.OnHitTarget += OnMeleeHit;
    }

    private void OnDisable()
    {
        if (meleeHitDetector != null)
            meleeHitDetector.OnHitTarget -= OnMeleeHit;
    }

    private void OnMeleeHit(Transform target)
    {
        if (itemEffectManager != null)
            itemEffectManager.OnAttackHit(target);
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float damage, Vector2 damageSource)
    {
        base.TakeDamage(damage, damageSource);

        if (visualAnimatorDriver != null)
            visualAnimatorDriver.PlayHurt();

        // 触发护甲特效
        if (itemEffectManager != null)
            itemEffectManager.OnDamaged(transform);
    }

    protected override void Die()
    {
        RaiseOnDeath();

        if (visualAnimatorDriver != null)
            visualAnimatorDriver.PlayDie();

        Destroy(gameObject, 0.5f);
    }

    /// <summary>
    /// Update：每帧读取输入、更新计时器、处理跳跃和冲刺触发。
    /// </summary>
    private void Update()
    {
        // 被击退时忽略输入
        if (isKnockedBack) return;

        // ── 读取输入 ──
        inputX = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetKeyDown(KeyCode.Space);

        // ── 地面检测 ──
        jumpGroundGraceTimer -= Time.deltaTime;
        isGrounded = jumpGroundGraceTimer > 0f ? false : groundCheck != null && groundCheck.IsGrounded();

        // ── 墙面检测与滑墙 ──
        isTouchingWall = wallCheck != null && wallCheck.IsWallDetected(facingDirection);
        isWallSliding = isTouchingWall && !isGrounded && rb.velocity.y < 0;

        // ── 更新土狼时间 ──
        // 站在地面上时重置计时器；离开地面后开始倒计时
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // ── 更新跳跃缓冲 ──
        if (jumpPressed)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // ── 冲刺冷却 ──
        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
                canDash = true;
        }

        // ── 触发跳跃 ──
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            Jump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // ── 触发冲刺 ──
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }

        // ── 动画参数 ──
        UpdateAnimations();
    }

    /// <summary>
    /// FixedUpdate：执行物理移动。
    /// </summary>
    private void FixedUpdate()
    {
        // 被击退或冲刺中时，不覆盖速度
        if (isKnockedBack || isDashing) return;

        // 水平移动
        SetVelocity(inputX * moveSpeed);

        // 滑墙时限制下落速度
        if (isWallSliding)
            SetVelocity(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));

        // 朝向翻转
        FlipController(inputX);
    }

    #endregion

    #region 朝向翻转

    /// <summary>
    /// 禁用 Entity 基类的 spriteRenderer.flipX 翻转。
    /// MaidVisual 的翻转由 PlayerAttackFacingController 通过 localScale.x 控制，
    /// 两套机制同时运行会互相覆盖导致翻转失效。
    /// </summary>
    protected override void Flip()
    {
        // 仅更新朝向状态，不操作 spriteRenderer.flipX
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
    }

    #endregion

    #region 跳跃

    /// <summary>
    /// 执行跳跃：给刚体施加向上速度。
    /// </summary>
    private void Jump()
    {
        // 先清空 Y 轴速度，确保从地面起跳时高度一致
        SetVelocity(rb.velocity.x, jumpForce);
        jumpGroundGraceTimer = jumpGroundGraceTime;

        if (visualAnimatorDriver != null)
            visualAnimatorDriver.PlayJump();
    }

    #endregion

    #region 冲刺

    /// <summary>
    /// 冲刺协程：在短时间内高速移动，期间忽略重力和输入。
    /// </summary>
    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;
        dashCooldownTimer = dashCooldown;

        // 冲刺方向：有输入时跟随输入，无输入时跟随当前朝向
        float dashDir = inputX != 0 ? inputX : facingDirection;

        // 冲刺期间：关闭重力，设置高速度
        rb.gravityScale = 0f;
        SetVelocity(dashDir * dashSpeed, 0f);

        if (anim != null)
            anim.SetTrigger("Dash");

        yield return new WaitForSeconds(dashDuration);

        // 冲刺结束：恢复重力，清空水平速度
        rb.gravityScale = 3f;
        SetVelocity(0f, rb.velocity.y);
        isDashing = false;
    }

    #endregion


    #region 动画

    /// <summary>
    /// 更新 Animator 参数，驱动动画状态机。
    /// </summary>
    private void UpdateAnimations()
    {
        if (visualAnimatorDriver == null) return;

        visualAnimatorDriver.SetGrounded(isGrounded);
        visualAnimatorDriver.SetWallSliding(isWallSliding);
    }

    #endregion

}
