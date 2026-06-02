using UnityEngine;

[CreateAssetMenu(fileName = "Projectile Trigger", menuName = "Inventory/Effects/Projectile Trigger")]
public class ProjectileTriggerEffect : ItemEffect
{
    [Header("触发设置")]
    public int comboHitRequired = 3;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileDamage = 20f;

    private int currentCombo;

    public override void ExecuteEffect(Transform target)
    {
        currentCombo++;
        if (currentCombo < comboHitRequired) return;
        currentCombo = 0;

        if (projectilePrefab == null || target == null) return;

        // 在攻击者位置生成投射物
        Transform attacker = target; // 实际应为攻击者，此处简化
        Vector3 spawnPos = attacker.position;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 dir = (target.position - spawnPos).normalized;
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = dir * projectileSpeed;

        Debug.Log($"ProjectileTriggerEffect: 第 {comboHitRequired} 段连击发射投射物");
    }
}
