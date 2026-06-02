using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能管理器 —— 挂在玩家上，统一管理技能的学习、装备和施放。
/// 负责输入分发（Y/U/I/O）、冷却计时、能量扣除、效果执行。
/// </summary>
public class SkillManager : MonoBehaviour
{
    #region 常量

    /// <summary>技能槽位数量。</summary>
    public const int SlotCount = 4;

    /// <summary>每个槽位对应的按键。</summary>
    private static readonly KeyCode[] SlotKeys = { KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O };

    #endregion

    #region Inspector

    [Header("技能点")]
    [SerializeField] private int currentSkillPoints = 3;
    [SerializeField] private int maxSkillPoints = 20;

    [Header("所有可学习技能（注册表）")]
    [SerializeField] private SkillData[] allSkills;

    #endregion

    #region 运行时状态

    // 已学习的技能集合
    private readonly HashSet<SkillData> learnedSkills = new HashSet<SkillData>();

    // 装备到 4 个槽位的技能
    private readonly SkillData[] equippedSkills = new SkillData[SlotCount];

    // 每个槽位的冷却计时器
    private readonly float[] cooldownTimers = new float[SlotCount];

    // 组件引用
    private PlayerSkillEnergy skillEnergy;
    private Entity entity;

    #endregion

    #region 事件

    /// <summary>技能被学习时触发。参数：learned SkillData</summary>
    public event Action<SkillData> OnSkillLearned;

    /// <summary>技能被装备时触发。参数：槽位索引, equipped SkillData（null 表示卸下）</summary>
    public event Action<int, SkillData> OnSkillEquipped;

    /// <summary>技能施放成功时触发。参数：槽位索引</summary>
    public event Action<int> OnSkillCast;

    /// <summary>冷却状态变化时触发。参数：槽位索引, 剩余冷却时间, 总冷却时间</summary>
    public event Action<int, float, float> OnCooldownChanged;

    /// <summary>技能点变化时触发。</summary>
    public event Action<int, int> OnSkillPointsChanged;

    #endregion

    #region 属性

    public int CurrentSkillPoints => currentSkillPoints;
    public int MaxSkillPoints => maxSkillPoints;
    public IReadOnlyList<SkillData> AllSkills => allSkills;
    public IReadOnlyList<SkillData> EquippedSkills => equippedSkills;

    #endregion

    #region 生命周期

    private void Awake()
    {
        skillEnergy = GetComponent<PlayerSkillEnergy>();
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        UpdateCooldowns();
        HandleInput();
        TickActiveEffects();
    }

    #endregion

    #region 输入

    private void HandleInput()
    {
        // 对话打开时不响应技能输入
        if (DialoguePanelController.IsAnyDialogueOpen)
            return;

        for (int i = 0; i < SlotCount; i++)
        {
            if (Input.GetKeyDown(SlotKeys[i]))
                TryCast(i);
        }
    }

    #endregion

    #region 冷却

