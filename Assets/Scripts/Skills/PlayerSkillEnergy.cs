// Script purpose: Stores the player's demo skill-energy points and restores them over time.
// Key Inspector variables:
// - baseMaxSkillEnergy: Default skill-energy cap before equipment bonuses.
// - bonusMaxSkillEnergy: Extra cap reserved for future equipment effects.
// - recoverSecondsPerPoint: Seconds required to recover one skill-energy point.
using System;
using UnityEngine;

public class PlayerSkillEnergy : MonoBehaviour
{
    // Default cap requested for the current prototype.
    public int baseMaxSkillEnergy = 5;

    // Future equipment can increase this value without changing the UI code.
    public int bonusMaxSkillEnergy;

    // One point is restored each time this timer fills.
    public float recoverSecondsPerPoint = 2f;

    // Runtime current value. Serialized so Unity play-mode debugging is easy.
    [SerializeField] private int currentSkillEnergy;

    // Runtime recovery timer used to drive both regen and UI progress.
    private float recoverTimer;

    public event Action<int, int> OnSkillEnergyChanged;

    public int CurrentSkillEnergy => currentSkillEnergy;

    public int MaxSkillEnergy => Mathf.Max(0, baseMaxSkillEnergy + bonusMaxSkillEnergy);

    public float RecoverProgress01 => recoverSecondsPerPoint > 0f
        ? Mathf.Clamp01(recoverTimer / recoverSecondsPerPoint)
        : 0f;

    private void Awake()
    {
        currentSkillEnergy = MaxSkillEnergy;
        NotifyChanged();
    }

    private void Update()
    {
        RecoverOverTime();
    }

    public bool TrySpendSkillEnergy(int cost)
    {
        int clampedCost = Mathf.Max(0, cost);
        if (currentSkillEnergy < clampedCost)
        {
            return false;
        }

        currentSkillEnergy -= clampedCost;
        recoverTimer = 0f;
        NotifyChanged();
        return true;
    }

    public void RestoreSkillEnergy(int amount)
    {
        int clampedAmount = Mathf.Max(0, amount);
        currentSkillEnergy = Mathf.Min(MaxSkillEnergy, currentSkillEnergy + clampedAmount);
        NotifyChanged();
    }

    public void SetBonusMaxSkillEnergy(int newBonus)
    {
        bonusMaxSkillEnergy = Mathf.Max(0, newBonus);
        currentSkillEnergy = Mathf.Min(currentSkillEnergy, MaxSkillEnergy);
        NotifyChanged();
    }

    private void RecoverOverTime()
    {
        if (currentSkillEnergy >= MaxSkillEnergy || recoverSecondsPerPoint <= 0f)
        {
            recoverTimer = 0f;
            return;
        }

        recoverTimer += Time.deltaTime;
        if (recoverTimer < recoverSecondsPerPoint)
        {
            return;
        }

        recoverTimer -= recoverSecondsPerPoint;
        RestoreSkillEnergy(1);
    }

    private void NotifyChanged()
    {
        OnSkillEnergyChanged?.Invoke(currentSkillEnergy, MaxSkillEnergy);
    }
}
