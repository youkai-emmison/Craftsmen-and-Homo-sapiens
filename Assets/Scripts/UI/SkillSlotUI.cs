using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 技能装备槽 HUD —— 显示 4 个技能槽位的图标、冷却遮罩和按键提示。
/// 挂在 Canvas 下的技能栏 UI 上，监听 SkillManager 事件自动刷新。
/// </summary>
public class SkillSlotUI : MonoBehaviour
{
    [Header("引用")]
    [SerializeField] private SkillManager skillManager;

    [Header("槽位 UI（按顺序对应 Y/U/I/O）")]
    [SerializeField] private SkillSlotElement[] slots;

    [System.Serializable]
    public class SkillSlotElement
    {
        public Image iconImage;           // 技能图标
        public Image cooldownOverlay;     // 冷却遮罩（Filled 类型，从上到下填充）
        public TextMeshProUGUI keyLabel;  // 按键提示文字
        public GameObject emptyIndicator; // 无技能时显示的占位符
    }

    private void Start()
    {
        if (skillManager == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
                skillManager = player.GetComponent<SkillManager>();
        }

        RefreshAll();
    }

    private void OnEnable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillEquipped += HandleEquippedChanged;
            skillManager.OnCooldownChanged += HandleCooldownChanged;
            skillManager.OnSkillCast += HandleSkillCast;
        }
    }

    private void OnDisable()
    {
        if (skillManager != null)
        {
            skillManager.OnSkillEquipped -= HandleEquippedChanged;
            skillManager.OnCooldownChanged -= HandleCooldownChanged;
            skillManager.OnSkillCast -= HandleSkillCast;
        }
    }

    private void Update()
    {
        // 实时更新冷却遮罩（事件驱动有延迟，逐帧刷新更平滑）
        if (skillManager == null || slots == null) return;

        for (int i = 0; i < SkillManager.SlotCount && i < slots.Length; i++)
        {
            if (slots[i] == null) continue;
            UpdateCooldownVisual(i);
        }
    }

    private void RefreshAll()
    {
        if (skillManager == null || slots == null) return;

        string[] keyLabels = { "Y", "U", "I", "O" };

        for (int i = 0; i < SkillManager.SlotCount && i < slots.Length; i++)
        {
            var slot = slots[i];
            if (slot == null) continue;

            var skill = skillManager.GetEquipped(i);

            // 按键标签
            if (slot.keyLabel != null)
                slot.keyLabel.text = keyLabels[i];

            UpdateSlotVisual(slot, skill);
        }
    }

    private void UpdateSlotVisual(SkillSlotElement slot, SkillData skill)
    {
        bool hasSkill = skill != null;

        // 空槽位指示器
        if (slot.emptyIndicator != null)
            slot.emptyIndicator.SetActive(!hasSkill);

        // 技能图标
        if (slot.iconImage != null)
        {
            slot.iconImage.enabled = hasSkill;
            if (hasSkill && skill.icon != null)
                slot.iconImage.sprite = skill.icon;
        }

        // 冷却遮罩初始状态
        if (slot.cooldownOverlay != null)
        {
            slot.cooldownOverlay.fillAmount = 0f;
            slot.cooldownOverlay.enabled = hasSkill;
        }
    }

    private void UpdateCooldownVisual(int slotIndex)
    {
        if (slotIndex >= slots.Length || slots[slotIndex] == null) return;

        float progress = skillManager.GetCooldownProgress(slotIndex);
        if (slots[slotIndex].cooldownOverlay != null)
            slots[slotIndex].cooldownOverlay.fillAmount = progress;
    }

    private void HandleEquippedChanged(int slot, SkillData skill)
    {
        if (slots == null || slot < 0 || slot >= slots.Length || slots[slot] == null) return;
        UpdateSlotVisual(slots[slot], skill);
    }

    private void HandleCooldownChanged(int slot, float remaining, float total)
    {
        // Update() 已逐帧刷新，此处可扩展为数字倒计时显示
    }

    private void HandleSkillCast(int slot)
    {
        // 可扩展：施放动画、音效等
    }
}
