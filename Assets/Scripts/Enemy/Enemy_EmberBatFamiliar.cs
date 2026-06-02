using UnityEngine;

// Flying enemy variant that uses the ember bat familiar visual.
// Key variables:
// - hoverAmplitude: Vertical idle/patrol wave height.
// - hoverFrequency: Vertical idle/patrol wave speed.
// - verticalChaseSpeed: How quickly the flying enemy adjusts height while chasing.
public class Enemy_EmberBatFamiliar : Enemy
{
    [Header("Flying Movement")]
    [SerializeField] private float hoverAmplitude = 0.35f;
    [SerializeField] private float hoverFrequency = 3f;
    [SerializeField] private float verticalChaseSpeed = 2.2f;

    private float startHeight;

    protected override void Awake()
    {
        base.Awake();

        // Flying enemies should not fall off platforms during patrol.
        if (rb != null)
            rb.gravityScale = 0f;
    }

    protected override void Start()
    {
        base.Start();
        startHeight = transform.position.y;
    }

    protected override void UpdatePatrol()
    {
        if (IsPlayerInRange(detectionRange))
        {
            SwitchState(EnemyState.Chase);
            return;
        }

        float hoverVelocity = GetHoverVelocity();
        SetVelocity(patrolDirection * moveSpeed, hoverVelocity);
        FlipController(patrolDirection);
    }

    protected override void UpdateChase()
    {
        if (playerTransform == null)
        {
            SwitchState(EnemyState.Idle);
            return;
        }

        if (!IsPlayerInRange(detectionRange))
        {
            SwitchState(EnemyState.Patrol);
            return;
        }

        if (IsPlayerInRange(attackRange) && !isAttackOnCooldown)
        {
            SwitchState(EnemyState.Attack);
            return;
        }

        Vector2 chaseVelocity = GetChaseVelocity();
        SetVelocity(chaseVelocity.x, chaseVelocity.y);
        FlipController(chaseVelocity.x);
    }

    private float GetHoverVelocity()
    {
        float targetHeight = startHeight + Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        return (targetHeight - transform.position.y) * hoverFrequency;
    }

    private Vector2 GetChaseVelocity()
    {
        Vector2 direction = playerTransform.position - transform.position;
        float horizontalDirection = Mathf.Abs(direction.x) < 0.1f ? facingDirection : Mathf.Sign(direction.x);
        float verticalDirection = Mathf.Clamp(direction.y, -1f, 1f);

        return new Vector2(
            horizontalDirection * moveSpeed * chaseSpeedMultiplier,
            verticalDirection * verticalChaseSpeed);
    }
}
