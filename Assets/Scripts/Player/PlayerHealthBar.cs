// Script purpose: Displays the player's current health with a simple world-space bar.
// Key Inspector variables:
// - playerHealth: PlayerHealth source read by the bar.
// - fillTransform: Fill Transform scaled to show remaining health.
// - fullWidth: X scale used when the player is at full health.
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    // Health source assigned from the Player object.
    public PlayerHealth playerHealth;

    // Fill Transform that shrinks as health decreases.
    public Transform fillTransform;

    // X scale used when health is full.
    public float fullWidth = 1.4f;

    private void Awake()
    {
        ValidateRequiredReferences();
        UpdateHealthBar();
    }

    private void LateUpdate()
    {
        UpdateHealthBar();
    }

    private void ValidateRequiredReferences()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealthBar: Player Health is not assigned.", this);
        }

        if (fillTransform == null)
        {
            Debug.LogError("PlayerHealthBar: Fill Transform is not assigned.", this);
        }
    }

    private void UpdateHealthBar()
    {
        if (playerHealth == null || fillTransform == null)
        {
            return;
        }

        if (playerHealth.maxHealth <= 0)
        {
            return;
        }

        float healthRatio = Mathf.Clamp01((float)playerHealth.CurrentHealth / playerHealth.maxHealth);
        ResizeFill(healthRatio);
        RepositionFill(healthRatio);
    }

    private void ResizeFill(float healthRatio)
    {
        Vector3 fillScale = fillTransform.localScale;
        fillScale.x = fullWidth * healthRatio;
        fillTransform.localScale = fillScale;
    }

    private void RepositionFill(float healthRatio)
    {
        Vector3 fillPosition = fillTransform.localPosition;
        fillPosition.x = (healthRatio - 1f) * fullWidth * 0.5f;
        fillTransform.localPosition = fillPosition;
    }
}
