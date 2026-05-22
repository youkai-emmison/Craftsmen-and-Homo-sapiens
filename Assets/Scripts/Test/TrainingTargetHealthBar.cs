// Script purpose: Updates a simple world-space health bar for the training target.
// Key Inspector variables:
// - fillTransform: Transform scaled to show remaining health.
// - fullWidth: Width used when health is full.
using UnityEngine;

public class TrainingTargetHealthBar : MonoBehaviour
{
    // The child transform that visually shrinks as health decreases.
    public Transform fillTransform;

    // X scale used when the target is at full health.
    public float fullWidth = 1f;

    // Prevents repeated missing-reference logs.
    private bool hasLoggedMissingFillTransform;

    public void SetHealth(int currentHealth, int maxHealth)
    {
        // This is a scene-space placeholder bar, not a full UI system.
        if (fillTransform == null)
        {
            LogMissingFillTransform();
            return;
        }

        if (maxHealth <= 0)
        {
            Debug.LogError("TrainingTargetHealthBar: Max Health must be greater than 0.", this);
            return;
        }

        float healthRatio = Mathf.Clamp01((float)currentHealth / maxHealth);
        ResizeFill(healthRatio);
        RepositionFill(healthRatio);
    }

    private void LogMissingFillTransform()
    {
        if (hasLoggedMissingFillTransform)
        {
            return;
        }

        Debug.LogError("TrainingTargetHealthBar: Fill Transform is not assigned.", this);
        hasLoggedMissingFillTransform = true;
    }

    private void ResizeFill(float healthRatio)
    {
        // Scaling X changes the visible amount of health.
        Vector3 fillScale = fillTransform.localScale;
        fillScale.x = fullWidth * healthRatio;
        fillTransform.localScale = fillScale;
    }

    private void RepositionFill(float healthRatio)
    {
        // Shift left so the bar drains from right to left instead of shrinking from the center.
        Vector3 fillPosition = fillTransform.localPosition;
        fillPosition.x = (healthRatio - 1f) * fullWidth * 0.5f;
        fillTransform.localPosition = fillPosition;
    }
}
