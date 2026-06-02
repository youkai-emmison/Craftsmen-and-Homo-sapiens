using UnityEngine;

/// <summary>
/// 装置子弹 —— 由装置发射，碰撞敌人造成伤害。
/// 挂载在子弹预制体上。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DeviceProjectile : MonoBehaviour
{
    #region 配置

    private int damage;
    private float lifetime = 5f;
    private float timer;

    #endregion

    #region 初始化

    /// <summary>
    /// 初始化子弹。
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="direction">飞行方向</param>
    /// <param name="speed">飞行速度</param>
    public void Initialize(int damage, Vector2 direction, float speed)
    {
        this.damage = damage;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.velocity = direction.normalized * speed;
        }

        // 旋转子弹朝向飞行方向
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        timer = 0f;
    }

    #endregion

    #region 生命周期

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region 碰撞

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 命中敌人
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }

        // 命中障碍物（地面、墙壁等）
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
            return;
        }

        // 命中其他实体层（如 Boss）
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    #endregion
}
