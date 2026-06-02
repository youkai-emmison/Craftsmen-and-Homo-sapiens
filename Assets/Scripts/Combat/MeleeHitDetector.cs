// Script purpose: Detects objects inside a melee circle.
// Key Inspector variables:
// - attackPoint: Center point of the melee check.
// - attackRange: Radius of the melee check.
// - hittableLayer: Layers that can be hit by the melee attack.
using UnityEngine;

public class MeleeHitDetector : MonoBehaviour
{
    // Center of the attack circle, usually placed in front of the player.
    public Transform attackPoint;

    // Radius of the melee hit area.
    public float attackRange = 0.75f;

    // Layers considered valid attack targets.
    public LayerMask hittableLayer;

    /// <summary>
    /// Detect all colliders in attack range.
    /// Returns empty array if no targets found.
    /// </summary>
    public Collider2D[] DetectTargets()
    {
        if (attackPoint == null)
        {
            Debug.LogError("MeleeHitDetector: Attack Point is not assigned.", this);
            return System.Array.Empty<Collider2D>();
        }

        return Physics2D.OverlapCircleAll(attackPoint.position, attackRange, hittableLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
