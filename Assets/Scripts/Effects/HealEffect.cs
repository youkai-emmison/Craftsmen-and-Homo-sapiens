using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Inventory/Effects/Heal")]
public class HealEffect : ItemEffect
{
    [Header("回复设置")]
    [Range(0f, 1f)] public float healPercent = 0.1f;

    public override void ExecuteEffect(Transform target)
    {
        if (target == null) return;
        CharacterStats stats = target.GetComponent<CharacterStats>();
        if (stats == null) return;

        float healAmount = stats.maxHealth.Value * healPercent;
        stats.currentHealth = Mathf.Min(stats.currentHealth + healAmount, stats.maxHealth.Value);
        Debug.Log($"HealEffect: 回复 {healAmount:F0} 生命值");
    }
}
