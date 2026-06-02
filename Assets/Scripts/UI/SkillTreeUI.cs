using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 技能树 UI —— 显示所有可学习技能，支持前置依赖检查、学习和装备。
/// 挂在 InventoryPanel 的 skillTreeContent 下。
/// </summary>
public class SkillTreeUI : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] private SkillManager skillManager;

    [Header("UI 模板")]
    [SerializeField] private Transform nodeListParent;       // 技能节点的父容器
    [SerializeField] private SkillNodeView nodeTemplate;     // 节点模板（运行时隐藏）

    [Header("装备槽 UI")]
    [SerializeField] private SkillEquipSlotView[] equipSlots; // 4 个装备槽视图

    [Header("信息面板")]
    [SerializeField] private GameObject infoPanel;            // 选中技能时显示详情
    [SerializeField] private Image infoIcon;
    [SerializeField] private TextMeshProUGUI infoNameText;
    [SerializeField] private TextMeshProUGUI infoDescText;
    [SerializeField] private TextMeshProUGUI infoCostText;
    [SerializeField] private TextMeshProUGUI infoPrereqText;
    [SerializeField] private Button infoLearnButton;
    [SerializeField] private TextMeshProUGUI infoLearnButtonText;
    [SerializeField] private TextMeshProUGUI infoEquipStatusText; // 装备状态文本
    [SerializeField] private TextMeshProUGUI skillPointsText;

    // 运行时生成的节点列表
    private readonly List<SkillNodeView> spawnedNodes = new List<SkillNodeView>();
    private SkillData selectedSkill;

    #region 子类定义

    /// <summary>
    /// 单个技能节点的 UI 视图。
    /// </summary>
    [System.Serializable]
    public class SkillNodeView
    {
        public GameObject root;
        public Image iconImage;
        public TextMeshProUGUI nameText;
        public Button button;
        public GameObject learnedIndicator;   // 已学习标记
        public GameObject lockedOverlay;      // 未满足前置时的遮罩
    }

    /// <summary>
    /// 装备槽视图（显示已装备技能的图标和按键提示）。
    /// </summary>
    [System.Serializable]
    public class SkillEquipSlotView
    {
        public Image iconImage;
        public TextMeshProUGUI keyLabel;
        public Button button;
    }

    #endregion

    #region 生命周期

    private void Start()
    {
        if (skillManager == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
                skillManager = player.GetComponent<SkillManager>();
        }

        if (nodeTemplate != null && nodeTemplate.root != null)
            nodeTemplate.root.SetActive(false);

        InitializeEquipSlots();
        Refresh();
    }

    private void OnEnable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLearned += _ => Refresh();
            skillManager.OnSkillEquipped += (_, __) => Refresh();
            skillManager.OnSkillPointsChanged += (_, __) => RefreshPointsDisplay();
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillLearned -= _ => Refresh();
            skillManager.OnSkillEquipped -= (_, __) => Refresh();
            skillManager.OnSkillPointsChanged -= (_, __) => RefreshPointsDisplay();
        }
    }

    #endregion

    #region 初始化

    private void InitializeEquipSlots()
    {
        string[] keyLabels = { "Y", "U", "I", "O" };
        if (equipSlots == null) return;

        for (int i = 0; i < equipSlots.Length && i < SkillManager.SlotCount; i++)
        {
            var slot = equipSlots[i];
            if (slot == null) continue;

            if (slot.keyLabel != null)
                slot.keyLabel.text = keyLabels[i];

            int slotIndex = i; // 捕获闭包
            if (slot.button != null)
                slot.button.onClick.AddListener(() => OnEquipSlotClicked(slotIndex));
        }
    }

    #endregion

    #region 刷新

    public void Refresh()
    {
        RefreshNodeList();
        RefreshEquipSlots();
        RefreshPointsDisplay();
        RefreshInfoPanel();
    }

    private void RefreshNodeList()
    {
        if (skillManager == null || skillManager.AllSkills == null) return;

        // 确保节点数量足够
        var allSkills = skillManager.AllSkills;
        EnsureNodeCount(allSkills.Count);

        for (int i = 0; i < allSkills.Count; i++)
        {
            var skill = allSkills[i];
            var node = spawnedNodes[i];
            if (node == null || node.root == null) continue;

            node.root.SetActive(true);

            bool learned = skillManager.HasSkill(skill);
            bool prereqMet = skill.ArePrerequisitesMet(skillManager);

            // 图标
            if (node.iconImage != null && skill.icon != null)
                node.iconImage.sprite = skill.icon;

            // 名称
            if (node.nameText != null)
                node.nameText.text = skill.skillName;

            // 已学习标记
            if (node.learnedIndicator != null)
                node.learnedIndicator.SetActive(learned);

            // 锁定遮罩（未学习且前置不满足）
            if (node.lockedOverlay != null)
                node.lockedOverlay.SetActive(!learned && !prereqMet);

            // 点击事件
            if (node.button != null)
            {
                node.button.onClick.RemoveAllListeners();
                var capturedSkill = skill; // 闭包捕获
                node.button.onClick.AddListener(() => OnNodeClicked(capturedSkill));
            }
        }

        // 隐藏多余的节点
        for (int i = allSkills.Count; i < spawnedNodes.Count; i++)
        {
            if (spawnedNodes[i] != null && spawnedNodes[i].root != null)
                spawnedNodes[i].root.SetActive(false);
        }
    }

    private void EnsureNodeCount(int count)
    {
        if (nodeTemplate == null || nodeTemplate.root == null || nodeListParent == null) return;

        while (spawnedNodes.Count < count)
        {
            var instance = Instantiate(nodeTemplate.root, nodeListParent);
            instance.SetActive(true);

            var view = new SkillNodeView
            {
                root = instance,
                iconImage = instance.transform.Find("Icon")?.GetComponent<Image>()
                           ?? instance.GetComponentInChildren<Image>(),
                nameText = instance.GetComponentInChildren<TextMeshProUGUI>(),
                button = instance.GetComponent<Button>(),
                // 这些需要按命名约定查找子对象
                learnedIndicator = instance.transform.Find("LearnedIndicator")?.gameObject,
                lockedOverlay = instance.transform.Find("LockedOverlay")?.gameObject
            };

            spawnedNodes.Add(view);
        }
    }

    private void RefreshEquipSlots()
    {
        if (skillManager == null || equipSlots == null) return;

        for (int i = 0; i < equipSlots.Length && i < SkillManager.SlotCount; i++)
        {
            var slot = equipSlots[i];
            if (slot == null) continue;

            var skill = skillManager.GetEquipped(i);
            bool hasSkill = skill != null;

            if (slot.iconImage != null)
            {
                slot.iconImage.enabled = hasSkill;
                if (hasSkill && skill.icon != null)
                    slot.iconImage.sprite = skill.icon;
            }
        }
    }

    private void RefreshPointsDisplay()
    {
        if (skillPointsText == null || skillManager == null) return;
        skillPointsText.text = $"Skill Points: {skillManager.CurrentSkillPoints} / {skillManager.MaxSkillPoints}";
    }

    private void RefreshInfoPanel()
    {
        if (selectedSkill == null)
        {
            if (infoPanel != null) infoPanel.SetActive(false);
            return;
        }

        if (infoPanel != null) infoPanel.SetActive(true);

        bool learned = skillManager != null && skillManager.HasSkill(selectedSkill);

        if (infoIcon != null && selectedSkill.icon != null)
            infoIcon.sprite = selectedSkill.icon;

        if (infoNameText != null)
            infoNameText.text = selectedSkill.skillName;

        if (infoDescText != null)
            infoDescText.text = selectedSkill.description;

        if (infoCostText != null)
            infoCostText.text = $"Cost: {selectedSkill.energyCost} EP | Cooldown: {selectedSkill.cooldown}s";

        if (infoPrereqText != null)
        {
            if (selectedSkill.prerequisites != null && selectedSkill.prerequisites.Length > 0)
            {
                var prereqNames = new List<string>();
                foreach (var prereq in selectedSkill.prerequisites)
                {
                    if (prereq != null)
                    {
                        bool met = skillManager != null && skillManager.HasSkill(prereq);
                        prereqNames.Add(met ? $"<color=green>{prereq.skillName}</color>" : $"<color=red>{prereq.skillName}</color>");
                    }
                }
                infoPrereqText.text = $"Prerequisites: {string.Join(", ", prereqNames)}";
            }
            else
            {
                infoPrereqText.text = "Prerequisites: None";
            }
        }

        if (infoLearnButton != null)
        {
            if (learned)
            {
                infoLearnButton.gameObject.SetActive(false);
            }
            else
            {
                infoLearnButton.gameObject.SetActive(true);
                bool canLearn = selectedSkill.ArePrerequisitesMet(skillManager)
                    && skillManager.CurrentSkillPoints >= selectedSkill.learnCost;
                infoLearnButton.interactable = canLearn;
                if (infoLearnButtonText != null)
                    infoLearnButtonText.text = canLearn ? $"Learn ({selectedSkill.learnCost} pts)" : "Requirements Not Met";
            }
        }

        // 显示装备状态
        if (infoEquipStatusText != null)
        {
            if (!learned)
            {
                infoEquipStatusText.text = "";
            }
            else
            {
                int equippedSlot = -1;
                for (int i = 0; i < SkillManager.SlotCount; i++)
                {
                    if (skillManager.GetEquipped(i) == selectedSkill)
                    {
                        equippedSlot = i;
                        break;
                    }
                }

                if (equippedSlot >= 0)
                {
                    string[] keyLabels = { "Y", "U", "I", "O" };
                    infoEquipStatusText.text = $"Equipped to [{keyLabels[equippedSlot]}] - Click slot to unequip";
                }
                else
                {
                    infoEquipStatusText.text = "Not equipped - Click a slot to equip";
                }
            }
        }
    }

    #endregion

    #region 交互

    private void OnNodeClicked(SkillData skill)
    {
        selectedSkill = skill;
        RefreshInfoPanel();
    }

    /// <summary>
    /// 信息面板的"学习"按钮回调。
    /// </summary>
    public void OnLearnButtonClicked()
    {
        if (selectedSkill == null || skillManager == null) return;
        skillManager.LearnSkill(selectedSkill);
    }

    private void OnEquipSlotClicked(int slotIndex)
    {
        if (skillManager == null) return;

        // 如果有选中的已学技能，装备到该槽位
        if (selectedSkill != null && skillManager.HasSkill(selectedSkill))
        {
            skillManager.EquipSkill(slotIndex, selectedSkill);
        }
        // 否则，如果该槽位已有技能，卸下
        else if (skillManager.GetEquipped(slotIndex) != null)
        {
            skillManager.EquipSkill(slotIndex, null);
        }
    }

    #endregion
}
