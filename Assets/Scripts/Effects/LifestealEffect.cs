using UnityEngine;

[CreateAssetMenu(fileName = "Lifesteal Effect", menuName = "Inventory/Effects/Lifesteal")]
public class LifestealEffect : ItemEffect
{
    [Header("吸血设置")]
    [Range(0f, 1f)] public float stealPercent = 0.02f;

    public override void ExecuteEffect(Transform target)
    {
        // target 是被攻击的敌人，需要从攻击者（Player）身上回血
        // 通过 ItemEffectManager 调用时 target = 被攻击者
        // 攻击者通过 Find 或传入
    }

    public void ExecuteEffect(Transform attacker, float damageDealt)
    {
        if (attacker == null) return;
        CharacterStats stats = attacker.GetComponent<CharacterStats>();
        if (stats == null) return;

        float healAmount = damageDealt * stealPercent;
        stats.currentHealth = Mathf.Min(stats.currentHealth + healAmount, stats.maxHealth.Value);
        Debug.Log($"LifestealEffect: 吸血 {healAmount:F1}");
    }
}
