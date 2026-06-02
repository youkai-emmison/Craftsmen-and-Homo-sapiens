using UnityEngine;

[CreateAssetMenu(fileName = "Conditional Effect", menuName = "Inventory/Effects/Conditional")]
public class ConditionalEffect : ItemEffect
{
    [Header("条件设置")]
    [Range(0f, 1f)] public float healthThreshold = 0.1f;
    public float freezeDuration = 3f;
    public float freezeRadius = 5f;
    public LayerMask enemyLayer;

    public override void ExecuteEffect(Transform target)
    {
        if (target == null) return;
        CharacterStats stats = target.GetComponent<CharacterStats>();
        if (stats == null) return;

        float healthRatio = stats.currentHealth / stats.maxHealth.Value;
        if (healthRatio > healthThreshold) return;

        // 冻结周围敌人
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, freezeRadius, enemyLayer);
        foreach (var hit in hits)
        {
            Entity entity = hit.GetComponent<Entity>();
            if (entity != null)
            {
                // 可以在这里添加冰冻状态逻辑
                Debug.Log($"ConditionalEffect: 冰冻了 {hit.name}");
            }
        }
    }
}
