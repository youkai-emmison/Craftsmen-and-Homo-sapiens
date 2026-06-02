using UnityEngine;

/// <summary>
/// 技能效果抽象基类 —— 定义技能激活/取消的逻辑接口。
/// 每个具体技能效果继承此类并实现 Activate。
/// </summary>
public abstract class SkillEffect : ScriptableObject
{
    [Header("特效信息")]
    public string effectName;
    [TextArea] public string description;

    /// <summary>
    /// 激活技能效果。
    /// </summary>
    /// <param name="caster">施法者 Transform</param>
    /// <param name="manager">SkillManager 引用，用于访问施法者组件</param>
    /// <returns>是否成功激活</returns>
    public abstract bool Activate(Transform caster, SkillManager manager);

    /// <summary>
    /// 取消技能效果（用于 buff 类技能的提前取消或过期）。
    /// </summary>
    public virtual void Deactivate(Transform caster) { }

    /// <summary>
    /// 当前是否处于激活状态（buff 类技能重写此属性）。
    /// </summary>
    public virtual bool IsActive => false;

    /// <summary>
    /// 每帧更新持续效果（buff 类技能重写此方法）。
    /// 由 SkillManager 在 Update 中对已装备且 IsActive 的效果调用。
    /// </summary>
    public virtual void Tick(Transform caster, float deltaTime) { }
}
