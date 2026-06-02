using UnityEngine;

/// <summary>
/// 技能数据定义 —— ScriptableObject，描述一个技能的所有静态属性。
/// 通过 Unity Inspector 的 Create → Skills → Skill Data 菜单创建。
/// </summary>
[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    [Header("基本信息")]
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("技能属性")]
    public int energyCost = 1;          // 施放消耗技力点
    public float cooldown = 1f;         // 冷却时间（秒）
    public int learnCost = 1;           // 学习消耗技能点

    [Header("技能树")]
    public SkillData[] prerequisites;   // 前置技能（必须全部已学才能学习本技能）

    [Header("效果")]
    public SkillEffect effect;          // 效果逻辑引用

    /// <summary>
    /// 检查是否满足所有前置技能要求。
    /// </summary>
    public bool ArePrerequisitesMet(SkillManager manager)
    {
        if (prerequisites == null || prerequisites.Length == 0)
            return true;

        foreach (var prereq in prerequisites)
        {
            if (prereq != null && !manager.HasSkill(prereq))
                return false;
        }
        return true;
    }
}
