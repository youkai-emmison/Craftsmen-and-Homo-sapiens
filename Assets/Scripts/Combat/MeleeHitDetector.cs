// Script purpose: Detects damageable objects inside a melee circle.
// Key Inspector variables:
// - attackPoint: Center point of the melee check.
// - attackRange: Radius of the melee check.
// - hittableLayer: Layers that can be hit by the melee attack.
// - damageAmount: Damage sent when no PlayerDemoStats is assigned.
// - playerDemoStats: Optional demo stats source that overrides attack damage.
using UnityEngine;

public class MeleeHitDetector : MonoBehaviour
{
    // Center of the attack circle, usually placed in front of the player.
    public Transform attackPoint;

    // Radius of the melee hit area.
    public float attackRange = 0.75f;

    // Layers considered valid attack targets.
    public LayerMask hittableLayer;

    // Damage used before demo progression is configured.
    public int damageAmount = 1;

    // Optional demo progression stats used to show stronger attacks after level-up.
    public PlayerDemoStats playerDemoStats;

    public void DetectHit()
    {
        if (attackPoint == null)
        {
            Debug.LogError("MeleeHitDetector: Attack Point is not assigned.", this);
            return;
        }

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hittableLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(GetCurrentDamageAmount());
            }
        }
    }

    private int GetCurrentDamageAmount()
    {
        if (playerDemoStats != null)
        {
            return playerDemoStats.CurrentAttackDamage;
        }

        return damageAmount;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
