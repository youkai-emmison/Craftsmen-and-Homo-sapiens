// Script purpose: Reads player attack input and triggers one melee hit check per cooldown.
// Key Inspector variables:
// - attackCooldown: Minimum time between attack attempts.
// - meleeHitDetector: Component that performs the actual melee overlap check.
// - attackSlashVisual: Optional visual flash shown when the attack is accepted.
// - maidVisualAnimatorDriver: Optional visual-only attack animation trigger.
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    // Minimum time in seconds before another attack can start.
    public float attackCooldown = 0.35f;

    // Required detector called once when an attack is accepted.
    public MeleeHitDetector meleeHitDetector;

    // Optional slash marker used only for demo readability.
    public AttackSlashVisual attackSlashVisual;

    // Optional facing controller used to flip the slash direction.
    public PlayerAttackFacingController attackFacingController;

    // Optional visual-only Animator driver. It does not control hit detection.
    public MaidVisualAnimatorDriver maidVisualAnimatorDriver;

    // Player stats for damage calculation.
    private PlayerStats playerStats;

    // Timestamp gate that prevents click-spam attacks.
    private float nextAttackAllowedTime;

    /// <summary>每次命中目标时触发，用于装备特效等</summary>
    public event System.Action<Transform> OnHitTarget;

    private void Awake()
    {
        if (meleeHitDetector == null)
        {
            Debug.LogError("PlayerAttackController: MeleeHitDetector is not assigned.", this);
        }
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (meleeHitDetector == null)
        {
            return;
        }

        if (Time.time < nextAttackAllowedTime)
        {
            return;
        }

        nextAttackAllowedTime = Time.time + attackCooldown;

        // Detect targets and deal damage.
        Collider2D[] targets = meleeHitDetector.DetectTargets();
        foreach (Collider2D target in targets)
        {
            Entity entity = target.GetComponent<Entity>();
            if (entity != null)
            {
                // Calculate damage using PlayerStats.
                float damage = playerStats != null ? playerStats.finalDamage : 1f;

                // Check for crit.
                bool isCrit = playerStats != null && Random.value < playerStats.finalCritRate;
                if (isCrit)
                {
                    damage *= playerStats.finalCritDamage;
                }

                // Apply damage.
                entity.TakeDamage(damage, transform.position);

                // Trigger hit event.
                OnHitTarget?.Invoke(target.transform);

                Debug.Log($"Hit {target.name} for {damage:F1} damage" + (isCrit ? " (CRIT!)" : ""));
            }
        }

        PlayAttackAnimation();
        PlayAttackVisual();
    }

    private void PlayAttackAnimation()
    {
        if (maidVisualAnimatorDriver == null)
        {
            return;
        }

        maidVisualAnimatorDriver.PlayAttack();
    }

    private void PlayAttackVisual()
    {
        if (attackSlashVisual == null || meleeHitDetector == null || meleeHitDetector.attackPoint == null)
        {
            return;
        }

        float facingDirectionX = attackFacingController != null ? attackFacingController.FacingDirectionX : Mathf.Sign(meleeHitDetector.attackPoint.localPosition.x);
        attackSlashVisual.Play(meleeHitDetector.attackPoint.position, facingDirectionX);
    }
}
