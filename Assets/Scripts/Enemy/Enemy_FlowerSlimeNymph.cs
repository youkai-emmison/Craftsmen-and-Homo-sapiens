using UnityEngine;

// Regenerating enemy variant that uses the flower slime nymph visual.
// Key variables:
// - healAmount: Health restored each heal tick.
// - healInterval: Seconds between heal ticks.
// - healRange: Reserved Inspector note for future ally-heal tuning; current demo heals self only.
public class Enemy_FlowerSlimeNymph : Enemy
{
    [Header("Self Heal")]
    [SerializeField] private float healAmount = 8f;
    [SerializeField] private float healInterval = 3f;
    [SerializeField] private float healRange = 3.5f;

    private float nextHealTime;

    protected override void Update()
    {
        base.Update();
        UpdateSelfHeal();
    }

    private void UpdateSelfHeal()
    {
        if (currentState == EnemyState.Dead)
            return;

        if (Time.time < nextHealTime)
            return;

        nextHealTime = Time.time + healInterval;
        HealSelf();
    }

    private void HealSelf()
    {
        if (currentHealth >= maxHealth)
            return;

        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        Debug.Log($"{name} healed {healAmount}. Health: {currentHealth}/{maxHealth}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.4f, 1f, 0.5f, 0.45f);
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
