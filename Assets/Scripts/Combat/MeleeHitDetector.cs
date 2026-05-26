// Script purpose: Detects damageable objects inside a melee circle.
// Key Inspector variables:
// - attackPoint: Center point of the melee check.
// - attackRange: Radius of the melee check.
// - hittableLayer: Layers that can be hit by the melee attack.
// - damageAmount: Damage sent to each IDamageable found.
using UnityEngine;

public class MeleeHitDetector : MonoBehaviour
{
    // Center of the attack circle, usually placed in front of the player.
    public Transform attackPoint;

    // Radius of the melee hit area.
    public float attackRange = 0.75f;

    // Layers considered valid attack targets.
    public LayerMask hittableLayer;

    // Damage applied to each hit target.
    public int damageAmount = 1;

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
                damageable.TakeDamage(damageAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
