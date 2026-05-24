// Script purpose: Spawns demo experience pickups when the assigned enemy is defeated.
// Key Inspector variables:
// - enemyHealth: EnemyHealth event source for the defeated signal.
// - experiencePickupPrefab: Pickup prefab spawned after defeat.
// - dropCount: Number of pickups spawned for compressed demo pacing.
// - dropOffset: Offset from the enemy position used for pickup placement.
using UnityEngine;

public class EnemyDropController : MonoBehaviour
{
    // Enemy health event source manually assigned in the Inspector.
    public EnemyHealth enemyHealth;

    // Prefab containing ExperiencePickup and a Trigger Collider2D.
    public GameObject experiencePickupPrefab;

    // Number of pickups spawned when this enemy is defeated.
    public int dropCount = 1;

    // Position offset applied from the defeated enemy.
    public Vector3 dropOffset = new Vector3(0f, 0.6f, 0f);

    private void Awake()
    {
        ValidateRequiredReferences();
    }

    private void OnEnable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDefeated += HandleEnemyDefeated;
        }
    }

    private void OnDisable()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnEnemyDefeated -= HandleEnemyDefeated;
        }
    }

    private void ValidateRequiredReferences()
    {
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyDropController: Enemy Health is not assigned.", this);
        }

        if (experiencePickupPrefab == null)
        {
            Debug.LogError("EnemyDropController: Experience Pickup Prefab is not assigned.", this);
        }

        if (dropCount <= 0)
        {
            Debug.LogError("EnemyDropController: Drop Count must be greater than 0.", this);
        }
    }

    private void HandleEnemyDefeated(EnemyHealth defeatedEnemy)
    {
        if (experiencePickupPrefab == null || dropCount <= 0)
        {
            return;
        }

        SpawnExperiencePickups();
    }

    private void SpawnExperiencePickups()
    {
        for (int pickupIndex = 0; pickupIndex < dropCount; pickupIndex++)
        {
            Vector3 pickupPosition = transform.position + dropOffset + GetSpreadOffset(pickupIndex);
            Instantiate(experiencePickupPrefab, pickupPosition, Quaternion.identity);
        }
    }

    private Vector3 GetSpreadOffset(int pickupIndex)
    {
        float horizontalOffset = (pickupIndex - (dropCount - 1) * 0.5f) * 0.35f;
        return new Vector3(horizontalOffset, 0f, 0f);
    }
}
