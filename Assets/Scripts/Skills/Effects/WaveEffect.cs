using UnityEngine;

/// <summary>
/// 浪涛回天效果 —— 向玩家面向方向释放一道巨浪。
/// 巨浪先击退敌人，再造成基于玩家属性的物理伤害。
/// </summary>
[CreateAssetMenu(fileName = "WaveEffect", menuName = "Skills/Wave Effect")]
public class WaveEffect : SkillEffect
{
    #region 配置

    [Header("浪潮参数")]
    public float spawnForwardOffset = 1.2f;             // 生成位置距玩家的前方偏移
    public float waveSpeed = 12f;                       // 前进速度
    public float lifetime = 0.7f;                       // 存活时间（秒）
    public Vector2 hitBoxSize = new Vector2(2f, 2.5f);  // 碰撞盒尺寸

    [Header("击退")]
    public float knockbackForce = 15f;                  // 击退力度
    public float knockbackDelay = 0.3f;                 // 击退后多久造成伤害

    [Header("伤害")]
    public float skillDamageMultiplier = 1.5f;          // 技能伤害倍率（基于 finalDamage）

    [Header("动画")]
    public Sprite[] frames;                             // 水浪帧 sprite 数组
    public float frameInterval = 0.1f;                  // 每帧间隔（秒）

    [Header("检测")]
    public LayerMask enemyLayer;                        // 敌人所在层

    [Header("视觉")]
    public float spriteScale = 2f;                      // sprite 缩放倍率
    public int sortingOrder = 10;                       // 渲染排序

    #endregion

    #region SkillEffect 实现

    public override bool Activate(Transform caster, SkillManager manager)
    {
        // ── 获取朝向 ──
        var facingCtrl = manager.GetComponent<PlayerAttackFacingController>();
        float facingX = facingCtrl != null ? facingCtrl.FacingDirectionX : 1f;

        // ── 获取玩家属性 ──
        PlayerStats stats = caster.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogWarning("WaveEffect: 施法者没有 PlayerStats 组件。");
            return false;
        }

        // ── 获取玩家碰撞体高度，用于碰撞盒 ──
        float playerColliderHeight = 1.4f; // 默认值
        var playerCollider = caster.GetComponent<CapsuleCollider2D>();
        if (playerCollider != null)
            playerColliderHeight = playerCollider.size.y;

        // ── 计算生成位置 ──
        Vector2 waveDir = new Vector2(facingX, 0f).normalized;

        // 玩家脚底位置（碰撞体中心 - 半高）
        float playerBottomY = caster.position.y;
        if (playerCollider != null)
            playerBottomY = caster.position.y + playerCollider.offset.y - playerCollider.size.y * 0.5f;

        // sprite 底部贴地：sprite 半高 = (64px / 64ppu) * spriteScale / 2
        float spriteWorldHeight = spriteScale; // 1 * spriteScale
        float spawnY = playerBottomY + spriteWorldHeight * 0.5f;

        Vector3 spawnPos = new Vector3(
            caster.position.x + waveDir.x * spawnForwardOffset,
            spawnY,
            0f
        );

        // ── 创建浪潮 GameObject ──
        GameObject waveObj = new GameObject("WaveProjectile");
        waveObj.transform.position = spawnPos;
        waveObj.transform.localScale = Vector3.one * spriteScale;

        // 添加 SpriteRenderer
        SpriteRenderer sr = waveObj.AddComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0)
        {
            sr.sprite = frames[0];
        }
        sr.sortingOrder = sortingOrder;

        // 添加碰撞体（高度匹配玩家）
        BoxCollider2D col = waveObj.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        Vector2 actualHitBox = new Vector2(hitBoxSize.x, playerColliderHeight / spriteScale);
        col.size = actualHitBox;

        // 添加刚体（Kinematic，不参与物理模拟）
        Rigidbody2D rb = waveObj.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 添加并配置 WaveProjectile
        WaveProjectile wave = waveObj.AddComponent<WaveProjectile>();
        wave.Configure(
            waveDir, stats, enemyLayer,
            waveSpeed, lifetime, actualHitBox,
            knockbackForce, knockbackDelay, skillDamageMultiplier,
            frames, frameInterval
        );

        Debug.Log($"WaveEffect: 浪涛回天已释放，方向 {waveDir}");
        return true;
    }

    #endregion
}
