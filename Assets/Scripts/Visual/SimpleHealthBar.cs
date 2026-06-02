// Script purpose: Updates a tiny world-space health bar for any Entity.
// Key Inspector variables:
// - fillTransform: Bar fill object scaled from 0 to 1 on X.
// - entity: Entity source whose currentHealth / maxHealth drive the bar.
using UnityEngine;

public class SimpleHealthBar : MonoBehaviour
{
    // Fill object scaled horizontally by the current health ratio.
    public Transform fillTransform;

    // Entity whose health drives this bar.
    public Entity entity;

    private void Awake()
    {
        if (fillTransform == null)
            Debug.LogError("SimpleHealthBar: Fill Transform is not assigned.", this);

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
        fillTransform.localScale = new Vector3(ratio, 1f, 1f);
    }
}
