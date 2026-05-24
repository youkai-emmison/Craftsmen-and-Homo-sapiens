// Script purpose: Stores temporary demo progression stats for fast level-up testing.
// Key Inspector variables:
// - level: Current demo level shown in the Inspector.
// - experience / experienceToLevel: Demo experience gate for the next level.
// - baseAttackDamage / bonusAttackDamage: Values combined into CurrentAttackDamage.
// - maxHealthBonus: Demo health growth value logged for later tuning.
using UnityEngine;

public class PlayerDemoStats : MonoBehaviour
{
    // Current compressed demo level.
    public int level = 1;

    // Current experience collected from demo pickups.
    public int experience;

    // Experience needed for the next demo level.
    public int experienceToLevel = 3;

    // Starting attack damage before demo upgrades.
    public int baseAttackDamage = 1;

    // Extra attack damage granted by demo level-ups.
    public int bonusAttackDamage;

    // Extra max health value recorded for later PlayerHealth integration.
    public int maxHealthBonus;

    // Attack damage read by MeleeHitDetector when this component is assigned.
    public int CurrentAttackDamage => baseAttackDamage + bonusAttackDamage;

    public void AddExperience(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("PlayerDemoStats: Experience amount must be greater than 0.", this);
            return;
        }

        experience += amount;
        Debug.Log($"PlayerDemoStats: Gained {amount} experience. Current experience: {experience}/{experienceToLevel}.", this);

        TryLevelUp();
    }

    private void TryLevelUp()
    {
        while (experience >= experienceToLevel)
        {
            experience -= experienceToLevel;
            ApplyLevelUp();
        }
    }

    private void ApplyLevelUp()
    {
        level += 1;
        bonusAttackDamage += 2;
        maxHealthBonus += 1;
        experienceToLevel += 2;

        Debug.Log($"Level Up! Level: {level}. Attack Damage: {CurrentAttackDamage}. Max Health Bonus: {maxHealthBonus}.", this);
    }
}
