// Script purpose: Updates a tiny world-space health bar for any Entity.
// Key Inspector variables:
// - fillTransform: Bar fill object scaled from full to empty on X.
// - entity: Entity source whose currentHealth / maxHealth drive the bar.
using UnityEngine;

public class SimpleHealthBar : MonoBehaviour
{
    // Fill object scaled horizontally while the left edge stays fixed.
    public Transform fillTransform;

    // Entity whose health drives this bar.
    public Entity entity;

    private Vector3 initialFillLocalScale;
    private Vector3 initialFillLocalPosition;

    private void Awake()
    {
        if (fillTransform == null)
            Debug.LogError("SimpleHealthBar: Fill Transform is not assigned.", this);
        else
            CacheFillTransform();

        if (entity == null)
            entity = GetComponentInParent<Entity>();

        if (entity == null)
            Debug.LogError("SimpleHealthBar: Entity is not assigned.", this);
    }

    private void Update()
    {
        if (fillTransform == null || entity == null) return;

        float ratio = entity.maxHealth > 0f
            ? Mathf.Clamp01(entity.currentHealth / entity.maxHealth)
            : 0f;

        UpdateFillTransform(ratio);
    }

    private void CacheFillTransform()
    {
        initialFillLocalScale = fillTransform.localScale;
        initialFillLocalPosition = fillTransform.localPosition;
    }

    private void UpdateFillTransform(float ratio)
    {
        Vector3 fillScale = initialFillLocalScale;
        fillScale.x = initialFillLocalScale.x * ratio;
        fillTransform.localScale = fillScale;

        // Most placeholder bars use a centered sprite pivot, so move the fill left
        // while shrinking to keep the left edge visually anchored.
        Vector3 fillPosition = initialFillLocalPosition;
        float lostWidth = initialFillLocalScale.x - fillScale.x;
        fillPosition.x = initialFillLocalPosition.x - lostWidth * 0.5f;
        fillTransform.localPosition = fillPosition;
    }
}
