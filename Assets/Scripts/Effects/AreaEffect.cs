using UnityEngine;

[CreateAssetMenu(fileName = "Area Effect", menuName = "Inventory/Effects/Area")]
public class AreaEffect : ItemEffect
{
    [Header("范围效果设置")]
    public float radius = 3f;
    public float damage = 50f;
    public LayerMask targetLayer;
    public GameObject effectVFX;

    public override void ExecuteEffect(Transform target)
    {
        if (target == null) return;

        // 生成 VFX
        if (effectVFX != null)
            Instantiate(effectVFX, target.position, Quaternion.identity);

        // 范围伤害
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, radius, targetLayer);
        foreach (var hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(Mathf.RoundToInt(damage));
        }

        Debug.Log($"AreaEffect: 在 {target.position} 造成范围伤害 {damage}");
    }
}
