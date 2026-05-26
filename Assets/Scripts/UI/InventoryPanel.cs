using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [Header("System References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Equipment equipment;

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private ItemTooltip tooltip;
    [SerializeField] private Text emptyMessageText;

    [Header("Tab Buttons")]
    [SerializeField] private Button backpackTabButton;
    [SerializeField] private Button craftingTabButton;
    [SerializeField] private GameObject backpackContent;
    [SerializeField] private GameObject craftingContent;

    [Header("Backpack Slots")]
    [SerializeField] private InventorySlotView[] slotViews;

    [Header("Equipment Slots")]
    [SerializeField] private EquipmentSlotView[] equipmentSlots;

    [Header("Character Info")]
    [SerializeField] private CharacterInfoPanel characterInfoPanel;

    [Header("Crafting")]
    [SerializeField] private CraftingPanel craftingPanel;

    public bool IsVisible { get; private set; }

    private PlayerStats playerStats;
    private int currentTab; // 0=背包, 1=制作

    private void Awake()
    {
        if (inventory == null) inventory = GetComponentInParent<Inventory>();
        if (equipment == null) equipment = GetComponentInParent<Equipment>();

        if (inventory != null)
            playerStats = inventory.GetComponentInParent<PlayerStats>();

        SetVisible(false);
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshBackpack;
            inventory.OnEquipmentChanged += RefreshEquipment;
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshBackpack;
            inventory.OnEquipmentChanged -= RefreshEquipment;
        }
    }

    private void Start()
    {
        InitializeSlots();
        SwitchTab(0);
        if (characterInfoPanel != null) characterInfoPanel.Initialize(playerStats);
    }

    private void Update()
    {
        // 实时刷新角色属性面板（因为 HP 等会变化）
        if (IsVisible && currentTab == 0 && characterInfoPanel != null)
            characterInfoPanel.Refresh();
    }

    public void Toggle()
    {
        SetVisible(!IsVisible);
        if (IsVisible) RefreshAll();
    }

    public void Close()
    {
        SetVisible(false);
        if (tooltip != null) tooltip.Hide();
    }

    public void SetVisible(bool visible)
    {
        IsVisible = visible;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
        if (visible) RefreshAll();
    }

    public void SwitchTab(int tab)
    {
        currentTab = tab;
        if (backpackContent != null) backpackContent.SetActive(tab == 0);
        if (craftingContent != null) craftingContent.SetActive(tab == 1);

        if (tab == 1 && craftingPanel != null)
            craftingPanel.RefreshRecipeList();
    }

    private void InitializeSlots()
    {
        // 背包格子
        if (slotViews != null)
        {
            for (int i = 0; i < slotViews.Length; i++)
            {
                if (slotViews[i] == null) continue;
                slotViews[i].SlotIndex = i;
                slotViews[i].SetTooltip(tooltip);
                slotViews[i].SetCallbacks(OnSlotClicked, OnSlotDiscard);
            }
        }

        // 装备栏
        if (equipmentSlots != null)
        {
            foreach (var eqSlot in equipmentSlots)
            {
                if (eqSlot == null) continue;
                eqSlot.Initialize(inventory, equipment, tooltip);
            }
        }

        // Tab 按钮
        if (backpackTabButton != null)
            backpackTabButton.onClick.AddListener(() => SwitchTab(0));
        if (craftingTabButton != null)
            craftingTabButton.onClick.AddListener(() => SwitchTab(1));
    }

    private void RefreshAll()
    {
        RefreshBackpack();
        RefreshEquipment();
        if (characterInfoPanel != null) characterInfoPanel.Refresh();
    }

    private void RefreshBackpack()
    {
        if (inventory == null || slotViews == null) return;

        var equipmentSlots_data = inventory.EquipmentSlots;
        var materialDict = inventory.MaterialDict;

        // 先显示装备
        int slotIdx = 0;
        for (int i = 0; i < equipmentSlots_data.Count && slotIdx < slotViews.Length; i++, slotIdx++)
        {
            if (slotViews[slotIdx] != null)
                slotViews[slotIdx].SetEquipment(equipmentSlots_data[i]);
        }

        // 再显示材料
        foreach (var kvp in materialDict)
        {
            if (slotIdx >= slotViews.Length) break;
            if (slotViews[slotIdx] != null)
                slotViews[slotIdx].SetMaterial(kvp.Key, kvp.Value);
            slotIdx++;
        }

        // 剩余格子置空
        for (; slotIdx < slotViews.Length; slotIdx++)
        {
            if (slotViews[slotIdx] != null)
                slotViews[slotIdx].SetEmpty();
        }

        if (emptyMessageText != null)
            emptyMessageText.enabled = equipmentSlots_data.Count == 0 && materialDict.Count == 0;
    }

    private void RefreshEquipment()
    {
        if (equipmentSlots == null) return;
        foreach (var eqSlot in equipmentSlots)
        {
            if (eqSlot != null) eqSlot.Refresh();
        }
    }

    private void OnSlotClicked(int slotIndex)
    {
        if (inventory == null) return;

        // 计算该 slotIndex 对应的是装备还是材料
        var equipmentList = inventory.EquipmentSlots;
        if (slotIndex < equipmentList.Count)
        {
            // 点击装备 → 穿戴
            equipment?.EquipFromSlot(slotIndex);
        }
        else
        {
            // 点击材料 → 无操作（或可扩展为使用）
        }
    }

    private void OnSlotDiscard(int slotIndex)
    {
        if (inventory == null) return;

        var equipmentList = inventory.EquipmentSlots;
        if (slotIndex < equipmentList.Count)
        {
            // Ctrl+Click 丢弃装备
            inventory.DiscardEquipment(equipmentList[slotIndex]);
        }
    }
}
