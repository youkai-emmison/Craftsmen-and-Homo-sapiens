// Script purpose: Stores player health and reports defeat for the prototype.
// Key Inspector variables:
// - maxHealth: Player health at scene start.
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Starting and maximum health for the player.
    public int maxHealth = 5;

    // Runtime health value reduced by enemy attacks.
    private int currentHealth;

    // Stops additional damage logs after defeat.
    private bool isDefeated;

    // Read-only access for future UI without giving outside scripts write access.
    public int CurrentHealth => currentHealth;

    private void Awake()
    {
        if (maxHealth <= 0)
        {
            Debug.LogError("PlayerHealth: Max Health must be greater than 0.", this);
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDefeated)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        Debug.Log($"PlayerHealth: Took {damageAmount} damage. Current health: {currentHealth}.", this);

        if (currentHealth <= 0)
        {
            DefeatPlayer();
        }
    }

    private void DefeatPlayer()
    {
        isDefeated = true;
        Debug.Log("Player defeated.", this);
    }
}
