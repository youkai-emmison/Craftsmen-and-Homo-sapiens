using UnityEngine;

/// <summary>
/// 装置超频效果 —— 临时增强场上所有装置的能力。
/// 作为 SkillEffect 附加到 SkillData。
/// </summary>
[CreateAssetMenu(fileName = "DeviceOverclock", menuName = "Skills/Device Overclock")]
public class DeviceOverclockEffect : SkillEffect
{
    [Header("增益配置")]
    public float damageMultiplier = 1.5f;    // 伤害提升 50%
    public float durationBonus = 10f;        // 额外持续时间（秒）
    public int durabilityBonus = 3;          // 额外耐久
    public float buffDuration = 15f;         // buff 持续时间（秒）

    private bool isActive;
    private float remainingTime;

    public override bool IsActive => isActive;

    public override bool Activate(Transform caster, SkillManager manager)
    {
        // 查找场上所有装置并应用增益
        DeviceController[] devices = FindObjectsByType<DeviceController>(FindObjectsSortMode.None);

        if (devices.Length == 0)
        {
            Debug.Log("DeviceOverclockEffect: 场上没有装置");
            return false;
        }

        foreach (var device in devices)
        {
            if (device.IsInitialized)
            {
                device.ApplyBuff(damageMultiplier, durationBonus, durabilityBonus);
            }
        }

        isActive = true;
        remainingTime = buffDuration;

        Debug.Log($"DeviceOverclockEffect: 已增强 {devices.Length} 个装置，持续 {buffDuration} 秒");
        return true;
    }

    public override void Deactivate(Transform caster)
    {
        if (!isActive) return;

        // 移除所有装置的增益
        DeviceController[] devices = FindObjectsByType<DeviceController>(FindObjectsSortMode.None);
        foreach (var device in devices)
        {
            if (device.IsInitialized)
            {
                device.RemoveBuff();
            }
        }

        isActive = false;
        remainingTime = 0f;

        Debug.Log("DeviceOverclockEffect: 增益已结束");
    }

    public override void Tick(Transform caster, float deltaTime)
    {
        if (!isActive) return;

        remainingTime -= deltaTime;
        if (remainingTime <= 0f)
        {
            Deactivate(caster);
        }
    }
}