    private void UpdateCooldowns()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < SlotCount; i++)
        {
            if (cooldownTimers[i] > 0f)
            {
                cooldownTimers[i] = Mathf.Max(0f, cooldownTimers[i] - dt);
                OnCooldownChanged?.Invoke(i, cooldownTimers[i],
                    equippedSkills[i] != null ? equippedSkills[i].cooldown : 0f);
            }
        }
    }

    private void TickActiveEffects()
    {
        if (entity == null) return;
        Transform caster = entity.transform;
        float dt = Time.deltaTime;

        for (int i = 0; i < SlotCount; i++)
        {
            var skill = equippedSkills[i];
            if (skill == null || skill.effect == null) continue;
            if (skill.effect.IsActive)
                skill.effect.Tick(caster, dt);
        }
    }

    #endregion

    #region 学习

    /// <summary>
    /// 尝试学习指定技能。检查前置技能和技能点。
    /// </summary>
    public bool LearnSkill(SkillData skill)
    {
        if (skill == null) return false;
        if (learnedSkills.Contains(skill)) return false;

        // 检查前置技能
        if (!skill.ArePrerequisitesMet(this))
        {
            Debug.Log($"学习 {skill.skillName} 失败：前置技能未满足。");
            return false;
        }

        // 检查技能点
        if (currentSkillPoints < skill.learnCost)
        {
            Debug.Log($"学习 {skill.skillName} 失败：技能点不足（需要 {skill.learnCost}，当前 {currentSkillPoints}）。");
            return false;
        }

        currentSkillPoints -= skill.learnCost;
        learnedSkills.Add(skill);

        OnSkillLearned?.Invoke(skill);
        OnSkillPointsChanged?.Invoke(currentSkillPoints, maxSkillPoints);

        Debug.Log($"已学习技能：{skill.skillName}，剩余技能点：{currentSkillPoints}");
        return true;
    }

    /// <summary>
    /// 检查是否已学习指定技能。
    /// </summary>
    public bool HasSkill(SkillData skill)
    {
        return skill != null && learnedSkills.Contains(skill);
    }

    /// <summary>
    /// 获取所有已学习的技能。
    /// </summary>
    public IEnumerable<SkillData> GetLearnedSkills()
    {
        return learnedSkills;
    }

    #endregion

    #region 装备

    /// <summary>
    /// 将技能装备到指定槽位。传入 null 可卸下该槽位的技能。
    /// </summary>
    public bool EquipSkill(int slot, SkillData skill)
    {
        if (slot < 0 || slot >= SlotCount) return false;

        // 卸下
        if (skill == null)
        {
            var old = equippedSkills[slot];
            equippedSkills[slot] = null;
            OnSkillEquipped?.Invoke(slot, null);
            return true;
        }

        // 必须已学习
        if (!learnedSkills.Contains(skill))
        {
            Debug.Log($"装备 {skill.skillName} 失败：尚未学习。");
            return false;
        }

        // 如果该技能已在其他槽位，交换
        for (int i = 0; i < SlotCount; i++)
        {
            if (i != slot && equippedSkills[i] == skill)
            {
                equippedSkills[i] = equippedSkills[slot];
                break;
            }
        }

        equippedSkills[slot] = skill;
        cooldownTimers[slot] = 0f;

        OnSkillEquipped?.Invoke(slot, skill);
        return true;
    }

    /// <summary>
    /// 获取指定槽位装备的技能。
    /// </summary>
    public SkillData GetEquipped(int slot)
    {
        if (slot < 0 || slot >= SlotCount) return null;
        return equippedSkills[slot];
    }

    #endregion

    #region 施放

    /// <summary>
    /// 尝试施放指定槽位的技能。
    /// </summary>
    public bool TryCast(int slot)
    {
        if (slot < 0 || slot >= SlotCount) return false;

        var skill = equippedSkills[slot];
        if (skill == null || skill.effect == null) return false;

        // 检查冷却
        if (cooldownTimers[slot] > 0f)
        {
            Debug.Log($"{skill.skillName} 冷却中（{cooldownTimers[slot]:F1}s）。");
            return false;
        }

        // 检查能量
        if (skillEnergy == null || !skillEnergy.TrySpendSkillEnergy(skill.energyCost))
        {
            Debug.Log($"{skill.skillName}：技力不足。");
            return false;
        }

        // 执行效果
        if (skill.effect.Activate(transform, this))
        {
            cooldownTimers[slot] = skill.cooldown;
            OnSkillCast?.Invoke(slot);
            OnCooldownChanged?.Invoke(slot, skill.cooldown, skill.cooldown);
            return true;
        }

        // 效果激活失败，退还能量
        skillEnergy.RestoreSkillEnergy(skill.energyCost);
        return false;
    }

    /// <summary>
    /// 获取指定槽位的剩余冷却时间。
    /// </summary>
    public float GetCooldownRemaining(int slot)
    {
        if (slot < 0 || slot >= SlotCount) return 0f;
        return cooldownTimers[slot];
    }

    /// <summary>
    /// 获取指定槽位的冷却进度（0~1，0 表示冷却完毕）。
    /// </summary>
    public float GetCooldownProgress(int slot)
    {
        var skill = equippedSkills[slot];
        if (skill == null || skill.cooldown <= 0f) return 0f;
        return cooldownTimers[slot] / skill.cooldown;
    }

    #endregion

    #region 技能点

    /// <summary>
    /// 增加技能点（升级时调用）。
    /// </summary>
    public void AddSkillPoints(int amount)
    {
        if (amount <= 0) return;
        currentSkillPoints = Mathf.Min(maxSkillPoints, currentSkillPoints + amount);
        OnSkillPointsChanged?.Invoke(currentSkillPoints, maxSkillPoints);
    }

    #endregion
}
