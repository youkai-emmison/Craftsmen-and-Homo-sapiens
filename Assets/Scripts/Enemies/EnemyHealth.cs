// Script purpose: Stores demo enemy health, applies damage, and announces defeat.
// Key Inspector variables:
// - maxHealth: Enemy health at scene start.
// - disableOnDefeat: Whether the enemy GameObject is disabled after defeat.
using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    // Starting and maximum health for this enemy.
    public int maxHealth = 3;

    // Whether this prototype enemy is disabled after defeat.
    public bool disableOnDefeat = true;

    // Runtime health value reduced by player attacks.
    private int currentHealth;

    // True after the enemy has been defeated.
    public bool IsDead { get; private set; }

    // Room and drop controllers subscribe to this to react to defeat.
    public Action<EnemyHealth> OnEnemyDefeated;

    private void Awake()
    {
        if (maxHealth <= 0)
        {
            Debug.LogError("EnemyHealth: Max Health must be greater than 0.", this);
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (IsDead)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        Debug.Log($"EnemyHealth: Took {damageAmount} damage. Current health: {currentHealth}.", this);

        if (currentHealth <= 0)
        {
            DefeatEnemy();
        }
    }

    private void DefeatEnemy()
    {
        IsDead = true;
        Debug.Log("EnemyHealth: Enemy defeated.", this);
        OnEnemyDefeated?.Invoke(this);

        if (disableOnDefeat)
        {
            gameObject.SetActive(false);
        }
    }
}
