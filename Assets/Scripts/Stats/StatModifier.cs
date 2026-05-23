/// <summary>
/// 属性修改器类型。
/// </summary>
public enum ModifierType
{
    Flat,       // 固定值加成（加减）
    Percent     // 百分比加成（乘算）
}

/// <summary>
/// 单个属性修改器。
/// 可来自装备、buff、技能等。
/// </summary>
[System.Serializable]
public class StatModifier
{
    public ModifierType type;
    public float value;
    public object source;   // 来源标识，用于移除特定来源的修改器

    public StatModifier(ModifierType type, float value, object source = null)
    {
        this.type = type;
        this.value = value;
        this.source = source;
    }
}
