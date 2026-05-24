// Script purpose: Updates a tiny world-space health bar for demo health components.
// Key Inspector variables:
// - fillTransform: Bar fill object scaled from 0 to 1 on X.
// - playerHealth / enemyHealth: One health source assigned manually.
using UnityEngine;

public class SimpleHealthBar : MonoBehaviour
{
    // Fill object scaled horizontally by the current health ratio.
    public Transform fillTransform;

    // Optional player health source.
    public PlayerHealth playerHealth;

    // Optional enemy health source.
    public EnemyHealth enemyHealth;

    private void Awake()
    {
        ValidateRequiredReferences();
    }

    private void Update()
    {
        UpdateFillScale();
    }

    private void ValidateRequiredReferences()
    {
        if (fillTransform == null)
        {
            Debug.LogError("SimpleHealthBar: Fill Transform is not assigned.", this);
        }

        if (playerHealth == null && enemyHealth == null)
        {
            Debug.LogError("SimpleHealthBar: Assign either Player Health or Enemy Health.", this);
        }
    }

    private void UpdateFillScale()
    {
        if (fillTransform == null)
        {
            return;
        }

        float healthRatio = GetHealthRatio();
        fillTransform.localScale = new Vector3(healthRatio, 1f, 1f);
    }

    private float GetHealthRatio()
    {
        if (playerHealth != null)
        {
            return Mathf.Clamp01((float)playerHealth.CurrentHealth / playerHealth.maxHealth);
        }

        if (enemyHealth != null)
        {
            return Mathf.Clamp01((float)enemyHealth.CurrentHealth / enemyHealth.maxHealth);
        }

        return 0f;
    }
}
