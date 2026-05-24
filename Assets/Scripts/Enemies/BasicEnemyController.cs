// Script purpose: Controls BasicEnemy patrol, chase, attack, and dead states.
// Key Inspector variables:
// - playerTarget / playerHealth: Player references assigned manually.
// - patrolLeftPoint / patrolRightPoint: Manual patrol limits.
// - enemyAttackController / enemyHealth: Required sibling behavior components.
// - patrolSpeed / chaseSpeed: Horizontal movement speeds.
// - detectionRange / attackRange: Distances used for state transitions.
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    // Player transform assigned in the Inspector.
    public Transform playerTarget;

    // Player health assigned in the Inspector for attack calls.
    public PlayerHealth playerHealth;

    // Left patrol limit for the enemy.
    public Transform patrolLeftPoint;

    // Right patrol limit for the enemy.
    public Transform patrolRightPoint;

    // Attack component that applies cooldown-gated damage.
    public EnemyAttackController enemyAttackController;

    // Health component used to stop behavior after defeat.
    public EnemyHealth enemyHealth;

    // Movement speed while patrolling.
    public float patrolSpeed = 2f;

    // Movement speed while chasing the player.
    public float chaseSpeed = 3f;

    // Distance where the enemy switches from patrol to chase.
    public float detectionRange = 4f;

    // Distance where the enemy stops moving and attacks.
    public float attackRange = 0.8f;

    // Rigidbody2D moved in FixedUpdate.
    private Rigidbody2D enemyRigidbody;

    // Current patrol destination.
    private Transform currentPatrolTarget;

    // Current finite state for this simple enemy.
    private EnemyState currentState = EnemyState.Patrol;

    // Read-only state for debugging.
    public EnemyState CurrentState => currentState;

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        currentPatrolTarget = patrolRightPoint;
        ValidateRequiredReferences();
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDefeated += HandleEnemyDefeated;
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDefeated -= HandleEnemyDefeated;
        }
    }

    private void Update()
    {
        if (!HasRequiredReferences() || currentState == EnemyState.Dead)
        {
            return;
        }

        UpdateStateFromPlayerDistance();
        TryAttackPlayer();
    }

    private void FixedUpdate()
    {
        if (!HasRequiredReferences())
        {
            return;
        }

        MoveForCurrentState();
    }

    private void ValidateRequiredReferences()
    {
        if (enemyRigidbody == null)
        {
            Debug.LogError("BasicEnemyController: This GameObject needs a Rigidbody2D component.", this);
        }

        if (playerTarget == null)
        {
            Debug.LogError("BasicEnemyController: Player Target is not assigned.", this);
        }

        if (playerHealth == null)
        {
            Debug.LogError("BasicEnemyController: Player Health is not assigned.", this);
        }

        if (patrolLeftPoint == null || patrolRightPoint == null)
        {
            Debug.LogError("BasicEnemyController: Patrol Left Point and Patrol Right Point must be assigned.", this);
        }

        if (enemyAttackController == null)
        {
            Debug.LogError("BasicEnemyController: Enemy Attack Controller is not assigned.", this);
        }

        if (enemyHealth == null)
        {
            Debug.LogError("BasicEnemyController: Enemy Health is not assigned.", this);
        }
    }

    private bool HasRequiredReferences()
    {
        return enemyRigidbody != null
            && playerTarget != null
            && playerHealth != null
            && patrolLeftPoint != null
            && patrolRightPoint != null
            && enemyAttackController != null
            && enemyHealth != null;
    }

    private void UpdateStateFromPlayerDistance()
    {
        if (enemyHealth.IsDead)
        {
            ChangeState(EnemyState.Dead);
            return;
        }

        float playerDistance = Vector2.Distance(transform.position, playerTarget.position);

        if (playerDistance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        if (playerDistance <= detectionRange)
        {
            ChangeState(EnemyState.Chase);
            return;
        }

        ChangeState(EnemyState.Patrol);
    }

    private void TryAttackPlayer()
    {
        if (currentState == EnemyState.Attack)
        {
            enemyAttackController.TryAttack(playerHealth);
        }
    }

    private void MoveForCurrentState()
    {
        if (currentState == EnemyState.Patrol)
        {
            Patrol();
            return;
        }

        if (currentState == EnemyState.Chase)
        {
            ChasePlayer();
            return;
        }

        StopHorizontalMovement();
    }

    private void Patrol()
    {
        MoveToward(currentPatrolTarget, patrolSpeed);

        if (ReachedPatrolTarget())
        {
            SwitchPatrolTarget();
        }
    }

    private void ChasePlayer()
    {
        MoveToward(playerTarget, chaseSpeed);
    }

    private void MoveToward(Transform target, float speed)
    {
        float directionX = Mathf.Sign(target.position.x - transform.position.x);
        Vector2 currentVelocity = enemyRigidbody.velocity;
        currentVelocity.x = directionX * speed;
        enemyRigidbody.velocity = currentVelocity;
    }

    private bool ReachedPatrolTarget()
    {
        return Mathf.Abs(transform.position.x - currentPatrolTarget.position.x) <= 0.1f;
    }

    private void SwitchPatrolTarget()
    {
        currentPatrolTarget = currentPatrolTarget == patrolLeftPoint ? patrolRightPoint : patrolLeftPoint;
    }

    private void ChangeState(EnemyState nextState)
    {
        currentState = nextState;
    }

    private void HandleEnemyDefeated(EnemyHealth defeatedEnemy)
    {
        ChangeState(EnemyState.Dead);
        StopHorizontalMovement();
    }

    private void StopHorizontalMovement()
    {
        Vector2 currentVelocity = enemyRigidbody.velocity;
        currentVelocity.x = 0f;
        enemyRigidbody.velocity = currentVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (patrolLeftPoint != null && patrolRightPoint != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(patrolLeftPoint.position, patrolRightPoint.position);
        }
    }
}
