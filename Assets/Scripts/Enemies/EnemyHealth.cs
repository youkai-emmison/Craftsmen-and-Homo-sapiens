// Script purpose: Stores enemy health, applies damage, and announces defeat.
// Key Inspector variables:
// - maxHealth: Enemy health at scene start.
// - hitFlash: Optional visual feedback component played when damaged.
using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    // Starting and maximum health for this enemy.
    public int maxHealth = 3;

    // Optional flash feedback used on hit.
    public SimpleHitFlash hitFlash;

    // Runtime health value reduced by player attacks.
    private int currentHealth;

    // Existing renderer used for simple fallback color feedback when present.
    private SpriteRenderer enemySpriteRenderer;

    // True after the enemy has been defeated.
    public bool IsDead { get; private set; }

    // RoomClearController subscribes to this to unlock the room.
    public Action<EnemyHealth> OnEnemyDefeated;

    private void Awake()
    {
        if (maxHealth <= 0)
        {
            Debug.LogError("EnemyHealth: Max Health must be greater than 0.", this);
        }

        currentHealth = maxHealth;
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (IsDead)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        Debug.Log($"EnemyHealth: Took {damageAmount} damage. Current health: {currentHealth}.", this);
        PlayHitFeedback();

        if (currentHealth <= 0)
        {
            DefeatEnemy();
        }
    }

    private void PlayHitFeedback()
    {
        if (hitFlash != null)
        {
            hitFlash.PlayFlash();
            return;
        }

        if (enemySpriteRenderer != null)
        {
            enemySpriteRenderer.color = Color.gray;
        }
    }

    private void DefeatEnemy()
    {
        IsDead = true;
        Debug.Log("EnemyHealth: Enemy defeated.", this);
        OnEnemyDefeated?.Invoke(this);
        gameObject.SetActive(false);
    }
}
