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
    [SerializeField] private Button skillTreeTabButton;
    [SerializeField] private Button settingsTabButton;

    [Header("Tab Panels")]
    [SerializeField] private GameObject backpackContent;
    [SerializeField] private GameObject craftingContent;
    [SerializeField] private GameObject skillTreeContent;
    [SerializeField] private GameObject settingsContent;

    [Header("Backpack Slots")]
    [SerializeField] private InventorySlotView[] slotViews;
    [SerializeField] private ScrollableInventoryList scrollableInventoryList;

    [Header("Equipment Slots")]
    [SerializeField] private EquipmentSlotView[] equipmentSlots;

    [Header("Weapon Slots")]
    [SerializeField] private EquipmentSlotView[] weaponSlotViews;

    [Header("Character Info")]
    [SerializeField] private CharacterInfoPanel characterInfoPanel;

    [Header("Crafting")]
    [SerializeField] private CraftingPanel craftingPanel;

    public bool IsVisible { get; private set; }

    private PlayerStats playerStats;
    private int currentTab; // 0=背包, 1=制作, 2=技能树, 3=设置

    private void Awake()
    {
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
        ResolveReferences();

        InitializeSlots();
        SwitchTab(0);
        if (characterInfoPanel != null) characterInfoPanel.Initialize(playerStats);
    }

    private void ResolveReferences()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("InventoryPanel: 找不到 tag=Player 的对象");
            return;
        }

        if (inventory == null) inventory = player.GetComponent<Inventory>();
        if (equipment == null) equipment = player.GetComponent<Equipment>();
        if (playerStats == null) playerStats = player.GetComponent<PlayerStats>();

        // 订阅事件（Start 中才拿到引用，所以在这里订阅）
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshBackpack;
            inventory.OnEquipmentChanged += RefreshEquipment;
        }
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

    /// <summary>
    /// 按快捷键时：如果面板未打开则打开并切到指定 Tab，如果已打开且就在该 Tab 则关闭面板，否则切到该 Tab。
    /// </summary>
    public void ToggleTab(int tab)
    {
        if (!IsVisible)
        {
            SetVisible(true);
            SwitchTab(tab);
        }
        else if (currentTab == tab)
        {
            Close();
        }
        else
        {
            SwitchTab(tab);
        }
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
        if (skillTreeContent != null) skillTreeContent.SetActive(tab == 2);
        if (settingsContent != null) settingsContent.SetActive(tab == 3);

        if (tab == 0) RefreshAll();
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

        // 武器槽
        if (weaponSlotViews != null)
        {
            foreach (var wpnSlot in weaponSlotViews)
            {
                if (wpnSlot == null) continue;
                wpnSlot.Initialize(inventory, equipment, tooltip);
            }
        }

        // Tab 按钮
        if (backpackTabButton != null)
            backpackTabButton.onClick.AddListener(() => SwitchTab(0));
        if (craftingTabButton != null)
            craftingTabButton.onClick.AddListener(() => SwitchTab(1));
        if (skillTreeTabButton != null)
            skillTreeTabButton.onClick.AddListener(() => SwitchTab(2));
        if (settingsTabButton != null)
            settingsTabButton.onClick.AddListener(() => SwitchTab(3));
    }

    private void RefreshAll()
    {
        RefreshBackpack();
        RefreshEquipment();
        if (characterInfoPanel != null) characterInfoPanel.Refresh();
    }

    private void RefreshBackpack()
    {
        if (inventory == null) { Debug.LogWarning("RefreshBackpack: inventory is null"); return; }


        // 新版：滚动列表
        if (scrollableInventoryList != null)
        {
            scrollableInventoryList.Refresh(inventory, OnSlotClicked, OnSlotDiscard);
            if (emptyMessageText != null)
                emptyMessageText.enabled = inventory.EquipmentSlots.Count == 0 && inventory.MaterialDict.Count == 0;
            return;
        }

        // 旧版：固定格子
        if (slotViews == null) return;

        var equipmentSlots_data = inventory.EquipmentSlots;
        var materialDict = inventory.MaterialDict;

        int slotIdx = 0;
        for (int i = 0; i < equipmentSlots_data.Count && slotIdx < slotViews.Length; i++, slotIdx++)
        {
            if (slotViews[slotIdx] != null)
                slotViews[slotIdx].SetEquipment(equipmentSlots_data[i]);
        }

        foreach (var kvp in materialDict)
        {
            if (slotIdx >= slotViews.Length) break;
            if (slotViews[slotIdx] != null)
                slotViews[slotIdx].SetMaterial(kvp.Key, kvp.Value);
            slotIdx++;
        }

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
        if (equipmentSlots != null)
        {
            foreach (var eqSlot in equipmentSlots)
            {
                if (eqSlot != null) eqSlot.Refresh();
            }
        }
        if (weaponSlotViews != null)
        {
            foreach (var wpnSlot in weaponSlotViews)
            {
                if (wpnSlot != null) wpnSlot.Refresh();
            }
        }
    }

    private void OnSlotClicked(int slotIndex)
    {
        if (inventory == null || equipment == null) return;

        var equipmentList = inventory.EquipmentSlots;
        if (slotIndex < equipmentList.Count)
        {
            equipment.EquipFromSlot(slotIndex);
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
