// Script purpose: Applies enemy damage to PlayerHealth with a cooldown.
// Key Inspector variables:
// - damageAmount: Damage dealt per accepted attack.
// - attackCooldown: Minimum time between enemy attacks.
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    // Damage applied to PlayerHealth each time the cooldown allows an attack.
    public int damageAmount = 1;

    // Minimum time in seconds before this enemy can attack again.
    public float attackCooldown = 1f;

    // Timestamp gate that prevents per-frame damage.
    private float nextAttackAllowedTime;

    public void TryAttack(PlayerHealth playerHealth)
    {
        if (playerHealth == null)
        {
            Debug.LogError("EnemyAttackController: PlayerHealth is not assigned for this attack.", this);
            return;
        }

        if (Time.time < nextAttackAllowedTime)
        {
            return;
        }

        nextAttackAllowedTime = Time.time + attackCooldown;
        playerHealth.TakeDamage(damageAmount);
    }
}
