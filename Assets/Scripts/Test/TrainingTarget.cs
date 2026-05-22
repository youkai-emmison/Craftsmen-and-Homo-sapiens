// Script purpose: Provides a static damageable target for attack testing.
// Key Inspector variables:
// - maxHealth: Target health at scene start.
// - healthBar: Scene-space health bar updated after damage.
using UnityEngine;

public class TrainingTarget : MonoBehaviour, IDamageable
{
    // Starting and maximum health for this test target.
    public int maxHealth = 3;

    // Optional visible health bar for training feedback.
    public TrainingTargetHealthBar healthBar;

    // Runtime health value reduced by player attacks.
    private int currentHealth;

    // Existing renderer used for simple hit feedback when present.
    private SpriteRenderer targetSpriteRenderer;

    private void Awake()
    {
        // TrainingTarget owns only its health and simple feedback; it is not enemy AI.
        currentHealth = maxHealth;
        targetSpriteRenderer = GetComponent<SpriteRenderer>();
        ValidateRequiredReferences();
        UpdateHealthBar();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"TrainingTarget: Took {damageAmount} damage. Current health: {currentHealth}.", this);

        UpdateHealthBar();
        ApplyHitFeedback();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ValidateRequiredReferences()
    {
        if (maxHealth <= 0)
        {
            Debug.LogError("TrainingTarget: Max Health must be greater than 0.", this);
        }

        if (healthBar == null)
        {
            Debug.LogError("TrainingTarget: Health Bar is not assigned. Drag the TrainingTargetHealthBar component into this field.", this);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }

        healthBar.SetHealth(currentHealth, maxHealth);
    }

    private void ApplyHitFeedback()
    {
        if (targetSpriteRenderer == null)
        {
            return;
        }

        // Temporary placeholder feedback until real hit animation exists.
        targetSpriteRenderer.color = Color.gray;
    }
}
