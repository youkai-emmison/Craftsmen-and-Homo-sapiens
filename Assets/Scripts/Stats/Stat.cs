using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 单个属性值：基础值 + 修改器列表。
/// 计算顺序：先叠加所有 Flat，再乘以 Percent 总和。
/// </summary>
[System.Serializable]
public class Stat
{
    public float baseValue;                     // 基础值
    private List<StatModifier> modifiers;       // 修改器列表

    public Stat()
    {
        modifiers = new List<StatModifier>();
    }

    public Stat(float baseValue) : this()
    {
        this.baseValue = baseValue;
    }

    /// <summary>
    /// 最终计算值：(baseValue + flatSum) × (1 + percentSum)
    /// </summary>
    public float Value => CalculateFinalValue();

    /// <summary>
    /// 不含任何修改器的原始基础值。
    /// </summary>
    public float BaseValue => baseValue;

    /// <summary>
    /// 添加修改器。
    /// </summary>
    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
    }

    /// <summary>
    /// 移除修改器。按引用或按 source 移除所有匹配项。
    /// </summary>
    public bool RemoveModifier(StatModifier modifier)
    {
        return modifiers.Remove(modifier);
    }

    /// <summary>
    /// 移除指定来源的所有修改器。
    /// </summary>
    public int RemoveAllModifiersFromSource(object source)
    {
        return modifiers.RemoveAll(m => m.source == source);
    }

    /// <summary>
    /// 清空所有修改器。
    /// </summary>
    public void ClearModifiers()
    {
        modifiers.Clear();
    }

    private float CalculateFinalValue()
    {
        float flatSum = 0f;
        float percentSum = 0f;

        foreach (var mod in modifiers)
        {
            if (mod.type == ModifierType.Flat)
                flatSum += mod.value;
            else
                percentSum += mod.value;
        }

        return (baseValue + flatSum) * (1f + percentSum);
    }
}
